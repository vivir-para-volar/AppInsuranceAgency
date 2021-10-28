using InsuranceAgency.Struct;
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace InsuranceAgency
{
    public static class Database
    {
        private static string ConnectionString = "Server=localhost;" + "database=InsuranceAgency;" + "Integrated Security=True";

        private static string _login;
        private static bool _admin = true;

        public static string Login { get { return _login; } private set { _login = value; } }
        public static bool Admin { get { return _admin; } private set { _admin = value; } }


        public static string GetHash(string input)
        {
            var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            return Convert.ToBase64String(hash);
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
                //SqlParameter passwordParam = new SqlParameter("@Password", GetHash(password));
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
        

        public static void AddEmployee(Employee employee)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                string query = "INSERT INTO Employees VALUES(@FullName, @Birthday, @Telephone, @Passport, @Login, @Password, @Admin, @Works)";
                SqlCommand command = new SqlCommand(query, con);

                SqlParameter fullNameParam = new SqlParameter("@FullName", employee.FullName);
                command.Parameters.Add(fullNameParam);
                SqlParameter birthdayParam = new SqlParameter("@Birthday", employee.Birthday);
                command.Parameters.Add(birthdayParam);
                SqlParameter telephoneParam = new SqlParameter("@Telephone", employee.Telephone);
                command.Parameters.Add(telephoneParam);
                SqlParameter passportParam = new SqlParameter("@Passport", employee.Passport);
                command.Parameters.Add(passportParam);
                SqlParameter loginParam = new SqlParameter("@Login", employee.Login);
                command.Parameters.Add(loginParam);
                SqlParameter passwordParam = new SqlParameter("@Password", GetHash(employee.Password));
                command.Parameters.Add(passwordParam);
                SqlParameter adminParam = new SqlParameter("@Admin", employee.Admin);
                command.Parameters.Add(adminParam);
                SqlParameter worksParam = new SqlParameter("@Works", employee.Works);
                command.Parameters.Add(worksParam);
            }
        }

        public static void AddPolicyholder(Policyholder policyholder)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                con.Open();

                string query = "INSERT INTO Policyholders VALUES(@FullName, @Birthday, @Telephone, @Passport)";
                SqlCommand command = new SqlCommand(query, con);

                SqlParameter fullNameParam = new SqlParameter("@FullName", policyholder.FullName);
                command.Parameters.Add(fullNameParam);
                SqlParameter birthdayParam = new SqlParameter("@Birthday", policyholder.Birthday);
                command.Parameters.Add(birthdayParam);
                SqlParameter telephoneParam = new SqlParameter("@Telephone", policyholder.Telephone);
                command.Parameters.Add(telephoneParam);
                SqlParameter passportParam = new SqlParameter("@Passport", policyholder.Passport);
                command.Parameters.Add(passportParam);
            }
        }
    }
}
