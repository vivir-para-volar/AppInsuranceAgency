using InsuranceAgency.Struct;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace InsuranceAgency
{
    public static class Database
    {
        private static string ConnectionString = "Server=localhost;" + "database=DBInsuranceAgency;" + "Integrated Security=True";

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
                string query = "SELECT Login, Admin, Works FROM Employees WHERE Login = @login AND Password = @password";
                SqlCommand command = new SqlCommand(query, con);

                command.Parameters.Add(new SqlParameter("@login", login));
                command.Parameters.Add(new SqlParameter("@Password", GetHash(password)));

                con.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (!reader.HasRows)
                {
                    throw new Exception("Неправильно указан логин и/или пароль");
                }
                while (reader.Read())
                {
                    if (Convert.ToBoolean(reader["Works"]))
                    {
                        _login = reader["Login"].ToString();
                        _admin = Convert.ToBoolean(reader["Admin"]);
                    }
                    else
                    {
                        throw new Exception("Данный сотрудник больше не работает");
                    }
                }
                reader.Close();
                con.Close();
            }
        }



        //Функции добавления
        public static void AddCar(Car car)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string query1 = "SELECT ID FROM Cars WHERE VIN = @vin";
                SqlCommand command1 = new SqlCommand(query1, con);
                command1.Parameters.Add(new SqlParameter("@vin", car.VIN));

                string query = "INSERT INTO Cars(Model, VIN, RegistrationPlate, VehiclePassport, Image) " + 
                               "VALUES(@model, @vin, @registrationPlate, @vehiclePassport, @image)";
                SqlCommand command = new SqlCommand(query, con);

                command.Parameters.Add(new SqlParameter("@model", car.Model));
                command.Parameters.Add(new SqlParameter("@vin", car.VIN));
                command.Parameters.Add(new SqlParameter("@registrationPlate", car.RegistrationPlate));
                command.Parameters.Add(new SqlParameter("@vehiclePassport", car.VehiclePassport));
                command.Parameters.Add(new SqlParameter("@image", car.Image));

                con.Open();
                SqlDataReader reader = command1.ExecuteReader();
                if (reader.HasRows)
                {
                    throw new Exception("Данный VIN номер уже используется");
                }
                reader.Close();

                command.ExecuteNonQuery();
                con.Close();
            }
        }

        public static void AddConnection(Connection connection)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string query1 = "SELECT ID FROM Policies WHERE ID = @id";
                SqlCommand command1 = new SqlCommand(query1, con);
                command1.Parameters.Add(new SqlParameter("@id", connection.PolicyID));

                string query2 = "SELECT ID FROM PersonsAllowedToDrive WHERE ID = @id";
                SqlCommand command2 = new SqlCommand(query2, con);
                command2.Parameters.Add(new SqlParameter("@id", connection.PersonAllowedToDriveID));

                string query = "INSERT INTO Connections(PolicyID, PersonAllowedToDriveID) " +
                               "VALUES(" +
                                   "@policyID, " +
                                   "@personAllowedToDriveID" +
                               ")";
                SqlCommand command = new SqlCommand(query, con);

                command.Parameters.Add(new SqlParameter("@policyID", connection.PolicyID));
                command.Parameters.Add(new SqlParameter("@personAllowedToDriveID", connection.PersonAllowedToDriveID));

                con.Open();
                SqlDataReader reader = command1.ExecuteReader();
                if (!reader.HasRows)
                {
                    throw new Exception("Данного полиса не существует");
                }
                reader.Close();

                reader = command2.ExecuteReader();
                if (!reader.HasRows)
                {
                    throw new Exception("Данного водителя не существует");
                }
                reader.Close();
                command.ExecuteNonQuery();
                con.Close();
            }
        }

        public static void AddEmployee(Employee employee)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string query1 = "SELECT ID FROM Employees WHERE Telephone = @telephone";
                SqlCommand command1 = new SqlCommand(query1, con);
                command1.Parameters.Add(new SqlParameter("@telephone", employee.Telephone));

                string query2 = "SELECT ID FROM Employees WHERE Passport = @passport";
                SqlCommand command2 = new SqlCommand(query2, con);
                command2.Parameters.Add(new SqlParameter("@passport", employee.Passport));

                string query3 = "SELECT ID FROM Employees WHERE Login = @login";
                SqlCommand command3 = new SqlCommand(query3, con);
                command3.Parameters.Add(new SqlParameter("@login", employee.Login));

                string query = "INSERT INTO Employees (FullName, Birthday, Telephone, Passport, Login, Password, Admin, Works) " +
                               "VALUES(@fullName, @birthday, @telephone, @passport, @login, @password, @admin, @works)";
                SqlCommand command = new SqlCommand(query, con);

                command.Parameters.Add(new SqlParameter("@fullName", employee.FullName));
                command.Parameters.Add(new SqlParameter("@birthday", employee.Birthday));
                command.Parameters.Add(new SqlParameter("@telephone", employee.Telephone));
                command.Parameters.Add(new SqlParameter("@passport", employee.Passport));
                command.Parameters.Add(new SqlParameter("@login", employee.Login));
                command.Parameters.Add(new SqlParameter("@password", GetHash(employee.Password)));
                command.Parameters.Add(new SqlParameter("@admin", employee.Admin));
                command.Parameters.Add(new SqlParameter("@works", employee.Works));

                con.Open();
                SqlDataReader reader = command1.ExecuteReader();
                if (reader.HasRows)
                {
                    throw new Exception("Данный телефон уже используется");
                }
                reader.Close();

                reader = command2.ExecuteReader();
                if (reader.HasRows)
                {
                    throw new Exception("Данный паспорт уже используется");
                }
                reader.Close();

                reader = command3.ExecuteReader();
                if (reader.HasRows)
                { 
                    throw new Exception("Данный логин уже занят");
                }
                reader.Close();

                command.ExecuteNonQuery();
                con.Close();
            }
        }

        //public static void AddInsuranceEvent(InsuranceEvent insuranceEvent)
        //{
        //    using (SqlConnection con = new SqlConnection(ConnectionString))
        //    {
        //        string query = "INSERT INTO InsuranceEvents(Date, InsurancePayment, PolicyID) " +
        //                       "VALUES(@date, @insurancePayment, @policyID)";
        //        SqlCommand command = new SqlCommand(query, con);

        //        command.Parameters.Add(new SqlParameter("@date", insuranceEvent.Date));
        //        command.Parameters.Add(new SqlParameter("@insurancePayment", insuranceEvent.InsurancePayment));
        //        command.Parameters.Add(new SqlParameter("@policyID", insuranceEvent.PolicyID));

        //        con.Open();
        //        command.ExecuteNonQuery();
        //        con.Close();
        //    }
        //}

        public static void AddPersonAllowedToDrive(PersonAllowedToDrive personAllowedToDrive)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string query1 = "SELECT ID FROM PersonsAllowedToDrive WHERE DrivingLicence = @drivingLicence";
                SqlCommand command1 = new SqlCommand(query1, con);
                command1.Parameters.Add(new SqlParameter("@drivingLicence", personAllowedToDrive.DrivingLicence));

                string query = "INSERT INTO PersonsAllowedToDrive(FullName, DrivingLicence) " + 
                               "VALUES(@fullName, @drivingLicence)";
                SqlCommand command = new SqlCommand(query, con);

                command.Parameters.Add(new SqlParameter("@fullName", personAllowedToDrive.FullName));
                command.Parameters.Add(new SqlParameter("@drivingLicence", personAllowedToDrive.DrivingLicence));

                con.Open();
                SqlDataReader reader = command1.ExecuteReader();
                if (reader.HasRows)
                {
                    throw new Exception("Данное водительское удостоверение уже используется");
                }
                reader.Close();

                command.ExecuteNonQuery();
                con.Close();
            }
        }

        public static void AddPolicyWithConnections(Policy policy, List<PersonAllowedToDrive> listPersonAllowedToDrive)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string query1 = "SELECT ID FROM Policyholders WHERE ID = @id";
                SqlCommand command1 = new SqlCommand(query1, con);
                command1.Parameters.Add(new SqlParameter("@id", policy.PolicyholderID));

                string query2 = "SELECT ID FROM Cars WHERE ID = @id";
                SqlCommand command2 = new SqlCommand(query2, con);
                command2.Parameters.Add(new SqlParameter("@id", policy.CarID));

                string query3 = "SELECT ID FROM Employees WHERE ID = @id";
                SqlCommand command3 = new SqlCommand(query3, con);
                command3.Parameters.Add(new SqlParameter("@id", policy.EmployeeID));

                string query = "INSERT INTO Policies(InsuranceType, InsurancePremium, InsuranceAmount, DateOfConclusion, ExpirationDate, PolicyholderID, CarID, EmployeeID) " +
                               "VALUES(" +
                                   "@insuranceType, " +
                                   "@insurancePremium, " +
                                   "@insuranceAmount, " +
                                   "@dateOfConclusion, " +
                                   "@expirationDate, " +
                                   "@policyholderID, " +
                                   "@carID, " +
                                   "@employeeID" +
                               "); SET @id=SCOPE_IDENTITY()";
                SqlCommand command = new SqlCommand(query, con);

                command.Parameters.Add(new SqlParameter("@insuranceType", policy.InsuranceType));
                command.Parameters.Add(new SqlParameter("@insurancePremium", policy.InsurancePremium));
                command.Parameters.Add(new SqlParameter("@insuranceAmount", policy.InsuranceAmount));
                command.Parameters.Add(new SqlParameter("@dateOfConclusion", policy.DateOfConclusion));
                command.Parameters.Add(new SqlParameter("@expirationDate", policy.ExpirationDate));
                command.Parameters.Add(new SqlParameter("@policyholderID", policy.PolicyholderID));
                command.Parameters.Add(new SqlParameter("@carID", policy.CarID));
                command.Parameters.Add(new SqlParameter("@employeeID", policy.EmployeeID));
                SqlParameter idParam = new SqlParameter
                {
                    ParameterName = "@id",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output // параметр выходной
                };
                command.Parameters.Add(idParam);

                con.Open();
                SqlDataReader reader = command1.ExecuteReader();
                if (!reader.HasRows)
                {
                    throw new Exception("Данного страхователя нет");
                }
                reader.Close();

                reader = command2.ExecuteReader();
                if (!reader.HasRows)
                {
                    throw new Exception("Данного автомобиля нет");
                }
                reader.Close();

                reader = command3.ExecuteReader();
                if (!reader.HasRows)
                {
                    throw new Exception("Данного сотрудника нет");
                }
                reader.Close();

                command.ExecuteNonQuery();

                var policyID = idParam.Value;

                foreach(var item in listPersonAllowedToDrive)
                {
                    string queryP1 = "SELECT ID FROM PersonsAllowedToDrive WHERE ID = @id";
                    SqlCommand commandP1 = new SqlCommand(queryP1, con);
                    commandP1.Parameters.Add(new SqlParameter("@id", item.ID));

                    string queryP = "INSERT INTO Connections(PolicyID, PersonAllowedToDriveID) " +
                               "VALUES(" +
                                   "@policyID, " +
                                   "@personAllowedToDriveID" +
                               ")";
                    SqlCommand commandP = new SqlCommand(queryP, con);

                    commandP.Parameters.Add(new SqlParameter("@policyID", policyID));
                    commandP.Parameters.Add(new SqlParameter("@personAllowedToDriveID", item.ID));

                    reader = commandP1.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        throw new Exception("Данного водителя нет");
                    }
                    reader.Close();

                    commandP.ExecuteNonQuery();
                }

                con.Close();
            }
        }

        public static void AddPolicyholder(Policyholder policyholder)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string query1 = "SELECT ID FROM Policyholders WHERE Telephone = @telephone";
                SqlCommand command1 = new SqlCommand(query1, con);
                command1.Parameters.Add(new SqlParameter("@telephone", policyholder.Telephone));

                string query2 = "SELECT ID FROM Policyholders WHERE Passport = @passport";
                SqlCommand command2 = new SqlCommand(query2, con);
                command2.Parameters.Add(new SqlParameter("@passport", policyholder.Passport));

                string query = "INSERT INTO Policyholders (FullName, Birthday, Telephone, Passport) " +
                               "VALUES(@fullName, @birthday, @telephone, @passport)";
                SqlCommand command = new SqlCommand(query, con);

                command.Parameters.Add(new SqlParameter("@fullName", policyholder.FullName));
                command.Parameters.Add(new SqlParameter("@birthday", policyholder.Birthday));
                command.Parameters.Add(new SqlParameter("@telephone", policyholder.Telephone));
                command.Parameters.Add(new SqlParameter("@passport", policyholder.Passport));

                con.Open();
                SqlDataReader reader = command1.ExecuteReader();
                if (reader.HasRows)
                {
                    throw new Exception("Данный телефон уже используется");
                }
                reader.Close();

                reader = command2.ExecuteReader();
                if (reader.HasRows)
                {
                    throw new Exception("Данный паспорт уже используется");
                }
                reader.Close();

                command.ExecuteNonQuery();
                con.Close();
            }
        }



        //Функции изменения
        public static void ChangeCar(Car car)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string query = "UPDATE Cars " +
                               "SET Model = @model, " +
                                   "VIN = @vin, " +
                                   "RegistrationPlate = @registrationPlate, " +
                                   "VehiclePassport = @vehiclePassport, " +
                                   "Image = @image " +
                               "WHERE ID = @id";
                SqlCommand command = new SqlCommand(query, con);

                command.Parameters.Add(new SqlParameter("@id", car.ID));
                command.Parameters.Add(new SqlParameter("@model", car.Model));
                command.Parameters.Add(new SqlParameter("@vin", car.VIN));
                command.Parameters.Add(new SqlParameter("@registrationPlate", car.RegistrationPlate));
                command.Parameters.Add(new SqlParameter("@vehiclePassport", car.VehiclePassport));
                command.Parameters.Add(new SqlParameter("@image", car.Image));

                con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }

        public static void ChangeEmployee(Employee employee, bool changePassword)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string query = "UPDATE Employees " +
                               "SET FullName = @fullName, " +
                                   "Birthday = @birthday, " +
                                   "Telephone = @telephone, " +
                                   "Passport = @passport, " +
                                   "Login = @login, " +
                                   "Password = @password, " +
                                   "Admin = @admin,  " +
                                   "Works = @works " +
                               "WHERE ID = @id";

                SqlCommand command = new SqlCommand(query, con);

                command.Parameters.Add(new SqlParameter("@id", employee.ID));
                command.Parameters.Add(new SqlParameter("@fullName", employee.FullName));
                command.Parameters.Add(new SqlParameter("@birthday", employee.Birthday));
                command.Parameters.Add(new SqlParameter("@telephone", employee.Telephone));
                command.Parameters.Add(new SqlParameter("@passport", employee.Passport));
                command.Parameters.Add(new SqlParameter("@login", employee.Login));
                if (changePassword) command.Parameters.Add(new SqlParameter("@password", GetHash(employee.Password)));
                else command.Parameters.Add(new SqlParameter("@password", employee.Password));
                command.Parameters.Add(new SqlParameter("@admin", employee.Admin));
                command.Parameters.Add(new SqlParameter("@works", employee.Works));

                con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }

        //public static void ChangeInsuranceEvent(InsuranceEvent insuranceEvent)
        //{
        //    using (SqlConnection con = new SqlConnection(ConnectionString))
        //    {
        //        string query = "UPDATE InsuranceEvents " + 
        //                       "SET Date = @date, " + 
        //                           "InsurancePayment = @insurancePayment, " +
        //                           "PolicyID = @policyID " +
        //                       "WHERE ID = @id";
        //        SqlCommand command = new SqlCommand(query, con);

        //        command.Parameters.Add(new SqlParameter("@id", insuranceEvent.ID));
        //        command.Parameters.Add(new SqlParameter("@date", insuranceEvent.Date));
        //        command.Parameters.Add(new SqlParameter("@insurancePayment", insuranceEvent.InsurancePayment));
        //        command.Parameters.Add(new SqlParameter("@policyID", insuranceEvent.PolicyID));

        //        con.Open();
        //        command.ExecuteNonQuery();
        //        con.Close();
        //    }
        //}

        public static void ChangePersonAllowedToDrive(PersonAllowedToDrive personAllowedToDrive)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string query = "UPDATE PersonsAllowedToDrive " +
                               "SET FullName = @fullName, " +
                                   "DrivingLicence = @drivingLicence " +
                               "WHERE ID = @id";
                SqlCommand command = new SqlCommand(query, con);

                command.Parameters.Add(new SqlParameter("@id", personAllowedToDrive.ID));
                command.Parameters.Add(new SqlParameter("@fullName", personAllowedToDrive.FullName));
                command.Parameters.Add(new SqlParameter("@drivingLicence", personAllowedToDrive.DrivingLicence));

                con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }

        //public static void ChangePolicy(Policy policy)
        //{
        //    using (SqlConnection con = new SqlConnection(ConnectionString))
        //    {
        //        string query = "UPDATE Policies " +
        //                       "SET InsuranceType = @insuranceType, " +
        //                           "InsuranceAmount = @insuranceAmount, " +
        //                           "InsurancePremium = @insurancePremium, " +
        //                           "DateOfConclusion = @dateOfConclusion, " +
        //                           "ExpirationDate = @expirationDate, " +
        //                           "PolicyholderID = (SELECT ID FROM Policyholders WHERE Passport = @policyholderPassport), " +
        //                           "CarID = (SELECT ID FROM Cars WHERE VIN = @vin), " +
        //                           "EmployeeID = (SELECT ID FROM Employees WHERE Passport = @employeePassport) " +
        //                       "WHERE ID = @id";
        //        SqlCommand command = new SqlCommand(query, con);

        //        command.Parameters.Add(new SqlParameter("@id", policy.ID));
        //        command.Parameters.Add(new SqlParameter("@insuranceType", policy.InsuranceType));
        //        command.Parameters.Add(new SqlParameter("@insuranceAmount", policy.InsuranceAmount));
        //        command.Parameters.Add(new SqlParameter("@insurancePremium", policy.InsurancePremium));
        //        command.Parameters.Add(new SqlParameter("@dateOfConclusion", policy.DateOfConclusion));
        //        command.Parameters.Add(new SqlParameter("@expirationDate", policy.ExpirationDate));
        //        command.Parameters.Add(new SqlParameter("@policyholderPassport", policy.PolicyholderPassport));
        //        command.Parameters.Add(new SqlParameter("@vin", policy.VIN));
        //        command.Parameters.Add(new SqlParameter("@employeePassport", policy.EmployeePassport));

        //        con.Open();
        //        command.ExecuteNonQuery();
        //        con.Close();
        //    }
        //}

        public static void ChangePolicyholder(Policyholder policyholder)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string query = "UPDATE Policyholders " +
                               "SET FullName = @fullName, " +
                                   "Birthday = @birthday, " +
                                   "Telephone = @telephone, " +
                                   "Passport = @passport " +
                               "WHERE ID = @id";
                SqlCommand command = new SqlCommand(query, con);

                command.Parameters.Add(new SqlParameter("@id", policyholder.ID));
                command.Parameters.Add(new SqlParameter("@fullName", policyholder.FullName));
                command.Parameters.Add(new SqlParameter("@birthday", policyholder.Birthday));
                command.Parameters.Add(new SqlParameter("@telephone", policyholder.Telephone));
                command.Parameters.Add(new SqlParameter("@passport", policyholder.Passport));

                con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }



        //Функции удаления
        public static void DeleteCar(string ID)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string query = "DELETE FROM Cars WHERE ID = @id";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.Add(new SqlParameter("@id", ID));

                con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }

        //public static void DeleteConnection(Connection connection)
        //{
        //    using (SqlConnection con = new SqlConnection(ConnectionString))
        //    {
        //        string query = "DELETE FROM Connections " +
        //                       "WHERE " +
        //                           "PolicyID = @policyID AND " +
        //                           "PersonAllowedToDriveID = (SELECT ID FROM PersonsAllowedToDrive WHERE DrivingLicence = @drivingLicence)";
        //        SqlCommand command = new SqlCommand(query, con);

        //        command.Parameters.Add(new SqlParameter("@policyID", connection.PolicyholderPassport));
        //        command.Parameters.Add(new SqlParameter("@drivingLicence", connection.DrivingLicence));

        //        con.Open();
        //        command.ExecuteNonQuery();
        //        con.Close();
        //    }
        //}

        public static void DeleteEmployee(string ID)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string query = "DELETE FROM Employees WHERE ID = @id";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.Add(new SqlParameter("@id", ID));
                
                con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }

        //public static void DeleteInsuranceEvent(InsuranceEvent insuranceEvent)
        //{
        //    using (SqlConnection con = new SqlConnection(ConnectionString))
        //    {
        //        string query = "DELETE FROM InsuranceEvents WHERE ID = @id";
        //        SqlCommand command = new SqlCommand(query, con);

        //        command.Parameters.Add(new SqlParameter("@id", insuranceEvent.ID));
                
        //        con.Open();
        //        command.ExecuteNonQuery();
        //        con.Close();
        //    }
        //}

        public static void DeletePersonAllowedToDrive(string ID)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string query = "DELETE FROM PersonsAllowedToDrive WHERE ID = @id";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.Add(new SqlParameter("@id", ID));
                
                con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }

        //public static void DeletePolicy(Policy policy)
        //{
        //    using (SqlConnection con = new SqlConnection(ConnectionString))
        //    {
        //        string query = "DELETE FROM Policies WHERE ID = @id";
        //        SqlCommand command = new SqlCommand(query, con);

        //        command.Parameters.Add(new SqlParameter("@id", policy.ID));
                
        //        con.Open();
        //        command.ExecuteNonQuery();
        //        con.Close();
        //    }
        //}

        public static void DeletePolicyholder(string ID)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string query = "DELETE FROM Policyholders WHERE ID = @id";
                SqlCommand command = new SqlCommand(query, con);

                command.Parameters.Add(new SqlParameter("@id", ID));
                
                con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }
        }

        

        //Функции поиска
        public static Car SearchCar(string search)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string query = "SELECT * FROM Cars WHERE VIN = @search";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.Add(new SqlParameter("@search", search));

                con.Open();
                SqlDataReader reader = command.ExecuteReader();

                Car car;
                if (!reader.HasRows)
                {
                    throw new Exception("Данного автомобиля не существует");
                }
                while (reader.Read())
                {
                    car = new Car(reader["ID"].ToString(),
                                  reader["Model"].ToString(),
                                  reader["VIN"].ToString(),
                                  reader["RegistrationPlate"].ToString(),
                                  reader["VehiclePassport"].ToString(),
                                  reader["Image"].ToString());
                    reader.Close();
                    con.Close();
                    return car;
                }
                return null;
            }
        }

        public static Car SearchCarID(string id)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string query = "SELECT * FROM Cars WHERE ID = @id";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.Add(new SqlParameter("@id", id));

                con.Open();
                SqlDataReader reader = command.ExecuteReader();

                Car car;
                if (!reader.HasRows)
                {
                    throw new Exception("Данного автомобиля не существует");
                }
                while (reader.Read())
                {
                    car = new Car(reader["Model"].ToString(),
                                  reader["VIN"].ToString(),
                                  reader["RegistrationPlate"].ToString(),
                                  reader["VehiclePassport"].ToString(),
                                  reader["Image"].ToString());
                    reader.Close();
                    con.Close();
                    return car;
                }
                return null;
            }
        }

        //public static void SearchConnection(Connection connection) { }

        public static Employee SearchEmployee(string search)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string query = "SELECT * FROM Employees WHERE Telephone = @search OR Passport = @search";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.Add(new SqlParameter("@search", search));

                con.Open();
                SqlDataReader reader = command.ExecuteReader();

                Employee employee;
                if (!reader.HasRows)
                {
                    throw new Exception("Данного сотрудника не существует");
                }
                while (reader.Read())
                {
                    employee = new Employee(reader["ID"].ToString(),
                                            reader["FullName"].ToString(),
                                            Convert.ToDateTime(reader["Birthday"].ToString()),
                                            reader["Telephone"].ToString(),
                                            reader["Passport"].ToString(),
                                            reader["Login"].ToString(),
                                            reader["Password"].ToString(),
                                            Convert.ToBoolean(reader["Admin"].ToString()),
                                            Convert.ToBoolean(reader["Works"].ToString()));
                    reader.Close();
                    con.Close();
                    return employee;
                }
                return null;
            }
        }

        public static Employee SearchEmployeeLogin(string login)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string query = "SELECT * FROM Employees WHERE Login = @login";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.Add(new SqlParameter("@login", login));

                con.Open();
                SqlDataReader reader = command.ExecuteReader();

                Employee employee;
                if (!reader.HasRows)
                {
                    throw new Exception("Данного сотрудника не существует");
                }
                while (reader.Read())
                {
                    employee = new Employee(reader["ID"].ToString(),
                                            reader["FullName"].ToString(),
                                            Convert.ToDateTime(reader["Birthday"].ToString()),
                                            reader["Telephone"].ToString(),
                                            reader["Passport"].ToString(),
                                            reader["Login"].ToString(),
                                            reader["Password"].ToString(),
                                            Convert.ToBoolean(reader["Admin"].ToString()),
                                            Convert.ToBoolean(reader["Works"].ToString()));
                    reader.Close();
                    con.Close();
                    return employee;
                }
                return null;
            }
        }

        //public static void SearchInsuranceEvent(InsuranceEvent insuranceEvent)
        //{
        //    using (SqlConnection con = new SqlConnection(ConnectionString))
        //    {
        //        string query = "INSERT INTO InsuranceEvents(Date, InsurancePayment, PolicyID) " +
        //                       "VALUES(@date, @insurancePayment, @policyID)";
        //        SqlCommand command = new SqlCommand(query, con);

        //        command.Parameters.Add(new SqlParameter("@date", insuranceEvent.Date));
        //        command.Parameters.Add(new SqlParameter("@insurancePayment", insuranceEvent.InsurancePayment));
        //        command.Parameters.Add(new SqlParameter("@policyID", insuranceEvent.PolicyID));

        //        con.Open();
        //        command.ExecuteNonQuery();
        //        con.Close();
        //    }
        //}

        public static PersonAllowedToDrive SearchPersonAllowedToDrive(string search)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string query = "SELECT * FROM PersonsAllowedToDrive WHERE DrivingLicence = @search";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.Add(new SqlParameter("@search", search));

                con.Open();
                SqlDataReader reader = command.ExecuteReader();

                PersonAllowedToDrive personAllowedToDrive;
                if (!reader.HasRows)
                {
                    throw new Exception("Данного водителя не существует");
                }
                while (reader.Read())
                {
                    personAllowedToDrive = new PersonAllowedToDrive(reader["ID"].ToString(),
                                                                    reader["FullName"].ToString(),
                                                                    reader["DrivingLicence"].ToString());
                    reader.Close();
                    con.Close();
                    return personAllowedToDrive;
                }
                return null;
            }
        }

        public static List<Policy> SearchPolicy(string search)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                var list = new List<Policy>();

                string query = "SELECT * FROM Policies WHERE PolicyholderID = @search";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.Add(new SqlParameter("@search", search));

                con.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var policy = new Policy(reader["ID"].ToString(),
                                            reader["InsuranceType"].ToString(),
                                            Convert.ToInt32(reader["InsurancePremium"].ToString()),
                                            Convert.ToInt32(reader["InsuranceAmount"].ToString()),
                                            Convert.ToDateTime(reader["DateOfConclusion"].ToString()),
                                            Convert.ToDateTime(reader["ExpirationDate"].ToString()),
                                            reader["PolicyholderID"].ToString(),
                                            reader["CarID"].ToString(),
                                            reader["EmployeeID"].ToString());
                    list.Add(policy);
                }
                reader.Close();
                con.Close();
                return list;
            }
        }

        public static Policyholder SearchPolicyholder(string search)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string query = "SELECT * FROM Policyholders WHERE Telephone = @search OR Passport = @search";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.Add(new SqlParameter("@search", search));

                con.Open();
                SqlDataReader reader = command.ExecuteReader();

                Policyholder policyholder;
                if (!reader.HasRows)
                {
                    throw new Exception("Данного страхователя не существует");
                }
                while (reader.Read())
                {
                    policyholder = new Policyholder(reader["ID"].ToString(),
                                            reader["FullName"].ToString(),
                                            Convert.ToDateTime(reader["Birthday"].ToString()),
                                            reader["Telephone"].ToString(),
                                            reader["Passport"].ToString());
                    reader.Close();
                    con.Close();
                    return policyholder;
                }
                return null;
            }
        }

        public static Policyholder SearchPolicyholderID(string id)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                string query = "SELECT * FROM Policyholders WHERE ID = @id";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.Add(new SqlParameter("@id", id));

                con.Open();
                SqlDataReader reader = command.ExecuteReader();

                Policyholder policyholder;
                if (!reader.HasRows)
                {
                    throw new Exception("Данного страхователя не существует");
                }
                while (reader.Read())
                {
                    policyholder = new Policyholder(reader["ID"].ToString(),
                                            reader["FullName"].ToString(),
                                            Convert.ToDateTime(reader["Birthday"].ToString()),
                                            reader["Telephone"].ToString(),
                                            reader["Passport"].ToString());
                    reader.Close();
                    con.Close();
                    return policyholder;
                }
                return null;
            }
        }



        //Функции получения всего списка
        public static List<Car> AllCars()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                var list = new List<Car>();

                string query = "SELECT * FROM Cars";
                SqlCommand command = new SqlCommand(query, con);

                con.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var car = new Car(reader["ID"].ToString(),
                                      reader["Model"].ToString(),
                                      reader["VIN"].ToString(),
                                      reader["RegistrationPlate"].ToString(),
                                      reader["VehiclePassport"].ToString(),
                                      reader["Image"].ToString());
                    list.Add(car);
                }
                reader.Close();
                con.Close();
                return list;
            }
        }

        public static List<Car> AllCarsDG()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                var list = new List<Car>();

                string query = "SELECT ID, Model, VIN, RegistrationPlate, VehiclePassport FROM Cars";
                SqlCommand command = new SqlCommand(query, con);

                con.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var car = new Car(reader["ID"].ToString(),
                                      reader["Model"].ToString(),
                                      reader["VIN"].ToString(),
                                      reader["RegistrationPlate"].ToString(),
                                      reader["VehiclePassport"].ToString(),
                                      "1");
                    list.Add(car);
                }
                reader.Close();
                con.Close();
                return list;
            }
        }

        public static List<Employee> AllEmployees()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                var list = new List<Employee>();

                string query = "SELECT * FROM Employees ORDER BY Works DESC, FullName";
                SqlCommand command = new SqlCommand(query, con);

                con.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var employee = new Employee(reader["ID"].ToString(),
                                                reader["FullName"].ToString(),
                                                Convert.ToDateTime(reader["Birthday"].ToString()),
                                                reader["Telephone"].ToString(),
                                                reader["Passport"].ToString(),
                                                reader["Login"].ToString(),
                                                reader["Password"].ToString(),
                                                Convert.ToBoolean(reader["Admin"].ToString()),
                                                Convert.ToBoolean(reader["Works"].ToString()));
                    list.Add(employee);
                }
                reader.Close();
                con.Close();
                return list;
            }
        }

        //public static List<InsuranceEvent> AllInsuranceEvents()
        //{
        //    using (SqlConnection con = new SqlConnection(ConnectionString))
        //    {
        //        var list = new List<InsuranceEvent>();

        //        string query = "SELECT * FROM InsuranceEvents";
        //        SqlCommand command = new SqlCommand(query, con);

        //        con.Open();
        //        SqlDataReader reader = command.ExecuteReader();

        //        while (reader.Read())
        //        {
        //            var insuranceEvent = new InsuranceEvent(reader["ID"].ToString(),
        //                                                    Convert.ToDateTime(reader["Date"].ToString()),
        //                                                    Convert.ToInt32(reader["InsurancePayment"].ToString()),
        //                                                    reader["PolicyID"].ToString());
        //            list.Add(insuranceEvent);
        //        }
        //        reader.Close();
        //        con.Close();
        //        return list;
        //    }
        //}

        public static List<PersonAllowedToDrive> AllPersonsAllowedToDrive()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                var list = new List<PersonAllowedToDrive>();

                string query = "SELECT * FROM PersonsAllowedToDrive ORDER BY FullName";
                SqlCommand command = new SqlCommand(query, con);

                con.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var personAllowedToDrive = new PersonAllowedToDrive(reader["ID"].ToString(),
                                                                        reader["FullName"].ToString(),
                                                                        reader["DrivingLicence"].ToString());
                    list.Add(personAllowedToDrive);
                }
                reader.Close();
                con.Close();
                return list;
            }
        }

        public static List<Policy> AllPolicies()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                var list = new List<Policy>();

                string query = "SELECT * FROM Policies";
                SqlCommand command = new SqlCommand(query, con);

                con.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var policy = new Policy(reader["ID"].ToString(),
                                            reader["InsuranceType"].ToString(),
                                            Convert.ToInt32(reader["InsurancePremium"].ToString()),
                                            Convert.ToInt32(reader["InsuranceAmount"].ToString()),
                                            Convert.ToDateTime(reader["DateOfConclusion"].ToString()),
                                            Convert.ToDateTime(reader["ExpirationDate"].ToString()),
                                            reader["PolicyholderID"].ToString(),
                                            reader["CarID"].ToString(),
                                            reader["EmployeeID"].ToString());
                    list.Add(policy);
                }
                reader.Close();
                con.Close();
                return list;
            }
        }

        public static List<Policyholder> AllPolicyholders()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                var list = new List<Policyholder>();

                string query = "SELECT * FROM Policyholders ORDER BY FullName";
                SqlCommand command = new SqlCommand(query, con);

                con.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var policyholder = new Policyholder(reader["ID"].ToString(),
                                                        reader["FullName"].ToString(),
                                                        Convert.ToDateTime(reader["Birthday"].ToString()),
                                                        reader["Telephone"].ToString(),
                                                        reader["Passport"].ToString());
                    list.Add(policyholder);
                }
                reader.Close();
                con.Close();
                return list;
            }
        }
    }
}
