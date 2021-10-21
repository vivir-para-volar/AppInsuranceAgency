using System;
using System.Data.SqlClient;

namespace InsuranceAgency
{
    public static class Database
    {
        private static string ConnectionString = "Server=localhost;" + "database=InsuranceAgency;" + "Integrated Security=True";

        private static string _login;
        private static bool _admin;

        public static string Login
        {
            get => _login;
        }

        public static void Authorization(string login, string password)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                string query = "SELECT Login, Admin FROM Employees WHERE Login = @Login AND Password = @Password";
                SqlCommand command = new SqlCommand(query, con);
                SqlParameter loginParam = new SqlParameter("@Login", login);
                command.Parameters.Add(loginParam);
                SqlParameter passwordParam = new SqlParameter("@Password", password);
                command.Parameters.Add(passwordParam);

                SqlDataReader reader = command.ExecuteReader();
                if(!reader.HasRows)
                {
                    throw new Exception("Неправильно указан логин и/или пароль");
                }
                while (reader.Read())
                {
                    _login = reader["Login"].ToString();
                    _admin = Convert.ToBoolean(reader["Admin"]);
                }
                reader.Close();
            }
        }
    }
}
