using System;
using System.Data;
using System.Web.UI;

namespace DoctorPatientChat
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Kullanıcı zaten giriş yapmışsa Dashboard'a yönlendir
            if (Session["UserID"] != null)
            {
                Response.Redirect("~/Dashboard.aspx");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            // Kullanıcı adı ve şifre kontrolü
            if (UserManager.ValidateUser(username, password))
            {
                // Kullanıcı bilgilerini getir
                DataRow user = UserManager.GetUserByUsername(username);

                if (user != null)
                {
                    // Kullanıcı bilgilerini session'a kaydet
                    Session["UserID"] = user["UserID"].ToString();
                    Session["Username"] = user["Username"].ToString();
                    Session["Role"] = user["Role"].ToString();
                    Session["FullName"] = user["FullName"].ToString();

                    // Dashboard'a yönlendir
                    Response.Redirect("~/Dashboard.aspx");
                }
                else
                {
                    lblMessage.Text = "Kullanıcı bilgileri alınamadı. Lütfen tekrar deneyin.";
                }
            }
            else
            {
                lblMessage.Text = "Kullanıcı adı veya şifre hatalı!";
            }
        }
    }
}