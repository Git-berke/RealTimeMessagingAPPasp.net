using System;
using System.Web.UI;
using System.Diagnostics;
using System.Data;

namespace DoctorPatientChat
{
    public partial class Dashboard : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Kullanıcı giriş yapmamışsa Login sayfasına yönlendir
                if (Session["UserID"] == null)
                {
                    Response.Redirect("~/Login.aspx");
                    return;
                }

                LoadReceivers();
            }
        }

        private void LoadReceivers()
        {
            try
            {
                // Kullanıcının rolüne göre alıcıları yükle
                string currentUserRole = Session["Role"].ToString();
                string targetRole = currentUserRole == "Doctor" ? "Patient" : "Doctor";
                
                // Alıcıları veritabanından yükle
                var users = UserManager.GetUsersByRole(targetRole);
                
                if (users != null && users.Rows.Count > 0)
                {
                    ddlReceiver.DataSource = users;
                    ddlReceiver.DataTextField = "FullName";
                    ddlReceiver.DataValueField = "UserID";
                    ddlReceiver.DataBind();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"LoadReceivers Error: {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        public string BackgroundClass
        {
            get
            {
                var role = Session["Role"]?.ToString();
                if (role == "Doctor") return "doctor-bg";
                if (role == "Patient") return "patient-bg";
                return string.Empty;
            }
        }

        public string WelcomeMessage
        {
            get
            {
                var role = Session["Role"]?.ToString();
                var name = Session["FullName"]?.ToString();
                if (string.IsNullOrEmpty(name)) name = "Kullanıcı";
                if (role == "Doctor") return $"Hoşgeldiniz, Doktor {name}";
                return $"Hoşgeldiniz, {name}";
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            // Bu metot artık kullanılmıyor, mesajlar SignalR üzerinden gönderiliyor
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Session'ı temizle
            Session.Clear();
            Session.Abandon();

            // Login sayfasına yönlendir
            Response.Redirect("~/Login.aspx");
        }
    }
}