<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="DoctorPatientChat.Profile" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Profil Bilgileri</title>
    <style>
        body {
            background: #f4f7fa;
        }
        .profile-card {
            background: <%# GetProfileCardBg() %>;
            border-radius: 18px;
            box-shadow: 0 4px 24px rgba(0,0,0,0.08);
            padding: 32px 28px 24px 28px;
            margin-top: 48px;
            border: none;
        }
        .profile-card .card-header {
            background: transparent;
            color: #234;
            border-bottom: none;
            margin-bottom: 12px;
        }
        .profile-label {
            display: block;
            margin-bottom: 6px;
            font-weight: 600;
            color: #234;
            font-size: 1.05rem;
        }
        .profile-input {
            border-radius: 10px !important;
            padding: 12px 14px !important;
            border: 1.5px solid #cfd8dc !important;
            font-size: 1rem;
            background: #f9fbfd;
            margin-bottom: 18px;
            transition: border-color 0.2s;
        }
        .profile-input:focus {
            border-color: #4f8edc;
            background: #fff;
            outline: none;
        }
        .profile-btn {
            background: linear-gradient(90deg,#4f8edc,#43e97b);
            color: #fff;
            border: none;
            border-radius: 8px;
            padding: 12px 0;
            font-size: 1.1rem;
            font-weight: 600;
            box-shadow: 0 2px 8px rgba(79,142,220,0.08);
            transition: background 0.2s, box-shadow 0.2s;
        }
        .profile-btn:hover {
            background: linear-gradient(90deg,#43e97b,#4f8edc);
            box-shadow: 0 4px 16px rgba(67,233,123,0.12);
            color: #fff;
        }
        .profile-message {
            margin-bottom: 16px;
            font-size: 1rem;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container" style="max-width: 480px;">
            <div class="profile-card">
                <div class="card-header text-center">
                    <h3 class="mb-0">Profil Bilgileri</h3>
                </div>
                <div class="card-body">
                    <asp:Label ID="lblMessage" runat="server" CssClass="profile-message text-danger" />
                    <div class="mb-3">
                        <label for="txtFullName" class="profile-label">Ad Soyad</label>
                        <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control profile-input" />
                    </div>
                    <div class="mb-3">
                        <label for="txtEmail" class="profile-label">E-posta</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control profile-input" TextMode="Email" />
                    </div>
                    <asp:Button ID="btnSave" runat="server" Text="Kaydet" CssClass="profile-btn w-100 mt-2" OnClick="btnSave_Click" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
