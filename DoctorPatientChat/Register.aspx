<%@ Page Title="Kayıt Ol" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="DoctorPatientChat.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row animated">
        <div class="col-md-8 col-md-offset-2">
            <div class="panel panel-primary">
                <div class="panel-heading">
                    <h3 class="panel-title"><i class="glyphicon glyphicon-user"></i> Yeni Kullanıcı Kaydı</h3>
                </div>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-4 hidden-xs">
                            <div class="text-center" style="padding: 40px 0;">
                                <i class="glyphicon glyphicon-user" style="font-size: 100px; color: #3498db;"></i>
                                <h4 style="margin-top: 20px;">Sağlık İletişiminde Yeni Dönem</h4>
                                <p class="text-muted">Hızlı, güvenli ve kolay iletişim için hemen kayıt olun.</p>
                            </div>
                        </div>
                        <div class="col-md-8">
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="col-md-4 control-label">Kullanıcı Adı:</label>
                                    <div class="col-md-8">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="glyphicon glyphicon-user"></i></span>
                                            <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Kullanıcı adınızı girin" required></asp:TextBox>
                                        </div>
                                        <asp:RegularExpressionValidator ID="revUsername" runat="server" 
                                            ControlToValidate="txtUsername" ValidationExpression="^[a-zA-Z0-9_]{3,20}$"
                                            ErrorMessage="Kullanıcı adı en az 3, en fazla 20 karakter olmalı ve sadece harfler, rakamlar ve alt çizgi içermelidir."
                                            Display="Dynamic" CssClass="text-danger small">
                                        </asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                
                                <div class="form-group">
                                    <label class="col-md-4 control-label">Şifre:</label>
                                    <div class="col-md-8">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="glyphicon glyphicon-lock"></i></span>
                                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Şifrenizi girin" required></asp:TextBox>
                                        </div>
                                        <asp:RegularExpressionValidator ID="revPassword" runat="server" 
                                            ControlToValidate="txtPassword" ValidationExpression="^.{6,}$"
                                            ErrorMessage="Şifre en az 6 karakter olmalıdır."
                                            Display="Dynamic" CssClass="text-danger small">
                                        </asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                
                                <div class="form-group">
                                    <label class="col-md-4 control-label">Şifre Tekrar:</label>
                                    <div class="col-md-8">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="glyphicon glyphicon-lock"></i></span>
                                            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Şifrenizi tekrar girin" required></asp:TextBox>
                                        </div>
                                        <asp:CompareValidator ID="cvPassword" runat="server" 
                                            ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword"
                                            ErrorMessage="Şifreler eşleşmiyor."
                                            Display="Dynamic" CssClass="text-danger small">
                                        </asp:CompareValidator>
                                    </div>
                                </div>
                                
                                <div class="form-group">
                                    <label class="col-md-4 control-label">Ad Soyad:</label>
                                    <div class="col-md-8">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="glyphicon glyphicon-font"></i></span>
                                            <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" placeholder="Adınız ve soyadınızı girin" required></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                
                                <div class="form-group">
                                    <label class="col-md-4 control-label">E-posta:</label>
                                    <div class="col-md-8">
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="glyphicon glyphicon-envelope"></i></span>
                                            <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" CssClass="form-control" placeholder="E-posta adresinizi girin" required></asp:TextBox>
                                        </div>
                                        <asp:RegularExpressionValidator ID="revEmail" runat="server" 
                                            ControlToValidate="txtEmail" ValidationExpression="^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$"
                                            ErrorMessage="Geçerli bir e-posta adresi girin."
                                            Display="Dynamic" CssClass="text-danger small">
                                        </asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                
                                <div class="form-group">
                                    <label class="col-md-4 control-label">Kullanıcı Tipi:</label>
                                    <div class="col-md-8">
                                        <asp:RadioButtonList ID="rblUserType" runat="server" RepeatDirection="Horizontal" CssClass="radio-inline">
                                            <asp:ListItem Value="Doctor">Doktor</asp:ListItem>
                                            <asp:ListItem Value="Patient" Selected="True">Hasta</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                </div>
                                
                                <div class="form-group">
                                    <div class="col-md-offset-4 col-md-8">
                                        <asp:Label ID="lblMessage" runat="server" CssClass="text-danger"></asp:Label>
                                    </div>
                                </div>
                                
                                <div class="form-group">
                                    <div class="col-md-offset-4 col-md-8">
                                        <asp:Button ID="btnRegister" runat="server" Text="Kayıt Ol" CssClass="btn btn-primary" OnClick="btnRegister_Click" />
                                        <a href="Login.aspx" class="btn btn-default">Giriş Sayfasına Dön</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>