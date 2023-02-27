using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EmploployeedB.Helper
{
    public static class dbHelper
    {
        public static SqlConnection cnn;
        public static SqlCommand command;
        public static SqlDataReader dataReader;
        public static string connectionString = "Data Source = DESKTOP-BVF0FR5;Initial Catalog=empdb; Integrated Security=true;";
        // IMPORTANT get connection string from appsettings


        public static bool ExecuteQuery(string query)
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
                return false;
            }
            finally
            {
                cnn.Close();
            }
            
        }

        // method to execute SP to be done by Saksham

       /* public static bool Executesp(string sp_name, List<SqlParameter> parameters)
        {



            return false;


        }*/
    }


    
}
