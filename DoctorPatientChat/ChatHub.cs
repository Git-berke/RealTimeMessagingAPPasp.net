using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using System.Collections.Concurrent;
using System.Data;

namespace DoctorPatientChat
{
    public class ChatHub : Hub
    {
        // Kullanıcı bağlantılarını saklamak için statik sözlük
        private static readonly ConcurrentDictionary<string, string> UserConnections = new ConcurrentDictionary<string, string>();
        private static readonly ConcurrentDictionary<string, string> TypingUsers = new ConcurrentDictionary<string, string>();

        // Kullanıcı bağlandığında
        public override Task OnConnected()
        {
            // Kullanıcı ID'sini al
            string userId = Context.QueryString["userId"];
            if (!string.IsNullOrEmpty(userId))
            {
                // Kullanıcıyı bağlantı listesine ekle
                UserConnections.TryAdd(Context.ConnectionId, userId);
                
                // Kullanıcıyı bir gruba ekle
                Groups.Add(Context.ConnectionId, userId);
                
                // Okunmamış mesaj sayısını gönder
                int unreadCount = MessageManager.GetUnreadMessageCount(Convert.ToInt32(userId));
                Clients.Caller.updateUnreadCount(unreadCount);
            }
            return base.OnConnected();
        }

        // Kullanıcı bağlantısı kesildiğinde
        public override Task OnDisconnected(bool stopCalled)
        {
            string userId;
            if (UserConnections.TryRemove(Context.ConnectionId, out userId))
            {
                // Kullanıcıyı gruptan çıkar
                Groups.Remove(Context.ConnectionId, userId);
                string typingReceiver;
                if (TypingUsers.TryRemove(Context.ConnectionId, out typingReceiver))
                {
                    Clients.Group(typingReceiver).userStoppedTyping(userId);
                }
            }
            return base.OnDisconnected(stopCalled);
        }

        public void LoadMessageHistory(string receiverId)
        {
            try
            {
                string senderId;
                if (!UserConnections.TryGetValue(Context.ConnectionId, out senderId))
                {
                    Clients.Caller.showError("Geçersiz kullanıcı oturumu.");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"Loading messages between {senderId} and {receiverId}");

                var messages = MessageManager.GetMessagesBetweenUsers(
                    Convert.ToInt32(senderId),
                    Convert.ToInt32(receiverId)
                );

                if (messages != null && messages.Rows.Count > 0)
                {
                    foreach (DataRow row in messages.Rows)
                    {
                        bool isSent = Convert.ToInt32(row["SenderID"]) == Convert.ToInt32(senderId);
                        string senderName = isSent ? "Siz" : row["SenderName"].ToString();
                        
                        System.Diagnostics.Debug.WriteLine($"Sending message: Sender={senderName}, Message={row["MessageText"]}, Time={row["Timestamp"]}");
                        
                        Clients.Caller.addMessageToHistory(
                            senderName,
                            row["MessageText"].ToString(),
                            Convert.ToDateTime(row["Timestamp"]).ToString("HH:mm"),
                            isSent
                        );
                    }

                    // Mesajları okundu olarak işaretle
                    MessageManager.MarkMessagesAsRead(
                        Convert.ToInt32(senderId),
                        Convert.ToInt32(receiverId)
                    );
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No messages found or messages is null");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"LoadMessageHistory Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                Clients.Caller.showError("Mesaj geçmişi yüklenirken bir hata oluştu.");
            }
        }

        // Mesaj gönderme
        public void SendMessage(string receiverId, string message)
        {
            try
            {
                string senderId;
                if (!UserConnections.TryGetValue(Context.ConnectionId, out senderId))
                {
                    Clients.Caller.showError("Geçersiz kullanıcı oturumu.");
                    return;
                }

                if (string.IsNullOrEmpty(receiverId))
                {
                    Clients.Caller.showError("Geçersiz alıcı bilgisi.");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"Sending message: From={senderId} To={receiverId} Message={message}");

                bool success = MessageManager.SaveMessage(
                    Convert.ToInt32(senderId),
                    Convert.ToInt32(receiverId),
                    message
                );

                if (success)
                {
                    string timestamp = DateTime.Now.ToString("HH:mm");
                    Clients.Group(receiverId).receiveMessage("Yeni Mesaj", message, timestamp);
                    Clients.Caller.messageSent(receiverId, message, timestamp);
                }
                else
                {
                    Clients.Caller.showError("Mesaj kaydedilemedi.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SendMessage Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                Clients.Caller.showError("Mesaj gönderilirken bir hata oluştu.");
            }
        }

        // Yazıyor sinyali gönderme
        public void StartTyping(string receiverId)
        {
            string senderId;
            if (UserConnections.TryGetValue(Context.ConnectionId, out senderId))
            {
                TypingUsers.TryAdd(Context.ConnectionId, receiverId);
                Clients.Group(receiverId).userTyping(senderId);
            }
        }

        public void StopTyping(string receiverId)
        {
            string senderId;
            if (UserConnections.TryGetValue(Context.ConnectionId, out senderId))
            {
                string removedReceiver;
                TypingUsers.TryRemove(Context.ConnectionId, out removedReceiver);
                Clients.Group(receiverId).userStoppedTyping(senderId);
            }
        }

        // Mesajları okundu olarak işaretleme
        public void MarkMessagesAsRead(string senderId)
        {
            try
            {
                string receiverId;
                if (!UserConnections.TryGetValue(Context.ConnectionId, out receiverId))
                {
                    Clients.Caller.showError("Geçersiz kullanıcı oturumu.");
                    return;
                }

                if (string.IsNullOrEmpty(senderId))
                {
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"Marking messages as read: From={senderId} To={receiverId}");

                // Mesajları okundu olarak işaretle
                bool success = MessageManager.MarkMessagesAsRead(
                    Convert.ToInt32(receiverId),
                    Convert.ToInt32(senderId)
                );

                if (success)
                {
                    // Gönderene mesajların okunduğu bilgisini gönder
                    Clients.Group(senderId).messagesRead(receiverId);
                    
                    // Okunmamış mesaj sayısını güncelle
                    int unreadCount = MessageManager.GetUnreadMessageCount(Convert.ToInt32(receiverId));
                    Clients.Caller.updateUnreadCount(unreadCount);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MarkMessagesAsRead Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        // Mesajlarda arama
        public void SearchMessages(string receiverId, string searchText)
        {
            try
            {
                string senderId;
                if (!UserConnections.TryGetValue(Context.ConnectionId, out senderId))
                {
                    Clients.Caller.showError("Geçersiz kullanıcı oturumu.");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"Searching messages between {senderId} and {receiverId} for text: {searchText}");

                var messages = MessageManager.SearchMessages(
                    Convert.ToInt32(senderId),
                    Convert.ToInt32(receiverId),
                    searchText
                );

                // Mesaj kutusunu temizle
                Clients.Caller.clearMessageContainer();

                if (messages != null && messages.Rows.Count > 0)
                {
                    foreach (DataRow row in messages.Rows)
                    {
                        bool isSent = Convert.ToInt32(row["SenderID"]) == Convert.ToInt32(senderId);
                        string senderName = isSent ? "Siz" : row["SenderName"].ToString();
                        bool isRead = Convert.ToBoolean(row["IsRead"]);
                        
                        Clients.Caller.addMessageToSearch(
                            senderName,
                            row["MessageText"].ToString(),
                            Convert.ToDateTime(row["Timestamp"]).ToString("HH:mm"),
                            isSent,
                            isRead,
                            searchText
                        );
                    }
                }
                else
                {
                    Clients.Caller.showNoSearchResults(searchText);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"SearchMessages Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                Clients.Caller.showError("Mesaj araması sırasında bir hata oluştu.");
            }
        }
    }
}