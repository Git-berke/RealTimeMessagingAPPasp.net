using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace DoctorPatientChat
{
	public partial class Profile : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			if (!IsPostBack)
			{
				if (Session["UserID"] == null)
				{
					Response.Redirect("~/Login.aspx");
					return;
				}

				int userId = Convert.ToInt32(Session["UserID"]);
				var user = UserManager.GetUserById(userId);
				if (user != null)
				{
					txtFullName.Text = user["FullName"].ToString();
					txtEmail.Text = user["Email"].ToString();
					Session["UserRole"] = user["Role"].ToString();
				}
			}
			this.DataBind(); // DataBinding'i etkinleştir
		}

		public string GetProfileCardBg()
		{
			string role = Session["UserRole"] as string;
			if (role == "Doctor")
				return "linear-gradient(135deg, #e3f0ff 0%, #f7fbff 100%)";
			if (role == "Patient")
				return "linear-gradient(135deg, #e6fff3 0%, #f7fbff 100%)";
			return "#f7fbff";
		}

		protected void btnSave_Click(object sender, EventArgs e)
		{
			if (Session["UserID"] == null)
			{
				Response.Redirect("~/Login.aspx");
				return;
			}

			int userId = Convert.ToInt32(Session["UserID"]);
			string fullName = txtFullName.Text.Trim();
			string email = txtEmail.Text.Trim();

			if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(email))
			{
				lblMessage.Text = "Ad Soyad ve E-posta boş bırakılamaz.";
				return;
			}

			bool updated = UserManager.UpdateUserProfile(userId, fullName, email);
			if (updated)
			{
				lblMessage.CssClass = "text-success mb-2";
				lblMessage.Text = "Profil başarıyla güncellendi.";
				Session["FullName"] = fullName;
			}
			else
			{
				lblMessage.CssClass = "text-danger mb-2";
				lblMessage.Text = "Profil güncellenirken bir hata oluştu.";
			}
		}
	}
}