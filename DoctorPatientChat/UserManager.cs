using System;
using System.Data;
using System.Data.OleDb;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using System.Diagnostics;

namespace DoctorPatientChat
{
    public class UserManager
    {
        // Kullanıcı girişi kontrolü
        public static bool ValidateUser(string username, string password)
        {
            string hashedPassword = HashPassword(password);

            string query = "SELECT COUNT(*) FROM USERS WHERE Username = ? AND Password = ?";
            OleDbParameter[] parameters = {
                new OleDbParameter("@Username", username),
                new OleDbParameter("@Password", hashedPassword)
            };

            int count = Convert.ToInt32(DatabaseConnection.ExecuteScalar(query, parameters));
            return count > 0;
        }

        // Kullanıcı bilgilerini getir
        public static DataRow GetUserByUsername(string username)
        {
            string query = "SELECT * FROM USERS WHERE Username = ?";
            OleDbParameter parameter = new OleDbParameter("@Username", username);

            DataTable dt = DatabaseConnection.ExecuteDataTable(query, parameter);
            if (dt != null && dt.Rows.Count > 0)
                return dt.Rows[0];
            return null;
        }

        // Yeni kullanıcı kaydı
        public static bool RegisterUser(string username, string password, string role, string fullName, string email)
        {
            try
            {
                Debug.WriteLine($"Kayıt işlemi başladı - Kullanıcı: {username}, Rol: {role}");

                // Veritabanı bağlantısını test et
                using (OleDbConnection testConn = DatabaseConnection.GetConnection())
                {
                    try
                    {
                        testConn.Open();
                        Debug.WriteLine("Veritabanı bağlantısı başarılı");
                        testConn.Close();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Veritabanı bağlantı hatası: {ex.Message}");
                        Debug.WriteLine($"Connection string: {testConn.ConnectionString}");
                        return false;
                    }
                }

                // Kullanıcı adı kontrolü
                string checkQuery = "SELECT COUNT(*) FROM [USERS] WHERE Username = ?";
                OleDbParameter checkParam = new OleDbParameter(null, username);

                try
                {
                    int userCount = Convert.ToInt32(DatabaseConnection.ExecuteScalar(checkQuery, checkParam));
                    Debug.WriteLine($"Mevcut kullanıcı sayısı: {userCount}");

                    if (userCount > 0)
                    {
                        Debug.WriteLine("Kullanıcı adı zaten kullanımda");
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Kullanıcı kontrolü sırasında hata: {ex.Message}");
                    if (ex.InnerException != null)
                        Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    return false;
                }

                string hashedPassword = HashPassword(password);
                Debug.WriteLine("Şifre hash'lendi");

                string query = "INSERT INTO [USERS] (Username, [Password], [Role], FullName, Email) VALUES (?, ?, ?, ?, ?)";
                OleDbParameter[] parameters = {
                    new OleDbParameter(null, username),
                    new OleDbParameter(null, hashedPassword),
                    new OleDbParameter(null, role),
                    new OleDbParameter(null, fullName),
                    new OleDbParameter(null, email)
                };

                try
                {
                    int result = DatabaseConnection.ExecuteNonQuery(query, parameters);
                    Debug.WriteLine($"Kayıt işlemi sonucu: {result} satır etkilendi");
                    return result > 0;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Kayıt işlemi sırasında hata: {ex.Message}");
                    if (ex.InnerException != null)
                        Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Genel hata: {ex.Message}");
                if (ex.InnerException != null)
                    Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                return false;
            }
        }

        // Şifre hashleme
        private static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }

        // Kullanıcıları role göre getir (Doktorlar veya Hastalar)
        public static DataTable GetUsersByRole(string role)
        {
            string query = "SELECT UserID, Username, FullName, Email FROM USERS WHERE Role = ?";
            OleDbParameter parameter = new OleDbParameter("@Role", role);

            return DatabaseConnection.ExecuteDataTable(query, parameter);
        }

        // Kullanıcı ID'sine göre kullanıcı bilgisi getir
        public static DataRow GetUserById(int userId)
        {
            string query = "SELECT * FROM USERS WHERE UserID = ?";
            OleDbParameter parameter = new OleDbParameter("@UserID", userId);

            DataTable dt = DatabaseConnection.ExecuteDataTable(query, parameter);
            if (dt != null && dt.Rows.Count > 0)
                return dt.Rows[0];
            return null;
        }

        // Kullanıcı profilini güncelle
        public static bool UpdateUserProfile(int userId, string fullName, string email)
        {
            string query = "UPDATE USERS SET FullName = ?, Email = ? WHERE UserID = ?";
            OleDbParameter[] parameters = new OleDbParameter[]
            {
                new OleDbParameter("@FullName", fullName),
                new OleDbParameter("@Email", email),
                new OleDbParameter("@UserID", userId)
            };
            int result = DatabaseConnection.ExecuteNonQuery(query, parameters);
            return result > 0;
        }
    }
}