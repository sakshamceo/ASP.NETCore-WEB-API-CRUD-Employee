using EmploployeedB.Helper;
using System.Data;
using System.Data.SqlClient;

namespace EmploployeedB.DAL
{
    public class MyDAL
    {
        private readonly string cs;
        private object models;

        public MyDAL(string connectionString)
        {
            cs = connectionString;
        }

        public int MyStoredProcedure(int parameter1, string parameter2 , string parameter3)
        {

            using (var connection = new SqlConnection(cs))
            {
                using (var command = new SqlCommand("DataOfEmp", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter("@Emp_id", SqlDbType.Int));
                    command.Parameters["@Emp_id"].Value = parameter1;

                    command.Parameters.Add(new SqlParameter("@Emp_name", SqlDbType.VarChar, 50));
                    command.Parameters["@Emp_name"].Value = parameter2;

                    command.Parameters.Add(new SqlParameter("@Emp_city", SqlDbType.VarChar, 50));
                    command.Parameters["@Emp_city"].Value = parameter3;

                    connection.Open();
                    return command.ExecuteNonQuery();
                }
            }
        }



    }
}
