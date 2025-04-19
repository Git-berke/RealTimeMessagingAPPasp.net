using System;
using System.Web.UI;

namespace DoctorPatientChat
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Kullanıcı zaten giriş yapmışsa Dashboard'a yönlendir
            if (Session["UserID"] != null)
            {
                Response.Redirect("~/Dashboard.aspx");
            }
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string role = rblUserType.SelectedValue; // "Doctor" veya "Patient"

            // Kullanıcı kaydı oluştur
            if (UserManager.RegisterUser(username, password, role, fullName, email))
            {
                // Başarılı kayıt mesajı
                Response.Redirect("~/Login.aspx?registered=true");
            }
            else
            {
                lblMessage.Text = "Kayıt işlemi sırasında bir hata oluştu. Kullanıcı adı zaten kullanılıyor olabilir.";
            }
        }
    }
}