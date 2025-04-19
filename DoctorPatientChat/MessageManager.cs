using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;

namespace DoctorPatientChat
{
    public class MessageManager
    {
        // Yeni mesaj kaydetme
        public static bool SaveMessage(int senderId, int receiverId, string messageText)
        {
            try
            {
                // Tablo yapısını kontrol et
                var tableStructure = DatabaseConnection.GetTableStructure("CHAT");
                if (tableStructure == null)
                {
                    Debug.WriteLine("CHAT tablosu bulunamadı veya erişilemedi.");
                    return false;
                }

                Debug.WriteLine("CHAT tablosu sütunları:");
                foreach (DataColumn column in tableStructure.Columns)
                {
                    Debug.WriteLine($"- {column.ColumnName} ({column.DataType})");
                }

                string query = "INSERT INTO [CHAT] (SenderID, ReceiverID, MessageText, [Timestamp], IsRead) VALUES (?, ?, ?, ?, ?)";
                OleDbParameter[] parameters = {
                    new OleDbParameter("@SenderID", OleDbType.Integer) { Value = senderId },
                    new OleDbParameter("@ReceiverID", OleDbType.Integer) { Value = receiverId },
                    new OleDbParameter("@MessageText", OleDbType.VarChar) { Value = messageText },
                    new OleDbParameter("@Timestamp", OleDbType.Date) { Value = DateTime.Now },
                    new OleDbParameter("@IsRead", OleDbType.Boolean) { Value = false }
                };

                int result = DatabaseConnection.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"SaveMessage Error: {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return false;
            }
        }

        // İki kullanıcı arasındaki mesajları getir
        public static DataTable GetMessagesBetweenUsers(int userId1, int userId2)
        {
            try
            {
                Debug.WriteLine($"Getting messages between users {userId1} and {userId2}");

                string query = @"SELECT 
                                 c.MessageID, 
                                 c.SenderID, 
                                 c.ReceiverID, 
                                 c.MessageText, 
                                 c.Timestamp, 
                                 c.IsRead,
                                 u1.Username AS SenderName, 
                                 u2.Username AS ReceiverName 
                               FROM 
                                 (CHAT c 
                                 INNER JOIN USERS u1 ON c.SenderID = u1.UserID) 
                                 INNER JOIN USERS u2 ON c.ReceiverID = u2.UserID 
                               WHERE 
                                 (c.SenderID = ? AND c.ReceiverID = ?) 
                                 OR (c.SenderID = ? AND c.ReceiverID = ?) 
                               ORDER BY 
                                 c.Timestamp ASC";

                OleDbParameter[] parameters = {
                    new OleDbParameter("@User1", userId1),
                    new OleDbParameter("@User2", userId2),
                    new OleDbParameter("@User2_2", userId2),
                    new OleDbParameter("@User1_2", userId1)
                };

                var result = DatabaseConnection.ExecuteDataTable(query, parameters);
                Debug.WriteLine($"Found {result?.Rows.Count ?? 0} messages");
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetMessagesBetweenUsers Error: {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return null;
            }
        }

        // Okunmamış mesajları işaretle
        public static bool MarkMessagesAsRead(int receiverId, int senderId)
        {
            try
            {
                string query = "UPDATE CHAT SET IsRead = ? WHERE SenderID = ? AND ReceiverID = ? AND IsRead = ?";
                OleDbParameter[] parameters = {
                    new OleDbParameter("@IsRead", true),
                    new OleDbParameter("@SenderID", senderId),
                    new OleDbParameter("@ReceiverID", receiverId),
                    new OleDbParameter("@IsReadFalse", false)
                };

                int result = DatabaseConnection.ExecuteNonQuery(query, parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"MarkMessagesAsRead Error: {ex.Message}");
                return false;
            }
        }

        // Kullanıcının okunmamış mesaj sayısını getir
        public static int GetUnreadMessageCount(int userId)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM CHAT WHERE ReceiverID = @UserId AND IsRead = False";
                    using (var command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        return Convert.ToInt32(command.ExecuteScalar());
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetUnreadMessageCount Error: {ex.Message}");
                return 0;
            }
        }

        // Belirli bir metin içeren mesajları ara
        public static DataTable SearchMessages(int userId1, int userId2, string searchText)
        {
            string query = @"SELECT c.*, u1.Username AS SenderName, u2.Username AS ReceiverName 
                           FROM CHAT c 
                           INNER JOIN USERS u1 ON c.SenderID = u1.UserID 
                           INNER JOIN USERS u2 ON c.ReceiverID = u2.UserID 
                           WHERE ((c.SenderID = ? AND c.ReceiverID = ?) OR (c.SenderID = ? AND c.ReceiverID = ?))
                           AND c.MessageText LIKE ?
                           ORDER BY c.Timestamp ASC";

            OleDbParameter[] parameters = {
                new OleDbParameter("@User1", userId1),
                new OleDbParameter("@User2", userId2),
                new OleDbParameter("@User2_2", userId2),
                new OleDbParameter("@User1_2", userId1),
                new OleDbParameter("@SearchText", "%" + searchText + "%")
            };

            return DatabaseConnection.ExecuteDataTable(query, parameters);
        }

        // İki kullanıcı arasındaki mesaj geçmişini getir
        public static DataTable GetMessageHistory(int userId1, int userId2, int pageSize = 50, int pageNumber = 1)
        {
            try
            {
                using (var connection = DatabaseConnection.GetConnection())
                {
                    connection.Open();
                    string query = @"
                        SELECT TOP @PageSize * FROM (
                            SELECT TOP (@PageSize * @PageNumber) 
                                c.*, 
                                s.FullName as SenderName,
                                r.FullName as ReceiverName
                            FROM CHAT c
                            LEFT JOIN USERS s ON c.SenderID = s.UserID
                            LEFT JOIN USERS r ON c.ReceiverID = r.UserID
                            WHERE (c.SenderID = @UserId1 AND c.ReceiverID = @UserId2)
                               OR (c.SenderID = @UserId2 AND c.ReceiverID = @UserId1)
                            ORDER BY c.Timestamp DESC
                        ) AS SubQuery
                        ORDER BY Timestamp ASC";

                    using (var command = new OleDbCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PageSize", pageSize);
                        command.Parameters.AddWithValue("@PageNumber", pageNumber);
                        command.Parameters.AddWithValue("@UserId1", userId1);
                        command.Parameters.AddWithValue("@UserId2", userId2);

                        var dataTable = new DataTable();
                        var adapter = new OleDbDataAdapter(command);
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetMessageHistory Error: {ex.Message}");
                return null;
            }
        }
    }
}