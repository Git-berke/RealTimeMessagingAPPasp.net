<%@ Page Title="Giriş" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="DoctorPatientChat.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row animated">
        <div class="col-md-6 col-md-offset-3">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title"><i class="glyphicon glyphicon-log-in"></i> Kullanıcı Girişi</h3>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-5 text-center hidden-xs">
                            <div style="padding: 40px 0;">
                                <img src="Images/doctor-icon.png" alt="Doktor-Hasta İletişimi" onerror="this.src='data:image/svg+xml;charset=UTF-8,%3Csvg%20width%3D%22200%22%20height%3D%22200%22%20xmlns%3D%22http%3A%2F%2Fwww.w3.org%2F2000%2Fsvg%22%3E%3Ccircle%20cx%3D%22100%22%20cy%3D%2270%22%20r%3D%2260%22%20fill%3D%22%233498db%22%2F%3E%3Ccircle%20cx%3D%22100%22%20cy%3D%2250%22%20r%3D%2225%22%20fill%3D%22white%22%2F%3E%3Crect%20x%3D%2260%22%20y%3D%22120%22%20width%3D%2280%22%20height%3D%2260%22%20fill%3D%22%233498db%22%2F%3E%3C%2Fsvg%3E';" style="max-width: 120px;">
                                <h4 style="margin-top: 20px; color: #3498db;">Hoş Geldiniz</h4>
                                <p class="text-muted">Doktor ve hastalar için güvenli mesajlaşma platformu</p>
                                
                                <% if (Request.QueryString["registered"] == "true") { %>
                                <div class="alert alert-success">
                                    <i class="glyphicon glyphicon-ok"></i> Kayıt işleminiz başarıyla tamamlandı. Giriş yapabilirsiniz.
                                </div>
                                <% } %>
                                
                                <% if (Request.QueryString["logout"] == "true") { %>
                                <div class="alert alert-info">
                                    <i class="glyphicon glyphicon-info-sign"></i> Oturumunuz güvenli bir şekilde sonlandırıldı.
                                </div>
                                <% } %>
                            </div>
                        </div>
                        <div class="col-md-7">
                            <div style="padding: 20px 10px;">
                                <div class="form-group">
                                    <label>Kullanıcı Adı</label>
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
                                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Kullanıcı adınız" required></asp:TextBox>
                                    </div>
                                </div>
                                
                                <div class="form-group">
                                    <label>Şifre</label>
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="glyphicon glyphicon-lock"></i></span>
                                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Şifreniz" required></asp:TextBox>
                                    </div>
                                </div>
                                
                                <div class="form-group">
                                    <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"></asp:Label>
                                </div>
                                
                                <div class="form-group">
                                    <asp:Button ID="btnLogin" runat="server" Text="Giriş Yap" CssClass="btn btn-primary btn-block" OnClick="btnLogin_Click" />
                                </div>
                                
                                <div class="text-center">
                                    <p>Hesabınız yok mu? <a href="Register.aspx" class="text-primary">Kayıt Ol</a></p>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>