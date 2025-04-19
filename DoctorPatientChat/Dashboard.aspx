<%@ Page Title="Ana Sayfa" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="DoctorPatientChat.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
        .chat-container {
            height: 500px;
            overflow-y: auto;
            border: 1px solid #ddd;
            padding: 10px;
            margin-bottom: 20px;
            background-color: #efeae2;
            background-image: url("data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADIAAAAyBAMAAADsEZWCAAAAG1BMVEVHcEzf39/h4eHh4eHh4eHh4eHh4eHh4eHi4uLh4eHh4eHh4eFpQJYbAAAACXBIWXMAAAsTAAALEwEAmpwYAAAAPElEQVQ4jWNgQAX8/AwMAgwMDIFAEMoKhLKBIIwBwmdgYGBkQhV0YEAVZGVkQhOEqyFrQLIaQnqo4YYAWbYPIQyzzEMAAAAASUVORK5CYII=");
            display: flex;
            flex-direction: column;
        }

        .message {
            display: flex;
            width: 100%;
            margin: 4px 0;
            position: relative;
        }

        .sent-message {
            justify-content: flex-end;
        }

        .received-message {
            justify-content: flex-start;
        }

        .message-bubble {
            max-width: 70%;
            padding: 6px 7px 8px 9px;
            border-radius: 7.5px;
            position: relative;
            box-shadow: 0 1px 0.5px rgba(0,0,0,0.13);
        }

        .sent-message .message-bubble {
            background-color: #e7ffdb;
            border-radius: 7.5px 0 7.5px 7.5px;
            margin-right: 10px;
        }

        .received-message .message-bubble {
            background-color: #ffffff;
            border-radius: 0 7.5px 7.5px 7.5px;
            margin-left: 10px;
        }

        .message-text {
            font-size: 14.2px;
            line-height: 19px;
            color: #111b21;
            margin-bottom: 20px; /* Daha fazla boşluk bırakıyorum (15px'ten 20px'e çıkardım) */
            word-wrap: break-word; /* Uzun metinlerin taşmaması için */
        }

        .message-meta {
            display: flex;
            justify-content: flex-end;
            align-items: center;
            position: absolute;
            bottom: 2px; /* Daha aşağıya indirmek için (4px'ten 2px'e indirdim) */
            right: 7px;
            background-color: inherit; /* Mesaj baloncuğunun rengini alacak */
            padding: 2px 0; /* Ek padding */
            border-radius: 4px; /* Yuvarlatılmış köşeler */
        }

        .message-time {
            font-size: 11px;
            line-height: 15px;
            color: #667781;
            margin-left: 3px;
            padding-right: 2px; /* Sağ taraftan boşluk */
        }

        .message-sender {
            font-size: 12.8px;
            line-height: 21px;
            color: #1fa855;
            font-weight: 500;
            margin-bottom: 2px;
        }

        .form-group {
            background-color: #f0f2f5;
            padding: 10px;
            margin-top: 10px;
        }

        .input-group .form-control {
            border-radius: 8px;
            padding: 9px 12px;
            border: 1px solid #ccd6dd;
            resize: none;
            height: auto;
        }

        .input-group .btn-primary {
            border-radius: 50%;
            width: 40px;
            height: 40px;
            padding: 0;
            display: flex;
            align-items: center;
            justify-content: center;
            margin-left: 8px;
        }

        .no-messages {
            text-align: center;
            color: #8696a0;
            padding: 20px;
            font-style: italic;
            width: 100%;
        }

        .message-container {
            display: flex;
            flex-direction: column;
            width: 100%;
        }

        .message-row {
            width: 100%;
            display: flex;
            margin: 2px 0;
        }

        .unread-badge {
            background-color: #25d366;
            color: white;
            border-radius: 50%;
            padding: 2px 6px;
            font-size: 12px;
            margin-left: 5px;
        }

        .logout-button {
            position: absolute;
            top: 10px;
            right: 10px;
            display: flex;
            align-items: center;
            gap: 12px;
        }
        
        /* Message Read Status Styles */
        .sent-tick, .read-tick {
            font-size: 11px;
            margin-left: 4px;
        }
        
        .sent-tick {
            color: #92a58c;
        }
        
        .read-tick {
            color: #4fc3f7;
        }
        
        .message.sent-message.read .message-time {
            color: #4fc3f7;
        }

        /* Search Highlight Styles */
        .search-highlight {
            background-color: #fff3cd !important;
            border: 1.5px solid #ffe066 !important;
        }

        .doctor-bg {
            background: linear-gradient(135deg, #1e3a5c 0%, #4f8edc 100%);
        }
        .patient-bg {
            background: linear-gradient(135deg, #43e97b 0%, #38f9d7 100%);
        }

        body.dark-mode, .container.dark-mode {
            background: #181c23 !important;
            color: #f1f1f1 !important;
        }
        .container.dark-mode .message-bubble {
            background: #232a34 !important;
            color: #f1f1f1 !important;
        }
        .container.dark-mode .form-group, .container.dark-mode .form-control {
            background: #232a34 !important;
            color: #f1f1f1 !important;
        }
        .container.dark-mode .btn, .container.dark-mode .btn-primary, .container.dark-mode .btn-danger {
            background: #2d3748 !important;
            color: #f1f1f1 !important;
            border-color: #444 !important;
        }
        .container.dark-mode .btn-link {
            color: #b2f5ea !important;
        }
        .container.dark-mode hr {
            border-color: #333 !important;
        }
        .container.dark-mode .message-sender {
            color: #7ee787 !important;
        }
        .container.dark-mode .message-time {
            color: #b2b2b2 !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="logout-button">
        <asp:Button ID="btnLogout" runat="server" Text="Çıkış Yap" CssClass="btn btn-danger" OnClick="btnLogout_Click" />
        <div>
            <label class="form-switch" style="cursor:pointer; user-select:none;">
                <input type="checkbox" id="themeToggle" style="display:none;">
                <span id="themeLabel" style="padding:6px 14px; border-radius:20px; background:#eee; color:#222; font-size:14px; border:1px solid #bbb;">🌙 Karanlık</span>
            </label>
        </div>
    </div>

    <!-- Alıcı seçme kısmı üstte -->
    <div class="form-group" style="margin-top: 60px; margin-bottom: 18px;">
        <label for="<%=ddlReceiver.ClientID%>" style="font-size:17px; font-weight:600; color:#234;">Mesaj göndermek istediğiniz kişiyi seçin</label>
        <asp:DropDownList ID="ddlReceiver" runat="server" CssClass="form-control" style="max-width:350px;"></asp:DropDownList>
    </div>

    <div class="container <%= BackgroundClass %>">
        <div class="row">
            <div class="col-md-12 d-flex align-items-center justify-content-between">
                <h2 style="margin-bottom:0;">Mesajlaşma</h2>
                <div class="d-flex flex-column align-items-center" style="min-width:120px;">
                    <a href="Profile.aspx" title="Profilim" style="display: flex; align-items: center; justify-content: center; width: 56px; height: 56px; background: linear-gradient(135deg,#4f8edc,#43e97b); border-radius: 50%; box-shadow: 0 2px 8px rgba(0,0,0,0.08); margin-bottom: 6px;">
                        <svg xmlns="http://www.w3.org/2000/svg" width="32" height="32" fill="#fff" viewBox="0 0 16 16">
                            <circle cx="8" cy="8" r="8" fill="rgba(255,255,255,0.08)"/>
                            <path d="M11 10a2 2 0 1 1-4 0 2 2 0 0 1 4 0z"/>
                            <path fill-rule="evenodd" d="M0 8a8 8 0 1 1 16 0A8 8 0 0 1 0 8zm8-7a7 7 0 1 0 0 14A7 7 0 0 0 8 1zM4.285 12.433A5.978 5.978 0 0 0 8 13c1.306 0 2.518-.418 3.515-1.118C11.356 11.226 9.805 10.5 8 10.5c-1.805 0-3.356.726-3.715 1.933z" fill="#fff"/>
                        </svg>
                    </a>
                    <a href="Profile.aspx" class="btn btn-outline-primary btn-sm" style="width:120px;">Profili Görüntüle</a>
                </div>
            </div>
        </div>
        <div style="font-size:18px; font-weight:500; margin-bottom:10px; color:#234;">
            <%= WelcomeMessage %>
        </div>
        <hr />
        <div class="form-inline mb-2">
            <input type="text" id="searchMessageInput" class="form-control mr-2" placeholder="Mesajlarda ara..." style="width: 250px; display: inline-block;" />
            <button type="button" id="searchMessageBtn" class="btn btn-secondary">Ara</button>
            <button type="button" id="clearSearchBtn" class="btn btn-link">Temizle</button>
        </div>
        <div class="row">
            <div class="col-md-8">
                <div class="chat-container" id="messageContainer">
                    <div class="no-messages">
                        Mesajlaşmak için bir kişi seçin.
                    </div>
                </div>
                <div class="form-group">
                    <div class="input-group"></div>
                        <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control" placeholder="Mesajınızı yazın..." />
                        <div class="input-group-append">
                            <asp:Button ID="btnSend" runat="server" Text="Gönder" CssClass="btn btn-primary" OnClientClick="return sendMessage();" />
                        </div>
                    </div>
                    <div id="typingIndicator" class="typing-indicator" style="display: none;"></div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script src="/Scripts/jquery-3.6.0.min.js"></script>
    <script src="/Scripts/jquery.signalR-2.4.3.min.js"></script>
    <script src="/signalr/hubs"></script>
    
    <script type="text/javascript">
        $(function () {
            var chat = $.connection.chatHub;
            var currentReceiverId = null;
            var isFirstLoad = true;
            var isConnected = false;

            // Mesaj alma fonksiyonu
            chat.client.receiveMessage = function (senderId, message, timestamp) {
                console.log('Received message:', { senderId, message, timestamp });
                addMessage(senderId, message, timestamp, false);
                scrollToBottom();
                
                // Mesajı aldığımızda, karşı tarafa okundu bilgisi gönder
                if (currentReceiverId) {
                    chat.server.markMessagesAsRead(currentReceiverId);
                }
            };

            // Mesaj gönderme onayı
            chat.client.messageSent = function (receiverId, message, timestamp) {
                console.log('Message sent:', { receiverId, message, timestamp });
                addMessage('Siz', message, timestamp, true, false);
                scrollToBottom();
            };

            // Mesaj okunduğunda
            chat.client.messagesRead = function (receiverId) {
                console.log('Messages read by:', receiverId);
                $('.message.sent-message:not(.read) .message-meta').each(function() {
                    $(this).append('<span class="read-tick">✓✓</span>');
                    $(this).closest('.message').addClass('read');
                });
            };

            // Okunmamış mesaj sayısını güncelle
            chat.client.updateUnreadCount = function (count) {
                console.log('Unread message count:', count);
                var badge = $('#unreadBadge');
                if (count > 0) {
                    if (badge.length === 0) {
                        $('h2').append('<span id="unreadBadge" class="unread-badge">' + count + '</span>');
                    } else {
                        badge.text(count);
                    }
                } else {
                    badge.remove();
                }
            };

            // Mesaj geçmişi ekleme
            chat.client.addMessageToHistory = function (senderName, message, timestamp, isSent, isRead) {
                console.log('Adding message to history:', { senderName, message, timestamp, isSent, isRead });
                if (isFirstLoad) {
                    $('#messageContainer').empty();
                    isFirstLoad = false;
                }
                addMessage(senderName, message, timestamp, isSent, isRead !== false);
                scrollToBottom();
                
                // Mesaj geçmişi yüklendiğinde mesajları okundu olarak işaretle
                if (currentReceiverId && !isSent) {
                    chat.server.markMessagesAsRead(currentReceiverId);
                }
            };

            // Yazıyor sinyali
            chat.client.userTyping = function (senderId) {
                showTypingIndicator(senderId);
            };

            // Yazmayı bırakma sinyali
            chat.client.userStoppedTyping = function (senderId) {
                hideTypingIndicator();
            };

            // Hata gösterimi
            chat.client.showError = function (message) {
                console.error('Error:', message);
                alert(message);
            };

            // Bağlantıyı başlat
            function startConnection() {
                if (!isConnected) {
                    $.connection.hub.qs = { 'userId': '<%= Session["UserID"] %>' };
                    $.connection.hub.start()
                        .done(function () {
                            console.log('SignalR bağlantısı başarılı');
                            isConnected = true;

                            // Eğer önceden seçili bir alıcı varsa mesajları yükle
                            var selectedReceiverId = $('#<%=ddlReceiver.ClientID%>').val();
                            if (selectedReceiverId) {
                                loadMessages(selectedReceiverId);
                            }
                        })
                        .fail(function (error) {
                            console.error('SignalR bağlantı hatası:', error);
                            setTimeout(startConnection, 5000); // 5 saniye sonra tekrar dene
                        });
                }
            }

            // Bağlantıyı başlat
            startConnection();

            // Bağlantı kesilirse tekrar dene
            $.connection.hub.disconnected(function () {
                console.log('SignalR bağlantısı kesildi');
                isConnected = false;
                setTimeout(startConnection, 5000);
            });

            // Alıcı seçildiğinde mesajları yükle
            $('#<%=ddlReceiver.ClientID%>').change(function () {
                var receiverId = $(this).val();
                if (receiverId) {
                    loadMessages(receiverId);
                }
            });

            function loadMessages(receiverId) {
                if (!isConnected) {
                    console.log('SignalR bağlantısı yok, mesajlar yüklenemiyor');
                    return;
                }

                console.log('Loading messages for receiver:', receiverId);
                currentReceiverId = receiverId;
                isFirstLoad = true;
                $('#messageContainer').html('<div class="text-center"><i>Mesajlar yükleniyor...</i></div>');
                chat.server.loadMessageHistory(receiverId);
            }

            // Mesaj gönderme
            window.sendMessage = function () {
                if (!isConnected) {
                    alert('Bağlantı sorunu var, lütfen sayfayı yenileyin.');
                    return false;
                }

                var message = $('#<%=txtMessage.ClientID%>').val().trim();
                if (message && currentReceiverId) {
                    console.log('Sending message:', { currentReceiverId, message });
                    chat.server.sendMessage(currentReceiverId, message);
                    $('#<%=txtMessage.ClientID%>').val('');
                }
                return false;
            };

            // Enter tuşu ile mesaj gönderme
            $('#<%=txtMessage.ClientID%>').keypress(function (e) {
                if (e.which == 13 && !e.shiftKey) {
                    e.preventDefault();
                    sendMessage();
                }
            });

            // Yazıyor sinyali gönderme
            var typingTimeout;
            $('#<%=txtMessage.ClientID%>').keyup(function () {
                if (currentReceiverId && isConnected) {
                    chat.server.startTyping(currentReceiverId);
                    clearTimeout(typingTimeout);
                    typingTimeout = setTimeout(function () {
                        chat.server.stopTyping(currentReceiverId);
                    }, 1000);
                }
            });

            // Mesaj ekleme fonksiyonu
            function addMessage(sender, message, timestamp, isSent, isRead) {
                console.log('Adding message:', { sender, message, timestamp, isSent, isRead });
                var messageClass = isSent ? 'sent-message' : 'received-message';
                if (isSent && isRead) {
                    messageClass += ' read';
                }
                
                var readTick = '';
                if (isSent) {
                    readTick = isRead ? '<span class="read-tick">✓✓</span>' : '<span class="sent-tick">✓</span>';
                }
                
                var messageHtml =
                    '<div class="message ' + messageClass + '">' +
                    '<div class="message-bubble">' +
                    (isSent ? '' : '<div class="message-sender">' + sender + '</div>') +
                    '<div class="message-text">' + message + '</div>' +
                    '<div class="message-meta">' +
                    '<span class="message-time">' + timestamp + '</span>' +
                    readTick +
                    '</div>' +
                    '</div>' +
                    '</div>';

                $('#messageContainer').append(messageHtml);
                scrollToBottom();
            }

            // Yazıyor göstergesi
            function showTypingIndicator(senderId) {
                $('#typingIndicator').text(senderId + ' yazıyor...').show();
            }

            function hideTypingIndicator() {
                $('#typingIndicator').hide();
            }

            // Mesaj kutusunu en alta kaydır
            function scrollToBottom() {
                var container = $('#messageContainer');
                container.scrollTop(container[0].scrollHeight);
            }

            // Mesaj arama fonksiyonu
            $(document).on('click', '#searchMessageBtn', function () {
                var searchTerm = $('#searchMessageInput').val().toLowerCase();
                if (!searchTerm) {
                    $('.message-bubble').removeClass('search-highlight');
                    $('.message').show();
                    return;
                }
                $('.message').each(function () {
                    var text = $(this).find('.message-text').text().toLowerCase();
                    if (text.indexOf(searchTerm) !== -1) {
                        $(this).show();
                        $(this).find('.message-bubble').addClass('search-highlight');
                    } else {
                        $(this).hide();
                    }
                });
            });

            // Temizle butonu
            $(document).on('click', '#clearSearchBtn', function () {
                $('#searchMessageInput').val('');
                $('.message-bubble').removeClass('search-highlight');
                $('.message').show();
            });

            // Enter ile arama
            $(document).on('keypress', '#searchMessageInput', function (e) {
                if (e.which == 13) {
                    $('#searchMessageBtn').click();
                }
            });
        });

        // Tema toggle işlemleri
        function setTheme(isDark) {
            var container = document.querySelector('.container');
            if (isDark) {
                document.body.classList.add('dark-mode');
                if(container) container.classList.add('dark-mode');
                document.getElementById('themeToggle').checked = true;
                document.getElementById('themeLabel').innerText = '☀️ Aydınlık';
            } else {
                document.body.classList.remove('dark-mode');
                if(container) container.classList.remove('dark-mode');
                document.getElementById('themeToggle').checked = false;
                document.getElementById('themeLabel').innerText = '🌙 Karanlık';
            }
        }
        document.addEventListener('DOMContentLoaded', function() {
            var themeToggle = document.getElementById('themeToggle');
            var savedTheme = localStorage.getItem('themeMode');
            setTheme(savedTheme === 'dark');
            themeToggle.addEventListener('change', function() {
                var isDark = themeToggle.checked;
                setTheme(isDark);
                localStorage.setItem('themeMode', isDark ? 'dark' : 'light');
            });
        });
    </script>
</asp:Content>