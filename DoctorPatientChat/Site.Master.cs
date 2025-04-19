using System;
using System.Web.UI;

namespace DoctorPatientChat
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Kullanıcı giriş yapmış mı kontrol et
            if (Session["UserID"] != null)
            {
                pnlLoggedIn.Visible = true;
                pnlNotLoggedIn.Visible = false;
            }
            else
            {
                pnlLoggedIn.Visible = false;
                pnlNotLoggedIn.Visible = true;
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            // Kullanıcı oturumunu sonlandır
            Session.Clear();
            Session.Abandon();
            Response.Redirect("~/Login.aspx");
        }
    }
}