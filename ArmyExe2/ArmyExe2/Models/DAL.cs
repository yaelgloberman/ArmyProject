using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Net.WebRequestMethods;

namespace ArmyExe2.Models
{
    public class DAL
    {
        public Response GetAllUsers(SqlConnection connection)
        {
            Response response = new Response();
            SqlDataAdapter da = new SqlDataAdapter("select * from users", connection);
            DataTable dt = new DataTable();
            List<Username> listUser = new List<Username>();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Username user = new Username();
                    user.Id = Convert.ToString(dt.Rows[i]["ID"]);
                    user.FullName = Convert.ToString(dt.Rows[i]["FullName"]);
                    user.StreetAddress = Convert.ToString(dt.Rows[i]["StreetAddress"]);
                    user.Phone = Convert.ToString(dt.Rows[i]["Phone"]);
                    user.MobilePhone = Convert.ToString(dt.Rows[i]["MobilePhone"]);
                    user.BirthDate = Convert.ToDateTime(dt.Rows[i]["BirthDate"]);
                    user.ImageData = Convert.ToString(dt.Rows[i]["ImageData"]);
                    user.PositiveForCorona = dt.Rows[i]["PositiveForCorona"] == DBNull.Value ? null : Convert.ToDateTime(dt.Rows[i]["PositiveForCorona"]);
                    user.RecoverFromCorona = dt.Rows[i]["RecoverFromCorona"] == DBNull.Value ? null : Convert.ToDateTime(dt.Rows[i]["RecoverFromCorona"]);
                    listUser.Add(user);
                }
                response.StatusCode = 200;
                response.StatusMessage = "DATA FOUND";
                response.listUser = listUser;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "NO DATA FOUND";
                response.listUser = null;
            }


            return response;
        }
        public Response GetSickByDays(SqlConnection connection)
        {
            Response response = new Response();
            response.daysSick = new List<int>();

            string query = "SELECT DAY(PositiveForCorona) AS Day, COUNT(*) AS PositiveCount, PositiveForCorona AS Date " +
                  "FROM users " +
                  "WHERE MONTH(PositiveForCorona) = MONTH(GETDATE()) AND YEAR(PositiveForCorona) = YEAR(GETDATE()) AND RecoverFromCorona IS NULL " +
                  "GROUP BY DAY(PositiveForCorona), PositiveForCorona " +
                  "ORDER BY DAY(PositiveForCorona)";


            SqlCommand command = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            // Initialize the sickDays list with 0's for all days in the current month
            int numDays = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);
            for (int i = 1; i <= numDays; i++)
            {
                response.daysSick.Add(0);
            }

            while (reader.Read())
            {
                // Check if the positive case is in the current month
                DateTime positiveDate = (DateTime)reader["Date"];
                if (positiveDate.Month == DateTime.Today.Month && positiveDate.Year == DateTime.Today.Year)
                {
                    // Get the day of the positive case and add the count to the sickDays list
                    int day = (int)reader["Day"];
                    int count = (int)reader["PositiveCount"];
                    response.daysSick[day - 1] += count;
                }
            }
            reader.Close();
            response.daysSick = response.daysSick;
            return response;
        }
        public Response GetNotVaccinatedCount(SqlConnection connection)
        {
            int i = GetAllUsers(connection).listUser.Count();
            SqlDataAdapter da = new SqlDataAdapter("SELECT COUNT(*), id FROM VaccineDetails GROUP BY id", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            int groupCountCorona = dt.Rows.Count;

            SqlDataAdapter da1 = new SqlDataAdapter("select * from users", connection);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);
            int groupCountUser = dt1.Rows.Count;
            Response response = new();
            response.NotVaccinatedCount= groupCountUser - groupCountCorona;
            return response;
        }
        public Response GetUserById(SqlConnection connection, string id)
        {
            Response response = new Response();
            SqlDataAdapter da = new SqlDataAdapter("select * from users where id='" + id + "'", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                Username user = new Username();
                user.Id = Convert.ToString(dt.Rows[0]["ID"]);
                user.FullName = Convert.ToString(dt.Rows[0]["FullName"]);
                user.StreetAddress = Convert.ToString(dt.Rows[0]["StreetAddress"]);
                user.Phone = Convert.ToString(dt.Rows[0]["Phone"]);
                user.MobilePhone = Convert.ToString(dt.Rows[0]["MobilePhone"]);
                user.BirthDate = Convert.ToDateTime(dt.Rows[0]["BirthDate"]);
                user.ImageData = Convert.ToString(dt.Rows[0]["ImageData"]);
                user.PositiveForCorona = dt.Rows[0]["PositiveForCorona"] == DBNull.Value ? null : Convert.ToDateTime(dt.Rows[0]["PositiveForCorona"]);
                user.RecoverFromCorona = dt.Rows[0]["RecoverFromCorona"] == DBNull.Value ? null : Convert.ToDateTime(dt.Rows[0]["RecoverFromCorona"]);
                response.StatusCode = 200;
                response.StatusMessage = "DATA FOUND";
                response.user = user;
            }

            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "NO DATA FOUND";
                response.user = null;
            }

            return response;
        }
        public Response addUser(SqlConnection connection, Username user)
        {

            Response response = new Response();
            int ExistUser = ExistInUsersTable(connection, user.Id);
            if (ExistUser == 0)
            {
                if(user.ImageData==null)
                {
                    user.ImageData = "https://p.kindpng.com/picc/s/24-248253_user-profile-default-image-png-clipart-png-download.png";
                }
       
                SqlCommand cmd1 = new SqlCommand("INSERT INTO users(ID, FullName,StreetAddress,Phone, MobilePhone, BirthDate, ImageData,PositiveForCorona , RecoverFromCorona) VALUES('" + user.Id + "','" + user.FullName + "','" + user.StreetAddress + "',  '" + user.Phone + "', '" + user.MobilePhone + "','" + user.BirthDate + "', '" + user.ImageData + "', '" + user.PositiveForCorona + "', '" + user.RecoverFromCorona + "')", connection);
                SqlCommand cmd = new SqlCommand("INSERT INTO users(ID, FullName, StreetAddress, Phone, MobilePhone, BirthDate, ImageData, PositiveForCorona, RecoverFromCorona) " +
                 "VALUES('" + user.Id + "', '" + user.FullName + "', '" + user.StreetAddress + "', '" + user.Phone + "', '" + user.MobilePhone + "', '" + user.BirthDate + "', '" + user.ImageData + "'" + ", " + (user.PositiveForCorona == null ? "NULL" : "'" + user.PositiveForCorona + "'") + ", " + (user.RecoverFromCorona == null ? "NULL" : "'" + user.RecoverFromCorona + "'") + ")", connection);

                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();
                if (i > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "user Added";
                    response.user = user;
                }

                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "no data inserted.";

                }

            }

            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "the id already exist.";

            }


            return response;
        }
        public Response addPositive(SqlConnection connection, String id, DateTime date)
        {
            Response response = new Response();
            int ExistUser = ExistInUsersTable(connection, id);
            if (ExistUser == 1)
            {
                var res = GetUserById(connection, id);
                if (res.user.PositiveForCorona == null)
                {
             
                    res.user.PositiveForCorona = date;
                    SqlCommand cmd = new SqlCommand("UPDATE users SET PositiveForCorona = @date WHERE ID = @id", connection);
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    int i = cmd.ExecuteNonQuery();
                    connection.Close();
                    if (i > 0)
                    {
                        response.StatusCode = 200;
                        response.StatusMessage = "positive date Added";
                        response.user = res.user;
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "no positive date Added.";
                    }
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "positive date already exist.";
                }
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "the id does not exist.";
            }
            return response;
        }
        public Response addRecover(SqlConnection connection, String id, DateTime date)
        {
            Response response = new Response();
            int ExistUser = ExistInUsersTable(connection, id);
            if (ExistUser == 1)
            {
                var res = GetUserById(connection, id);
                if (res.user.PositiveForCorona != null)
                {
                    if (date >= res.user.PositiveForCorona)
                    {
                        if (res.user.RecoverFromCorona != null)
                        {

                            res.user.RecoverFromCorona = date;
                            SqlCommand cmd = new SqlCommand("UPDATE users SET RecoverFromCorona = @date WHERE ID = @id", connection);
                            cmd.Parameters.AddWithValue("@date", date);
                            cmd.Parameters.AddWithValue("@id", id);
                            connection.Open();
                            int i = cmd.ExecuteNonQuery();
                            connection.Close();
                            if (i > 0)
                            {
                                response.StatusCode = 200;
                                response.StatusMessage = "recover date Added";
                                response.user = res.user;
                            }
                            else
                            {
                                response.StatusCode = 100;
                                response.StatusMessage = "no recover date Added.";
                            }
                        }
                        else
                        {
                            response.StatusCode = 100;
                            response.StatusMessage = "recover date already exist.";
                        }
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.StatusMessage = "the date of recover cant be before positive test.";

                    }

                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "cant add recover date if there is not positive date.";

                }


            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "the id does not exist.";
            }
            return response;
        }
        public Response GetAllCoronaDetails(SqlConnection connection)
        {
            Response response = new Response();
            SqlDataAdapter da = new SqlDataAdapter("select * from VaccineDetails", connection);
            DataTable dt = new DataTable();
            List<CoronaDetails> listCoronaDetails = new List<CoronaDetails>();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CoronaDetails coronaDetails = new CoronaDetails();
                    coronaDetails.Id = Convert.ToString(dt.Rows[i]["id"]);
                    coronaDetails.CoronaVaccine = Convert.ToDateTime(dt.Rows[i]["CoronaVaccine"]);
                    coronaDetails.CoronaManufacturer = Convert.ToString(dt.Rows[i]["CoronaManufacturer"]);
                    listCoronaDetails.Add(coronaDetails);

                }
                response.StatusCode = 200;
                response.StatusMessage = "List Corona Details Found";
                response.listCoronaDetails = listCoronaDetails;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "List Corona Details Not Found";
                response.listUser = null;
            }
            return response;
        }
        public Response GetCoronaDetailsById(SqlConnection connection, string id)
        {          
                Response response = new Response();
                SqlDataAdapter da = new SqlDataAdapter("select * from VaccineDetails where id = '" + id + "'", connection);
                DataTable dt = new DataTable();
                List<CoronaDetails> listCoronaDetails = new List<CoronaDetails>();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        CoronaDetails coronaDetails = new CoronaDetails();
                        coronaDetails.Id = Convert.ToString(dt.Rows[i]["id"]);
                        coronaDetails.CoronaVaccine = Convert.ToDateTime(dt.Rows[i]["CoronaVaccine"]);
                        coronaDetails.CoronaManufacturer = Convert.ToString(dt.Rows[i]["CoronaManufacturer"]);
                        listCoronaDetails.Add(coronaDetails);

                    }
                    response.StatusCode = 200;
                    response.StatusMessage = "List Corona Details Found";
                    response.listCoronaDetails = listCoronaDetails;
                  }

              
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "List Corona Details Not Found";
                    response.listUser = null;
                }
                return response;
        }
        public int ExistInCoronaTable(SqlConnection connection, string id)
        {
            Response response = new Response();
            SqlDataAdapter da = new SqlDataAdapter("select * from VaccineDetails where id = '" + id + "'", connection);
            DataTable dt = new DataTable();

            da.Fill(dt);
            return dt.Rows.Count;

        }
        public int ExistInUsersTable(SqlConnection connection, string id)
        {
            Response response = new Response();
            SqlDataAdapter da = new SqlDataAdapter("select * from users where id = '" + id + "'", connection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt.Rows.Count;
        }
        public Response addCoronaDetails(SqlConnection connection, CoronaDetails coronaDetails)
        {
            Response response = new Response();
            int ExistCorona = ExistInCoronaTable(connection, coronaDetails.Id);
            int ExistUser = ExistInUsersTable(connection, coronaDetails.Id);
            if (ExistUser!=0 && ExistCorona < 4 )
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO VaccineDetails(Id, CoronaVaccine, CoronaManufacturer) VALUES('" + coronaDetails.Id + "','" + coronaDetails.CoronaVaccine + "','" + coronaDetails.CoronaManufacturer + "')", connection);
                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();
                if (i > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "Corona Details Added";
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "No Corona Details Added";

                }
            }
            else
            {
                if (ExistUser==0)
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "Cant add the corona detail because id not found";
                    response.corona = null;

                }
                else
                { 
                    response.StatusCode = 100;
                    response.StatusMessage = "Cant add the corona detail because there are more than 4 corona details";
                    response.corona = null;

                }
            }
            return response;
        }
        public bool IsValidID(string idNum)
        {
            // Check that the ID number is the correct length
            if (idNum.Length != 9)
                return false;

            // Check that the ID number consists only of digits
            if (!int.TryParse(idNum, out int _))
                return false;
            // If all checks pass, the ID number is valid
            return true;
        }
        public List<string> ValidateUsername(Username username)
        {
            List<string> errors = new List<string>();

            // Validate Id
            if (string.IsNullOrEmpty(username.Id) || username.Id.Length != 9 || !username.Id.All(char.IsDigit))
            {
                errors.Add("Id must be a non-empty string of 9 digits.");
            }

            // Validate FullName
            if (string.IsNullOrEmpty(username.FullName) || username.FullName.Length > 50)
            {
                errors.Add("FullName must be a non-empty string of at most 50 characters.");
            }

            // Validate StreetAddress
            if (string.IsNullOrEmpty(username.StreetAddress))
            {
                errors.Add("StreetAddress must be a non-empty string.");
            }

            // Validate Phone
            if (!string.IsNullOrEmpty(username.Phone))
            {
                // Remove hyphen from Phone and concatenate parts
                username.Phone = Regex.Replace(username.Phone, @"-", "");
                username.Phone = $"{username.Phone.Substring(0, 3)}{username.Phone.Substring(3)}";

                if (!Regex.IsMatch(username.Phone, @"^\d{9}$"))
                {
                    errors.Add("Phone must be a string of 9 digits.");
                }
            }


            // Validate MobilePhone
            if (!string.IsNullOrEmpty(username.MobilePhone))
            {
                // Remove hyphen from MobilePhone
                username.MobilePhone = Regex.Replace(username.MobilePhone, @"-", "");

                if (!Regex.IsMatch(username.MobilePhone, @"^\d{10}$"))
                {
                    errors.Add("MobilePhone, if provided, must be a string of 10 digits.");
                }
            }


            // Validate BirthDate
            if (username.BirthDate > DateTime.Today)
            {
                errors.Add("BirthDate must be a date in the past.");
            }

            // Validate ImageData
            if (username.ImageData != null && username.ImageData.Length > 500)
            {
                errors.Add("ImageData, if provided, must be a string of at most 500 characters.");
            }

            return errors;
        }
        public List<string> ValidateCoronaDetails(CoronaDetails cd)
        {
            List<string> errors = new List<string>();

            // Validate Id
            if (string.IsNullOrEmpty(cd.Id) || cd.Id.Length != 9 || !cd.Id.All(char.IsDigit))
            {
                errors.Add("Id must be a non-empty string of 9 digits.");
            }

            // Validate FullName
            if (string.IsNullOrEmpty(cd.CoronaManufacturer) || cd.CoronaManufacturer.Length > 50)
            {
                errors.Add("Corona manufacturer must be a non-empty string of at most 50 characters.");
            }

            // Validate CoronaVaccine
            if (cd.CoronaVaccine == DateTime.MinValue)
            {
                errors.Add("Corona Vaccine date is required.");
            }
            else if (cd.CoronaVaccine.Year < 1900 || cd.CoronaVaccine.Year > 9999)
            {
                errors.Add("Corona Vaccine date is invalid.");
            }

            return errors;
        }

    }
}