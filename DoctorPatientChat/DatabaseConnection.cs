using System;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Web;
using System.Diagnostics;

namespace DoctorPatientChat
{
    public class DatabaseConnection
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["ChatConnectionString"].ConnectionString;

        // Bağlantı nesnesi oluşturma
        public static OleDbConnection GetConnection()
        {
            Debug.WriteLine($"Bağlantı dizesi: {connectionString}");
            return new OleDbConnection(connectionString);
        }

        // SQL komutunu çalıştırma (INSERT, UPDATE, DELETE)
        public static int ExecuteNonQuery(string commandText, params OleDbParameter[] parameters)
        {
            Debug.WriteLine($"ExecuteNonQuery çağrıldı. SQL: {commandText}");
            using (OleDbConnection connection = GetConnection())
            {
                using (OleDbCommand command = new OleDbCommand(commandText, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                        foreach (OleDbParameter param in parameters)
                        {
                            Debug.WriteLine($"Parametre: {param.ParameterName}, Değer: {param.Value}");
                        }
                    }

                    try
                    {
                        Debug.WriteLine("Veritabanı bağlantısı açılıyor...");
                        connection.Open();
                        Debug.WriteLine("Bağlantı açıldı, sorgu çalıştırılıyor...");
                        int result = command.ExecuteNonQuery();
                        Debug.WriteLine($"Sorgu çalıştırıldı. Etkilenen satır sayısı: {result}");
                        return result;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"ExecuteNonQuery hatası: {ex.Message}");
                        if (ex.InnerException != null)
                            Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                        throw;
                    }
                    finally
                    {
                        Debug.WriteLine("Bağlantı kapatılıyor...");
                        if (connection.State == ConnectionState.Open)
                            connection.Close();
                    }
                }
            }
        }

        // Tek değer döndüren SQL sorgusu çalıştırma
        public static object ExecuteScalar(string commandText, params OleDbParameter[] parameters)
        {
            Debug.WriteLine($"ExecuteScalar çağrıldı. SQL: {commandText}");
            using (OleDbConnection connection = GetConnection())
            {
                using (OleDbCommand command = new OleDbCommand(commandText, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                        foreach (OleDbParameter param in parameters)
                        {
                            Debug.WriteLine($"Parametre: {param.ParameterName}, Değer: {param.Value}");
                        }
                    }

                    try
                    {
                        Debug.WriteLine("Veritabanı bağlantısı açılıyor...");
                        connection.Open();
                        Debug.WriteLine("Bağlantı açıldı, sorgu çalıştırılıyor...");
                        object result = command.ExecuteScalar();
                        Debug.WriteLine($"Sorgu çalıştırıldı. Sonuç: {result}");
                        return result;
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"ExecuteScalar hatası: {ex.Message}");
                        if (ex.InnerException != null)
                            Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                        throw;
                    }
                    finally
                    {
                        Debug.WriteLine("Bağlantı kapatılıyor...");
                        if (connection.State == ConnectionState.Open)
                            connection.Close();
                    }
                }
            }
        }

        // DataSet döndüren SQL sorgusu çalıştırma
        public static DataSet ExecuteDataSet(string commandText, params OleDbParameter[] parameters)
        {
            using (OleDbConnection connection = GetConnection())
            {
                using (OleDbCommand command = new OleDbCommand(commandText, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    OleDbDataAdapter adapter = new OleDbDataAdapter(command);
                    DataSet dataSet = new DataSet();

                    connection.Open();
                    adapter.Fill(dataSet);
                    connection.Close();

                    return dataSet;
                }
            }
        }

        // DataTable döndüren SQL sorgusu çalıştırma
        public static DataTable ExecuteDataTable(string commandText, params OleDbParameter[] parameters)
        {
            DataSet ds = ExecuteDataSet(commandText, parameters);
            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }

        // OleDbDataReader döndüren SQL sorgusu çalıştırma
        public static OleDbDataReader ExecuteReader(string commandText, params OleDbParameter[] parameters)
        {
            OleDbConnection connection = GetConnection();
            OleDbCommand command = new OleDbCommand(commandText, connection);

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            connection.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
        }

        public static DataTable GetTableStructure(string tableName)
        {
            try
            {
                string query = $"SELECT * FROM [{tableName}] WHERE 1=0";
                return ExecuteDataTable(query);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"GetTableStructure Error: {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                return null;
            }
        }
    }
}