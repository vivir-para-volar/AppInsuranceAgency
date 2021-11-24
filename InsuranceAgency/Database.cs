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
            bool flag = false;
            try
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
                        flag = true;
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
                            flag = true;
                            throw new Exception("Данный сотрудник больше не работает");
                        }
                    }
                    reader.Close();
                    con.Close();
                }
            }
            catch (Exception exp)
            {
                if (flag)
                {
                    throw exp;
                }
                else
                {
                    throw new Exception("Ошибка в работе БД");
                }
            }
        }



        //Функции добавления
        public static void AddCarWithPhotos(Car car, List<string> photos)
        {
            bool flag = false;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    string query1 = "SELECT ID FROM Cars WHERE VIN = @vin";
                    SqlCommand command1 = new SqlCommand(query1, con);
                    command1.Parameters.Add(new SqlParameter("@vin", car.VIN));


                    string query = "BEGIN TRANSACTION " +
                                   "INSERT INTO Cars(Model, VIN, RegistrationPlate, VehiclePassport) " +
                                   "VALUES(@model, @vin, @registrationPlate, @vehiclePassport); " +

                                   "DECLARE @id INT; " +
                                   "SET @id=SCOPE_IDENTITY(); " +

                                   "INSERT INTO Photos(EncodedPhoto, CarID) VALUES ";
                    for (var i = 0; i < photos.Count - 1; i++)
                    {
                        query += "(@photo" + i + ", @id), ";
                    }
                    query += "(@photo" + (photos.Count - 1) + ", @id); " +
                             "COMMIT TRANSACTION";

                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.Add(new SqlParameter("@model", car.Model));
                    command.Parameters.Add(new SqlParameter("@vin", car.VIN));
                    command.Parameters.Add(new SqlParameter("@registrationPlate", car.RegistrationPlate));
                    command.Parameters.Add(new SqlParameter("@vehiclePassport", car.VehiclePassport));
                    for (var i = 0; i < photos.Count; i++)
                    {
                        command.Parameters.Add(new SqlParameter("@photo" + i, photos[i]));
                    }

                    con.Open();
                    SqlDataReader reader = command1.ExecuteReader();
                    if (reader.HasRows)
                    {
                        flag = true;
                        throw new Exception("Данный VIN номер уже используется");
                    }
                    reader.Close();

                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception exp)
            {
                if (flag)
                {
                    throw exp;
                }
                else
                {
                    throw new Exception("Ошибка в работе БД");
                }
            }
        }

        public static void AddEmployee(Employee employee)
        {
            bool flag = false;
            try
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
                        flag = true;
                        throw new Exception("Данный телефон уже используется");
                    }
                    reader.Close();

                    reader = command2.ExecuteReader();
                    if (reader.HasRows)
                    {
                        flag = true;
                        throw new Exception("Данный паспорт уже используется");
                    }
                    reader.Close();

                    reader = command3.ExecuteReader();
                    if (reader.HasRows)
                    {
                        flag = true;
                        throw new Exception("Данный логин уже занят");
                    }
                    reader.Close();

                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception exp)
            {
                if (flag)
                {
                    throw exp;
                }
                else
                {
                    throw new Exception("Ошибка в работе БД");
                }
            }
        }

        public static void AddInsuranceEvent(InsuranceEvent insuranceEvent)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    string query = "INSERT INTO InsuranceEvents(Date, InsurancePayment, PolicyID) " +
                                   "VALUES(@date, @insurancePayment, @policyID)";
                    SqlCommand command = new SqlCommand(query, con);

                    command.Parameters.Add(new SqlParameter("@date", insuranceEvent.Date));
                    command.Parameters.Add(new SqlParameter("@insurancePayment", insuranceEvent.InsurancePayment));
                    command.Parameters.Add(new SqlParameter("@policyID", insuranceEvent.PolicyID));

                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch 
            {
                throw new Exception("Ошибка в работе БД");
            }
        }

        public static void AddPersonAllowedToDrive(PersonAllowedToDrive personAllowedToDrive)
        {
            bool flag = false;
            try
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
                        flag = true;
                        throw new Exception("Данное водительское удостоверение уже используется");
                    }
                    reader.Close();

                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception exp)
            {
                if (flag)
                {
                    throw exp;
                }
                else
                {
                    throw new Exception("Ошибка в работе БД");
                }
            }
        }

        public static void AddPolicyWithConnections(Policy policy, List<PersonAllowedToDrive> listPersonAllowedToDrive)
        {
            bool flag = false;
            try
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

                    string query = "BEGIN TRANSACTION " +
                                   "INSERT INTO Policies(InsuranceType, InsurancePremium, InsuranceAmount, DateOfConclusion, ExpirationDate, PolicyholderID, CarID, EmployeeID) " +
                                   "VALUES(@insuranceType, " +
                                          "@insurancePremium, " +
                                          "@insuranceAmount, " +
                                          "@dateOfConclusion, " +
                                          "@expirationDate, " +
                                          "@policyholderID, " +
                                          "@carID, " +
                                          "@employeeID" +
                                   "); " +

                                   "DECLARE @id INT; " +
                                   "SET @id=SCOPE_IDENTITY(); " +

                                   "INSERT INTO Connections(PolicyID, PersonAllowedToDriveID) VALUES ";
                    for (var i = 0; i < listPersonAllowedToDrive.Count - 1; i++)
                    {
                        query += "(@id, @personAllowedToDriveID" + i + "), ";
                    }
                    query += "(@id, @personAllowedToDriveID" + (listPersonAllowedToDrive.Count - 1) + "); " +
                             "COMMIT TRANSACTION";

                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.Add(new SqlParameter("@insuranceType", policy.InsuranceType));
                    command.Parameters.Add(new SqlParameter("@insurancePremium", policy.InsurancePremium));
                    command.Parameters.Add(new SqlParameter("@insuranceAmount", policy.InsuranceAmount));
                    command.Parameters.Add(new SqlParameter("@dateOfConclusion", policy.DateOfConclusion));
                    command.Parameters.Add(new SqlParameter("@expirationDate", policy.ExpirationDate));
                    command.Parameters.Add(new SqlParameter("@policyholderID", policy.PolicyholderID));
                    command.Parameters.Add(new SqlParameter("@carID", policy.CarID));
                    command.Parameters.Add(new SqlParameter("@employeeID", policy.EmployeeID));
                    for (var i = 0; i < listPersonAllowedToDrive.Count; i++)
                    {
                        command.Parameters.Add(new SqlParameter("@personAllowedToDriveID" + i, listPersonAllowedToDrive[i].ID));
                    }

                    con.Open();
                    SqlDataReader reader = command1.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        flag = true;
                        throw new Exception("Данного страхователя нет");
                    }
                    reader.Close();

                    reader = command2.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        flag = true;
                        throw new Exception("Данного автомобиля нет");
                    }
                    reader.Close();

                    reader = command3.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        flag = true;
                        throw new Exception("Данного сотрудника нет");
                    }
                    reader.Close();

                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception exp)
            {
                if (flag)
                {
                    throw exp;
                }
                else
                {
                    throw new Exception("Ошибка в работе БД");
                }
            }
        }

        public static void AddPolicyholder(Policyholder policyholder)
        {
            bool flag = false;
            try
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
                        flag = true;
                        throw new Exception("Данный телефон уже используется");
                    }
                    reader.Close();

                    reader = command2.ExecuteReader();
                    if (reader.HasRows)
                    {
                        flag = true;
                        throw new Exception("Данный паспорт уже используется");
                    }
                    reader.Close();

                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception exp)
            {
                if (flag)
                {
                    throw exp;
                }
                else
                {
                    throw new Exception("Ошибка в работе БД");
                }
            }
        }



        //Функции изменения
        public static void ChangeCarWithPhotos(Car car, List<Photo> listDeletePhotos, List<string> listAddPhotos)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    string query = "BEGIN TRANSACTION " +
                                   "UPDATE Cars " +
                                   "SET RegistrationPlate = @registrationPlate, " +
                                       "VehiclePassport = @vehiclePassport " +
                                   "WHERE ID = @id ";
                    if (listAddPhotos.Count != 0)
                    {
                        query += "INSERT INTO Photos(EncodedPhoto, CarID) VALUES ";
                        for (int i = 0; i < listAddPhotos.Count - 1; i++)
                        {
                            query += "(@photo" + i + ", @id), ";
                        }
                        query += "(@photo" + (listAddPhotos.Count - 1) + ", @id); ";
                    }
                    if (listDeletePhotos.Count != 0)
                    {
                        for (int i = 0; i < listDeletePhotos.Count; i++)
                        {
                            query += "DELETE FROM Photos WHERE ID = @photoID" + i + ";";
                        }
                    }
                    query += "COMMIT TRANSACTION";

                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.Add(new SqlParameter("@id", car.ID));
                    command.Parameters.Add(new SqlParameter("@registrationPlate", car.RegistrationPlate));
                    command.Parameters.Add(new SqlParameter("@vehiclePassport", car.VehiclePassport));
                    for (int i = 0; i < listAddPhotos.Count; i++)
                    {
                        command.Parameters.Add(new SqlParameter("@photo" + i, listAddPhotos[i]));
                    }
                    for (int i = 0; i < listDeletePhotos.Count; i++)
                    {
                        command.Parameters.Add(new SqlParameter("@photoID" + i, listDeletePhotos[i].ID));
                    }

                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch
            {
                throw new Exception("Ошибка в работе БД");
            }
        }

        public static void ChangeEmployee(Employee employee, bool changePassword)
        {
            bool flag = false;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    string query1 = "SELECT ID FROM Employees WHERE Telephone = @telephone AND ID <> @id";
                    SqlCommand command1 = new SqlCommand(query1, con);
                    command1.Parameters.Add(new SqlParameter("@telephone", employee.Telephone));
                    command1.Parameters.Add(new SqlParameter("@id", employee.ID));

                    string query2 = "SELECT ID FROM Employees WHERE Passport = @passport AND ID <> @id";
                    SqlCommand command2 = new SqlCommand(query2, con);
                    command2.Parameters.Add(new SqlParameter("@passport", employee.Passport));
                    command2.Parameters.Add(new SqlParameter("@id", employee.ID));

                    string query3 = "SELECT ID FROM Employees WHERE Login = @login AND ID <> @id";
                    SqlCommand command3 = new SqlCommand(query3, con);
                    command3.Parameters.Add(new SqlParameter("@login", employee.Login));
                    command3.Parameters.Add(new SqlParameter("@id", employee.ID));

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
                    SqlDataReader reader = command1.ExecuteReader();
                    if (reader.HasRows)
                    {
                        flag = true;
                        throw new Exception("Данный телефон уже используется");
                    }
                    reader.Close();

                    reader = command2.ExecuteReader();
                    if (reader.HasRows)
                    {
                        flag = true;
                        throw new Exception("Данный паспорт уже используется");
                    }
                    reader.Close();

                    reader = command3.ExecuteReader();
                    if (reader.HasRows)
                    {
                        flag = true;
                        throw new Exception("Данный логин уже занят");
                    }
                    reader.Close();

                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception exp)
            {
                if (flag)
                {
                    throw exp;
                }
                else
                {
                    throw new Exception("Ошибка в работе БД");
                }
            }
        }

        public static void ChangePersonAllowedToDrive(PersonAllowedToDrive personAllowedToDrive)
        {
            bool flag = false;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    string query1 = "SELECT ID FROM PersonsAllowedToDrive WHERE DrivingLicence = @drivingLicence AND ID <> @id";
                    SqlCommand command1 = new SqlCommand(query1, con);
                    command1.Parameters.Add(new SqlParameter("@drivingLicence", personAllowedToDrive.DrivingLicence));
                    command1.Parameters.Add(new SqlParameter("@id", personAllowedToDrive.ID));

                    string query = "UPDATE PersonsAllowedToDrive " +
                                   "SET FullName = @fullName, " +
                                       "DrivingLicence = @drivingLicence " +
                                   "WHERE ID = @id";
                    SqlCommand command = new SqlCommand(query, con);

                    command.Parameters.Add(new SqlParameter("@id", personAllowedToDrive.ID));
                    command.Parameters.Add(new SqlParameter("@fullName", personAllowedToDrive.FullName));
                    command.Parameters.Add(new SqlParameter("@drivingLicence", personAllowedToDrive.DrivingLicence));

                    con.Open();
                    SqlDataReader reader = command1.ExecuteReader();
                    if (reader.HasRows)
                    {
                        flag = true;
                        throw new Exception("Данное водительское удостоверение уже используется");
                    }
                    reader.Close();

                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception exp)
            {
                if (flag)
                {
                    throw exp;
                }
                else
                {
                    throw new Exception("Ошибка в работе БД");
                }
            }
        }

        public static void ChangePolicyWithConnections(Policy policy, List<PersonAllowedToDrive> listDeletePersons, List<PersonAllowedToDrive> listAddPersons)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    string query = "BEGIN TRANSACTION " +
                                   "UPDATE Policies " +
                                   "SET InsurancePremium = @insurancePremium, " +
                                       "ExpirationDate = @expirationDate " +
                                   "WHERE ID = @id; ";
                    if (listAddPersons.Count != 0)
                    {
                        query += "INSERT INTO Connections(PolicyID, PersonAllowedToDriveID) VALUES ";
                        for (int i = 0; i < listAddPersons.Count - 1; i++)
                        {
                            query += "(@id, @addPersonID" + i + "), ";
                        }
                        query += "(@id, @addPersonID" + (listAddPersons.Count - 1) + "); ";
                    }
                    if (listDeletePersons.Count != 0)
                    {
                        for (int i = 0; i < listDeletePersons.Count; i++)
                        {
                            query += "DELETE FROM Connections WHERE PolicyID = @id AND PersonAllowedToDriveID = @deletePersonID" + i + ";";
                        }
                    }
                    query += "COMMIT TRANSACTION";

                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.Add(new SqlParameter("@id", policy.ID));
                    command.Parameters.Add(new SqlParameter("@insurancePremium", policy.InsurancePremium));
                    command.Parameters.Add(new SqlParameter("@expirationDate", policy.ExpirationDate));
                    for (int i = 0; i < listAddPersons.Count; i++)
                    {
                        command.Parameters.Add(new SqlParameter("@addPersonID" + i, listAddPersons[i].ID));
                    }
                    for (int i = 0; i < listDeletePersons.Count; i++)
                    {
                        command.Parameters.Add(new SqlParameter("@deletePersonID" + i, listDeletePersons[i].ID));
                    }

                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch
            {
                throw new Exception("Ошибка в работе БД");
            }
        }

        public static void ChangePolicyholder(Policyholder policyholder)
        {
            bool flag = false;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    string query1 = "SELECT ID FROM Policyholders WHERE Telephone = @telephone AND ID <> @id";
                    SqlCommand command1 = new SqlCommand(query1, con);
                    command1.Parameters.Add(new SqlParameter("@telephone", policyholder.Telephone));
                    command1.Parameters.Add(new SqlParameter("@id", policyholder.ID));

                    string query2 = "SELECT ID FROM Policyholders WHERE Passport = @passport AND ID <> @id";
                    SqlCommand command2 = new SqlCommand(query2, con);
                    command2.Parameters.Add(new SqlParameter("@passport", policyholder.Passport));
                    command2.Parameters.Add(new SqlParameter("@id", policyholder.ID));

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
                    SqlDataReader reader = command1.ExecuteReader();
                    if (reader.HasRows)
                    {
                        flag = true;
                        throw new Exception("Данный телефон уже используется");
                    }
                    reader.Close();

                    reader = command2.ExecuteReader();
                    if (reader.HasRows)
                    {
                        flag = true;
                        throw new Exception("Данный паспорт уже используется");
                    }
                    reader.Close();

                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception exp)
            {
                if (flag)
                {
                    throw exp;
                }
                else
                {
                    throw new Exception("Ошибка в работе БД");
                }
            }
        }



        //Функции удаления
        public static void DeleteCar(int id)
        {
            bool flag = false;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    string query1 = "SELECT ID FROM Policies WHERE CarID = @carID";
                    SqlCommand command1 = new SqlCommand(query1, con);
                    command1.Parameters.Add(new SqlParameter("@carID", id));

                    string query = "DELETE FROM Cars WHERE ID = @id";
                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.Add(new SqlParameter("@id", id));

                    con.Open();
                    SqlDataReader reader = command1.ExecuteReader();
                    if (reader.HasRows)
                    {
                        flag = true;
                        throw new Exception("Вы не можете удалить данный автомобили, так как на него оформлен полис");
                    }
                    reader.Close();

                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception exp)
            {
                if (flag)
                {
                    throw exp;
                }
                else
                {
                    throw new Exception("Ошибка в работе БД");
                }
            }

        }

        public static void DeleteEmployee(int id)
        {
            bool flag = false;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    string query1 = "SELECT ID FROM Policies WHERE EmployeeID = @employeeID";
                    SqlCommand command1 = new SqlCommand(query1, con);
                    command1.Parameters.Add(new SqlParameter("@employeeID", id));

                    string query = "DELETE FROM Employees WHERE ID = @id";
                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.Add(new SqlParameter("@id", id));

                    con.Open();
                    SqlDataReader reader = command1.ExecuteReader();
                    if (reader.HasRows)
                    {
                        flag = true;
                        throw new Exception("Вы не можете удалить данного сотрудника, так как он оформил полис");
                    }
                    reader.Close();

                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception exp)
            {
                if (flag)
                {
                    throw exp;
                }
                else
                {
                    throw new Exception("Ошибка в работе БД");
                }
            }
        }

        public static void DeletePersonAllowedToDrive(int id)
        {
            bool flag = false;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    string query1 = "SELECT * FROM Connections WHERE PersonAllowedToDriveID = @personAllowedToDriveID";
                    SqlCommand command1 = new SqlCommand(query1, con);
                    command1.Parameters.Add(new SqlParameter("@personAllowedToDriveID", id));

                    string query = "DELETE FROM PersonsAllowedToDrive WHERE ID = @id";
                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.Add(new SqlParameter("@id", id));

                    con.Open();
                    SqlDataReader reader = command1.ExecuteReader();
                    if (reader.HasRows)
                    {
                        flag = true;
                        throw new Exception("Вы не можете удалить данного водителя, так как на него оформлен полис");
                    }
                    reader.Close();

                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception exp)
            {
                if (flag)
                {
                    throw exp;
                }
                else
                {
                    throw new Exception("Ошибка в работе БД");
                }
            }
        }

        public static void DeletePolicyholder(int id)
        {
            bool flag = false;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    string query1 = "SELECT ID FROM Policies WHERE PolicyholderID = @policyholderID";
                    SqlCommand command1 = new SqlCommand(query1, con);
                    command1.Parameters.Add(new SqlParameter("@policyholderID", id));

                    string query = "DELETE FROM Policyholders WHERE ID = @id";
                    SqlCommand command = new SqlCommand(query, con);

                    command.Parameters.Add(new SqlParameter("@id", id));

                    con.Open();
                    SqlDataReader reader = command1.ExecuteReader();
                    if (reader.HasRows)
                    {
                        flag = true;
                        throw new Exception("Вы не можете удалить данного страхователя, так как он оформил полис");
                    }
                    reader.Close();

                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception exp)
            {
                if (flag)
                {
                    throw exp;
                }
                else
                {
                    throw new Exception("Ошибка в работе БД");
                }
            }
        }



        //Функции поиска
        public static Car SearchCar(string vin)
        {
            bool flag = false;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    string query = "SELECT * FROM Cars WHERE VIN = @vin";
                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.Add(new SqlParameter("@vin", vin));

                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    Car car;
                    if (!reader.HasRows)
                    {
                        flag = true;
                        throw new Exception("Данный автомобиль не существует");
                    }
                    while (reader.Read())
                    {
                        car = new Car(Convert.ToInt32(reader["ID"].ToString()),
                                      reader["Model"].ToString(),
                                      reader["VIN"].ToString(),
                                      reader["RegistrationPlate"].ToString(),
                                      reader["VehiclePassport"].ToString());
                        reader.Close();
                        con.Close();
                        return car;
                    }
                    return null;
                }
            }
            catch (Exception exp)
            {
                if (flag)
                {
                    throw exp;
                }
                else
                {
                    throw new Exception("Ошибка в работе БД");
                }
            }
        }

        public static Car SearchCarID(int id)
        {
            bool flag = false;
            try
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
                        flag = true;
                        throw new Exception("Данный автомобиль не существует");
                    }
                    while (reader.Read())
                    {
                        car = new Car(Convert.ToInt32(reader["ID"].ToString()),
                                      reader["Model"].ToString(),
                                      reader["VIN"].ToString(),
                                      reader["RegistrationPlate"].ToString(),
                                      reader["VehiclePassport"].ToString());
                        reader.Close();
                        con.Close();
                        return car;
                    }
                    return null;
                }
            }
            catch (Exception exp)
            {
                if (flag)
                {
                    throw exp;
                }
                else
                {
                    throw new Exception("Ошибка в работе БД");
                }
            }
        }

        public static List<Connection> SearchConnection(int policyID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    var list = new List<Connection>();

                    string query = "SELECT * FROM Connections WHERE PolicyID = @policyID";
                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.Add(new SqlParameter("@policyID", policyID));

                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var connection = new Connection(Convert.ToInt32(reader["PolicyID"].ToString()),
                                                        Convert.ToInt32(reader["PersonAllowedToDriveID"].ToString()));
                        list.Add(connection);
                    }
                    reader.Close();
                    con.Close();
                    return list;
                }
            }
            catch
            {
                throw new Exception("Ошибка в работе БД");
            }
        }

        public static Employee SearchEmployee(string telephoneOrPassport)
        {
            bool flag = false;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    string query = "SELECT * FROM Employees WHERE Telephone = @telephoneOrPassport OR Passport = @telephoneOrPassport";
                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.Add(new SqlParameter("@telephoneOrPassport", telephoneOrPassport));

                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    Employee employee;
                    if (!reader.HasRows)
                    {
                        flag = true;
                        throw new Exception("Данный сотрудник не существует");
                    }
                    while (reader.Read())
                    {
                        employee = new Employee(Convert.ToInt32(reader["ID"].ToString()),
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
            catch (Exception exp)
            {
                if (flag)
                {
                    throw exp;
                }
                else
                {
                    throw new Exception("Ошибка в работе БД");
                }
            }
        }

        public static Employee SearchEmployeeID(int id)
        {
            bool flag = false;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    string query = "SELECT * FROM Employees WHERE ID = @id";
                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.Add(new SqlParameter("@id", id));

                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    Employee employee;
                    if (!reader.HasRows)
                    {
                        flag = true;
                        throw new Exception("Данный сотрудник не существует");
                    }
                    while (reader.Read())
                    {
                        employee = new Employee(Convert.ToInt32(reader["ID"].ToString()),
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
            catch (Exception exp)
            {
                if (flag)
                {
                    throw exp;
                }
                else
                {
                    throw new Exception("Ошибка в работе БД");
                }
            }
        }

        public static Employee SearchEmployeeLogin(string login)
        {
            bool flag = false;
            try
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
                        flag = true;
                        throw new Exception("Данный сотрудник не существует");
                    }
                    while (reader.Read())
                    {
                        employee = new Employee(Convert.ToInt32(reader["ID"].ToString()),
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
            catch (Exception exp)
            {
                if (flag)
                {
                    throw exp;
                }
                else
                {
                    throw new Exception("Ошибка в работе БД");
                }
            }
        }

        public static List<InsuranceEvent> SearchInsuranceEvent(int policyID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    var list = new List<InsuranceEvent>();

                    string query = "SELECT * FROM InsuranceEvents WHERE PolicyID = @policyID ORDER BY Date";
                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.Add(new SqlParameter("@policyID", policyID));

                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var insuranceEvent = new InsuranceEvent(Convert.ToInt32(reader["ID"].ToString()),
                                                                Convert.ToDateTime(reader["Date"].ToString()),
                                                                Convert.ToInt32(reader["InsurancePayment"].ToString()),
                                                                Convert.ToInt32(reader["PolicyID"].ToString()));
                        list.Add(insuranceEvent);
                    }
                    reader.Close();
                    con.Close();
                    return list;
                }
            }
            catch
            {
                throw new Exception("Ошибка в работе БД");
            }
        }

        public static DateTime SearchInsuranceEventMaxDate(int policyID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    string query = "SELECT MAX(Date) FROM InsuranceEvents WHERE PolicyID = @policyID";
                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.Add(new SqlParameter("@policyID", policyID));

                    con.Open();
                    var temp = command.ExecuteScalar().ToString();
                    if (temp == String.Empty)
                    {
                        return default;
                    }
                    DateTime dateMax = Convert.ToDateTime(temp);
                    con.Close();
                    return dateMax;
                }
            }
            catch
            {
                throw new Exception("Ошибка в работе БД");
            }
        }

        public static PersonAllowedToDrive SearchPersonAllowedToDrive(string drivingLicence)
        {
            bool flag = false;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    string query = "SELECT * FROM PersonsAllowedToDrive WHERE DrivingLicence = @drivingLicence";
                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.Add(new SqlParameter("@drivingLicence", drivingLicence));

                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    PersonAllowedToDrive personAllowedToDrive;
                    if (!reader.HasRows)
                    {
                        flag = true;
                        throw new Exception("Данный водитель не существует");
                    }
                    while (reader.Read())
                    {
                        personAllowedToDrive = new PersonAllowedToDrive(Convert.ToInt32(reader["ID"].ToString()),
                                                                        reader["FullName"].ToString(),
                                                                        reader["DrivingLicence"].ToString());
                        reader.Close();
                        con.Close();
                        return personAllowedToDrive;
                    }
                    return null;
                }
            }
            catch (Exception exp)
            {
                if (flag)
                {
                    throw exp;
                }
                else
                {
                    throw new Exception("Ошибка в работе БД");
                }
            }
        }

        public static PersonAllowedToDrive SearchPersonAllowedToDriveID(int id)
        {
            bool flag = false;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    string query = "SELECT * FROM PersonsAllowedToDrive WHERE ID = @id";
                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.Add(new SqlParameter("@id", id));

                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    PersonAllowedToDrive personAllowedToDrive;
                    if (!reader.HasRows)
                    {
                        flag = true;
                        throw new Exception("Данный водитель не существует");
                    }
                    while (reader.Read())
                    {
                        personAllowedToDrive = new PersonAllowedToDrive(Convert.ToInt32(reader["ID"].ToString()),
                                                                        reader["FullName"].ToString(),
                                                                        reader["DrivingLicence"].ToString());
                        reader.Close();
                        con.Close();
                        return personAllowedToDrive;
                    }
                    return null;
                }
            }
            catch (Exception exp)
            {
                if (flag)
                {
                    throw exp;
                }
                else
                {
                    throw new Exception("Ошибка в работе БД");
                }
            }
        }

        public static List<Photo> SearchPhoto(int carID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    var list = new List<Photo>();

                    string query = "SELECT * FROM Photos WHERE CarID = @carID";
                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.Add(new SqlParameter("@carID", carID));

                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var policy = new Photo(Convert.ToInt32(reader["ID"].ToString()),
                                               reader["EncodedPhoto"].ToString(),
                                               Convert.ToInt32(reader["CarID"].ToString()));
                        list.Add(policy);
                    }
                    reader.Close();
                    con.Close();
                    return list;
                }
            }
            catch
            {
                throw new Exception("Ошибка в работе БД");
            }
        }

        public static List<Policy> SearchPolicy(int policyholderID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    var list = new List<Policy>();

                    string query = "SELECT * FROM Policies WHERE PolicyholderID = @policyholderID ORDER BY DateOfConclusion DESC";
                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.Add(new SqlParameter("@policyholderID", policyholderID));

                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var policy = new Policy(Convert.ToInt32(reader["ID"].ToString()),
                                                reader["InsuranceType"].ToString(),
                                                Convert.ToInt32(reader["InsurancePremium"].ToString()),
                                                Convert.ToInt32(reader["InsuranceAmount"].ToString()),
                                                Convert.ToDateTime(reader["DateOfConclusion"].ToString()),
                                                Convert.ToDateTime(reader["ExpirationDate"].ToString()),
                                                Convert.ToInt32(reader["PolicyholderID"].ToString()),
                                                Convert.ToInt32(reader["CarID"].ToString()),
                                                Convert.ToInt32(reader["EmployeeID"].ToString()));
                        list.Add(policy);
                    }
                    reader.Close();
                    con.Close();
                    return list;
                }
            }
            catch
            {
                throw new Exception("Ошибка в работе БД");
            }
        }

        public static Policy SearchPolicyID(int id)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    var list = new List<Policy>();

                    string query = "SELECT * FROM Policies WHERE ID = @id";
                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.Add(new SqlParameter("@id", id));

                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        var policy = new Policy(Convert.ToInt32(reader["ID"].ToString()),
                                                reader["InsuranceType"].ToString(),
                                                Convert.ToInt32(reader["InsurancePremium"].ToString()),
                                                Convert.ToInt32(reader["InsuranceAmount"].ToString()),
                                                Convert.ToDateTime(reader["DateOfConclusion"].ToString()),
                                                Convert.ToDateTime(reader["ExpirationDate"].ToString()),
                                                Convert.ToInt32(reader["PolicyholderID"].ToString()),
                                                Convert.ToInt32(reader["CarID"].ToString()),
                                                Convert.ToInt32(reader["EmployeeID"].ToString()));
                        reader.Close();
                        con.Close();
                        return policy;
                    }
                    return null;
                }
            }
            catch
            {
                throw new Exception("Ошибка в работе БД");
            }
        }

        public static Policyholder SearchPolicyholder(string telephoneOrPassport)
        {
            bool flag = false;
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    string query = "SELECT * FROM Policyholders WHERE Telephone = @telephoneOrPassport OR Passport = @telephoneOrPassport";
                    SqlCommand command = new SqlCommand(query, con);
                    command.Parameters.Add(new SqlParameter("@telephoneOrPassport", telephoneOrPassport));

                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    Policyholder policyholder;
                    if (!reader.HasRows)
                    {
                        flag = true;
                        throw new Exception("Данный страхователь не существует");
                    }
                    while (reader.Read())
                    {
                        policyholder = new Policyholder(Convert.ToInt32(reader["ID"].ToString()),
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
            catch (Exception exp)
            {
                if (flag)
                {
                    throw exp;
                }
                else
                {
                    throw new Exception("Ошибка в работе БД");
                }
            }
        }

        public static Policyholder SearchPolicyholderID(int id)
        {
            bool flag = false;
            try
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
                        flag = true;
                        throw new Exception("Данный страхователь не существует");
                    }
                    while (reader.Read())
                    {
                        policyholder = new Policyholder(Convert.ToInt32(reader["ID"].ToString()),
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
            catch (Exception exp)
            {
                if (flag)
                {
                    throw exp;
                }
                else
                {
                    throw new Exception("Ошибка в работе БД");
                }
            }
        }



        //Функции получения всего списка
        public static List<Car> AllCars()
        {
            try
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
                        var car = new Car(Convert.ToInt32(reader["ID"].ToString()),
                                          reader["Model"].ToString(),
                                          reader["VIN"].ToString(),
                                          reader["RegistrationPlate"].ToString(),
                                          reader["VehiclePassport"].ToString());
                        list.Add(car);
                    }
                    reader.Close();
                    con.Close();
                    return list;
                }
            }
            catch
            {
                throw new Exception("Ошибка в работе БД");
            }
        }

        public static List<Employee> AllEmployees()
        {
            try
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
                        var employee = new Employee(Convert.ToInt32(reader["ID"].ToString()),
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
            catch
            {
                throw new Exception("Ошибка в работе БД");
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
            try
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
                        var personAllowedToDrive = new PersonAllowedToDrive(Convert.ToInt32(reader["ID"].ToString()),
                                                                            reader["FullName"].ToString(),
                                                                            reader["DrivingLicence"].ToString());
                        list.Add(personAllowedToDrive);
                    }
                    reader.Close();
                    con.Close();
                    return list;
                }
            }
            catch
            {
                throw new Exception("Ошибка в работе БД");
            }
        }

        public static List<Policy> AllPolicies()
        {
            try
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
                        var policy = new Policy(Convert.ToInt32(reader["ID"].ToString()),
                                                reader["InsuranceType"].ToString(),
                                                Convert.ToInt32(reader["InsurancePremium"].ToString()),
                                                Convert.ToInt32(reader["InsuranceAmount"].ToString()),
                                                Convert.ToDateTime(reader["DateOfConclusion"].ToString()),
                                                Convert.ToDateTime(reader["ExpirationDate"].ToString()),
                                                Convert.ToInt32(reader["PolicyholderID"].ToString()),
                                                Convert.ToInt32(reader["CarID"].ToString()),
                                                Convert.ToInt32(reader["EmployeeID"].ToString()));
                        list.Add(policy);
                    }
                    reader.Close();
                    con.Close();
                    return list;
                }
            }
            catch
            {
                throw new Exception("Ошибка в работе БД");
            }
        }

        public static List<Policyholder> AllPolicyholders()
        {
            try
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
                        var policyholder = new Policyholder(Convert.ToInt32(reader["ID"].ToString()),
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
            catch
            {
                throw new Exception("Ошибка в работе БД");
            }
        }



        //Отчёты
        public static (int, int, int) Reports(string insuranceType, DateTime dateStart, DateTime dateEnd)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    string query;
                    SqlCommand command;
                    if (insuranceType == "ОСАГО и КАСКО")
                    {
                        query = "SELECT (SELECT SUM(InsurancePayment) " +
                                "FROM InsuranceEvents " +
                                "WHERE Date >= @dateStart AND Date <= @dateEnd), " +

                                "COUNT(ID), SUM(InsurancePremium) " +
                                "FROM Policies " +
                                "WHERE DateOfConclusion >= @dateStart AND DateOfConclusion <= @dateEnd";
                        command = new SqlCommand(query, con);
                        command.Parameters.Add(new SqlParameter("@dateStart", dateStart));
                        command.Parameters.Add(new SqlParameter("@dateEnd", dateEnd));
                    }
                    else
                    {
                        query = "SELECT (SELECT SUM(InsurancePayment) " +
                                "FROM Policies as p LEFT JOIN InsuranceEvents as ie ON p.ID = ie.PolicyID " +
                                "WHERE p.InsuranceType = @insuranceType AND Date >= @dateStart AND Date <= @dateEnd), " +

                                "COUNT(ID), SUM(InsurancePremium) " +
                                "FROM Policies " +
                                "WHERE InsuranceType = @insuranceType AND DateOfConclusion >= @dateStart AND DateOfConclusion <= @dateEnd";
                        command = new SqlCommand(query, con);
                        command.Parameters.Add(new SqlParameter("@insuranceType", insuranceType));
                        command.Parameters.Add(new SqlParameter("@dateStart", dateStart));
                        command.Parameters.Add(new SqlParameter("@dateEnd", dateEnd));
                    }

                    con.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    int countContracts = 0, sumContracts = 0, sumInsuranceEvents = 0;
                    while (reader.Read())
                    {
                        string tempSumInsuranceEvents = reader.GetValue(0).ToString();
                        if (tempSumInsuranceEvents == String.Empty) tempSumInsuranceEvents = "0";
                        string tempCountContracts = reader.GetValue(1).ToString();
                        if (tempCountContracts == String.Empty) tempCountContracts = "0";
                        string tempSumContracts = reader.GetValue(2).ToString();
                        if (tempSumContracts == String.Empty) tempSumContracts = "0";

                        countContracts = Convert.ToInt32(tempCountContracts);
                        sumContracts = Convert.ToInt32(tempSumContracts);
                        sumInsuranceEvents = Convert.ToInt32(tempSumInsuranceEvents);
                    }
                    reader.Close();
                    var touple = (countContracts, sumContracts, sumInsuranceEvents);
                    con.Close();
                    return touple;
                }
            }
            catch
            {
                throw new Exception("Ошибка в работе БД");
            }
        }
    }
}
