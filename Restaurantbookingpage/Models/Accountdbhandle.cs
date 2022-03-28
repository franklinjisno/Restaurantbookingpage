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
            //string constring = ConfigurationManager.ConnectionStrings["BookingConn"].ToString();
            //string constring = "Data Source=LAPTOP-5IQ1TLRU;Initial Catalog=Bookingpage;Integrated Security=True";
            string constring = "Data Source=ASUSX515;Initial Catalog=Bookingpage;Integrated Security=True";
            con = new SqlConnection(constring);
        }
        // **************** ADD NEW Account *********************
        public bool AddAccount(Registration smodel)
        {
            connection();
            SqlCommand cmd = new SqlCommand("AddNewAccount", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Name", smodel.Name);
            cmd.Parameters.AddWithValue("@Email", smodel.Email);
            cmd.Parameters.AddWithValue("@Password", smodel.Password);
            cmd.Parameters.AddWithValue("@ContactNo", smodel.ContactNo);
            cmd.Parameters.AddWithValue("@CreatedBy",smodel.CreatedBy);
            cmd.Parameters.AddWithValue("@CreatedDate", smodel.CreatedBy);
            cmd.Parameters.AddWithValue("@ModifiedBy", smodel.ModifiedBy);
            cmd.Parameters.AddWithValue("@ModifiedDate", smodel.ModifiedDate);

            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
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
                            CreatedBy= Convert.ToString(dr["CreatedBy"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                           ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                        });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return accountList;
        }
      
        public List<Registration> GetAccounts()
        {
            connection();
            List<Registration> Accountlist = new List<Registration>();

            SqlCommand cmd = new SqlCommand("GetAccountDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                Accountlist.Add(
                    new Registration
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Name = Convert.ToString(dr["Name"]),
                        Email = Convert.ToString(dr["Email"]),
                        Password = Convert.ToString(dr["Password"]),
                        ContactNo = Convert.ToString(dr["ContactNo"]),
                        CreatedBy = Convert.ToString(dr["CreatedBy"]),
                        CreatedDate = Convert.ToString(dr["CreatedDate"]),
                        ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                        ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
                    });
            }
            return Accountlist;
        }

        // ***************** UPDATE Account DETAILS *********************
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
                                CreatedBy = Convert.ToString(dr["CreatedBy"]),
                                CreatedDate = Convert.ToString(dr["CreatedDate"]),
                                ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                                ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
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
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
        public bool UpdateDetails(Registration smodel)
        {

            connection();
            SqlCommand cmd = new SqlCommand("UpdateAccountDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", smodel.Id);
            cmd.Parameters.AddWithValue("@Name", smodel.Name);
            cmd.Parameters.AddWithValue("@Email", smodel.Email);
            cmd.Parameters.AddWithValue("@Password", smodel.Password);
            cmd.Parameters.AddWithValue("@ContactNo", smodel.ContactNo);
            cmd.Parameters.AddWithValue("@CreatedBy", smodel.CreatedBy);
            cmd.Parameters.AddWithValue("@CreatedDate", smodel.CreatedBy);
            cmd.Parameters.AddWithValue("@ModifiedBy", smodel.ModifiedBy);
            cmd.Parameters.AddWithValue("@ModifiedDate", smodel.ModifiedDate);
            con.Open();
            int i = cmd.ExecuteNonQuery();
            con.Close();

            if (i >= 1)
                return true;
            else
                return false;
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
                            CreatedBy = Convert.ToString(dr["CreatedBy"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToString(dr["ModifiedBy"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),
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
