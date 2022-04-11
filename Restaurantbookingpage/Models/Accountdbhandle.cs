using Restaurantbookingpage.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurantbookingpage
{
    public class Accountdbhandle
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = "Data Source=ASUSX515;Initial Catalog=Bookingpage;Integrated Security=True";
            con = new SqlConnection(constring);
        }
      

        // ********** View Account DETAILS ********************

        public List<Registration> GetAccount(int pageIndex, int pageSize, string searchValue)
        {
            connection();
            List<Registration> accountList = new List<Registration>();

            SqlCommand cmd = new SqlCommand("GetAccounts", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pageIndex", pageIndex);
            cmd.Parameters.AddWithValue("@pageSize", pageSize);
            cmd.Parameters.AddWithValue("@searchValue", searchValue);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            try
            {
                con.Open();
                sd.Fill(dt);
                con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    accountList.Add(
                        new Registration
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Name = Convert.ToString(dr["Name"]),
                            Email = Convert.ToString(dr["Email"]),
                            Password= Convert.ToString(dr["Password"]),
                            ContactNo= Convert.ToString(dr["ContactNo"]),
                            CreatedBy= Convert.ToInt32(dr["CreatedBy"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                           ModifiedBy = Convert.ToInt32(dr["ModifiedBy"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            Deleted=Convert.ToInt32(dr["Deleted"])
                        });

                }
                foreach (var n in accountList)
                {
                    if (n.CreatedBy > 0)
                    {
                        var user = GetAdminById(n.CreatedBy);
                        n.CreatedByName = user.Name;

                    }

                    if (n.ModifiedBy > 0)
                    {
                        var user = GetAdminById(n.ModifiedBy);
                        n.ModifiedByName = user.Name;
                    }
                    else
                    {
                        var user = GetUserByName(n.ModifiedByName);
                        n.ModifiedBy = user.ModifiedBy;
                    }

                }

            }
            catch (Exception ex)
            {
                throw;
            }
            return accountList;
        }

        public Registration GetUsersByName(string Email)
        {
            connection();
            Registration user = new Registration();
            SqlCommand cmd = new SqlCommand("GetUsersByName", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", Email);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            try
            {

                if (con.State == ConnectionState.Open)
                {
                    sd.Fill(dt);
                    con.Close();
                    if (dt.Rows.Count > 0)
                    {
                        user.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                        user.Name = Convert.ToString(dt.Rows[0]["Name"]);
                        user.Email = Convert.ToString(dt.Rows[0]["Email"]);
                        user.Password = Convert.ToString(dt.Rows[0]["Password"]);
                        user.ContactNo = Convert.ToString(dt.Rows[0]["ContactNo"]);
                        user.CreatedBy = Convert.ToInt32(dt.Rows[0]["CreatedBy"]);
                        user.CreatedDate = Convert.ToString(dt.Rows[0]["CreatedDate"]);
                        user.ModifiedBy = Convert.ToInt32(dt.Rows[0]["ModifiedBy"]);
                        user.ModifiedDate = Convert.ToString(dt.Rows[0]["ModifiedDate"]);
                        user.Deleted = Convert.ToInt32(dt.Rows[0]["Deleted"]);

                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return user;
        }


        public Registration GetUserByName(string Email)
        {
            connection();
            Registration user = new Registration();
            SqlCommand cmd = new SqlCommand("GetUserByName", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", Email);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            try
            {

                if (con.State == ConnectionState.Open)
                {
                    sd.Fill(dt);
                    con.Close();
                    if (dt.Rows.Count > 0)
                    {
                        user.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                        user.Name = Convert.ToString(dt.Rows[0]["Name"]);
                        user.Email = Convert.ToString(dt.Rows[0]["Email"]);
                        user.Password = Convert.ToString(dt.Rows[0]["Password"]);
                        user.ContactNo = Convert.ToString(dt.Rows[0]["ContactNo"]);

                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return user;
        }

        public List<Registration> GetById(int id)
        {
           
            
            connection();
            List<Registration> accountlist = new List<Registration>();
            SqlCommand cmd = new SqlCommand("GetAccountById", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            try
            {

                if (con.State == ConnectionState.Open)
                {
                    sd.Fill(dt);
                    con.Close();
                    foreach (DataRow dr in dt.Rows)
                    {
                        accountlist.Add(
                            new Registration
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Name = Convert.ToString(dr["Name"]),
                                Email = Convert.ToString(dr["Email"]),
                                Password = Base64Decode(Convert.ToString(dr["Password"])),
                                ContactNo = Convert.ToString(dr["ContactNo"]),
                                CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                ModifiedBy = Convert.ToInt32(dr["ModifiedBy"]),
                                ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                                Deleted = Convert.ToInt32(dr["Deleted"])



                            });
                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return accountlist;
            }



        public Registration GetAdminById(int id)
        {


            connection();
            Registration user = new Registration();
            SqlCommand cmd = new SqlCommand("GetAdminById", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            try
            {

                if (con.State == ConnectionState.Open)
                {
                    sd.Fill(dt);
                    con.Close();
                    if (dt.Rows.Count > 0)
                    {
                        user.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                        user.Name = Convert.ToString(dt.Rows[0]["Name"]);
                        user.Email = Convert.ToString(dt.Rows[0]["Email"]);
                        user.Password = Convert.ToString(dt.Rows[0]["Password"]);
                        user.ContactNo = Convert.ToString(dt.Rows[0]["ContactNo"]);
                        

                    }
                }
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
            }
            return user;
        }



      

        // ********************** DELETE Account DETAILS *******************
        public bool DeleteAccount(int id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("DeleteAccount", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", id);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }
        public bool ResetPassword(string password,string email)
        {
            connection();
            var encryptpassowrd = Base64Encode(password);

            SqlCommand cmd = new SqlCommand("PasswordReset", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@email",email);
            cmd.Parameters.AddWithValue("@password", encryptpassowrd);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }
        public bool ResetPasswordAdmin(string password, string email)
        {
            connection();
            var encryptpassowrd = Base64Encode(password);

            SqlCommand cmd = new SqlCommand("PasswordResetAdmin", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters.AddWithValue("@password", encryptpassowrd);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
        }


        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public int GetCounts(string searchValue)
        {
            int count = 0;
            connection();
            SqlCommand cmd = new SqlCommand("GetCounts", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@searchValue", searchValue);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                count = Convert.ToInt32(row["Column1"]);
            }
            return count;
        }

        public List<Registration> ListOfAccount()
        {
            connection();
            List<Registration> accountList = new List<Registration>();

            SqlCommand cmd = new SqlCommand("GetAccountDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            try
            {
                con.Open();
                sd.Fill(dt);
                con.Close();

                foreach (DataRow dr in dt.Rows)
                {
                    accountList.Add(
                        new Registration
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Name = Convert.ToString(dr["Name"]),
                            Email = Convert.ToString(dr["Email"]),
                            Password = Convert.ToString(dr["Password"]),
                            ContactNo = Convert.ToString(dr["ContactNo"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                            Deleted = Convert.ToInt32(dr["Deleted"])

                        });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return accountList;
        }

    }


}
