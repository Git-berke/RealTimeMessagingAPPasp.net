using System;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Web;

namespace DoctorPatientChat
{
    public class DatabaseConnection
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["ChatConnectionString"].ConnectionString;

        // Bağlantı nesnesi oluşturma
        public static OleDbConnection GetConnection()
        {
            return new OleDbConnection(connectionString);
        }

        // SQL komutunu çalıştırma (INSERT, UPDATE, DELETE)
        public static int ExecuteNonQuery(string commandText, params OleDbParameter[] parameters)
        {
            using (OleDbConnection connection = GetConnection())
            {
                using (OleDbCommand command = new OleDbCommand(commandText, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    connection.Close();

                    return result;
                }
            }
        }

        // Tek değer döndüren SQL sorgusu çalıştırma
        public static object ExecuteScalar(string commandText, params OleDbParameter[] parameters)
        {
            using (OleDbConnection connection = GetConnection())
            {
                using (OleDbCommand command = new OleDbCommand(commandText, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    object result = command.ExecuteScalar();
                    connection.Close();

                    return result;
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
    }
}