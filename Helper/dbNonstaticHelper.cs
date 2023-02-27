using System;
using System.Data.SqlClient;

namespace EmploployeedB.Helper
{
    public class dbNonstaticHelper
    {
        public  SqlConnection cnn;
        public  SqlCommand command;
        public  SqlDataReader dataReader;

        public  string connectionString = "";
        // IMPORTANT get connection string from appsettings

        //constructor
        public dbNonstaticHelper(string connectionstring)
        {
            this.connectionString = connectionstring;
        }

        public bool ExecuteNonQuery(string query)
        {
            cnn = new SqlConnection(connectionString);
            try
            {

                command = new SqlCommand(query, cnn);
                cnn.Open();
                command.ExecuteNonQuery();
                return true;

            }
            catch (Exception ex)
            {
                // Saksham to implement
            }
            finally
            {
                cnn.Close();
            }

            return false;

        }

    }
}
