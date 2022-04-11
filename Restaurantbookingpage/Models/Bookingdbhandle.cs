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
    public class Bookingdbhandle
    {
        private SqlConnection con;
        private void connection()
        {
            string constring = "Data Source=ASUSX515;Initial Catalog=Bookingpage;Integrated Security=True";
            con = new SqlConnection(constring);
        }

        public bool AddBooking(Booking smodel)
        {
            connection();
            SqlCommand cmd = new SqlCommand("AddNewBooking", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Customer_Name", smodel.Customer_Name);
            cmd.Parameters.AddWithValue("@Datetime", smodel.Datetime);
            cmd.Parameters.AddWithValue("@Dinning_Type", smodel.Dinning_Type);
            cmd.Parameters.AddWithValue("@NumberofGuest", smodel.NumberofGuest);
            cmd.Parameters.AddWithValue("@Category", smodel.Category);
            cmd.Parameters.AddWithValue("@Contact", smodel.Contact);
            cmd.Parameters.AddWithValue("@CreatedBy", smodel.CreatedBy);
            cmd.Parameters.AddWithValue("@CreatedDate", smodel.CreatedDate);
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

        // ********** VIEW Booking DETAILS ********************

        public List<Booking> GetBooking(int pageIndex, int pageSize, string searchValue)
        {
            connection();
            List<Booking> bookingList = new List<Booking>();

                SqlCommand cmd = new SqlCommand("GetBookings", con);
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
                    bookingList.Add(
                        new Booking
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Customer_Name = Convert.ToString(dr["Customer_Name"]),
                            Datetime = Convert.ToDateTime(dr["Datetime"]),
                            Dinning_Type = Convert.ToString(dr["Dinning_Type"]),
                            NumberofGuest = Convert.ToInt32(dr["NumberofGuest"]),
                            Contact = Convert.ToString(dr["Contact"]),
                            Category = Convert.ToString(dr["Category"]),
                             CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),

                        });
                }
                foreach (var n in bookingList)
                {
                    if (n.CreatedBy > 1000)
                    {
                        var user = GetUserById(n.CreatedBy);
                        n.CreatedByName = user.Name;
                    }
                    else

                    {
                        Accountdbhandle db=new Accountdbhandle();
                        var user = db.GetAdminById(n.CreatedBy);
                        n.CreatedByName = user.Name;
                    }
                    if (n.ModifiedBy > 1000)
                    {
                        var user = GetUserById(n.ModifiedBy);
                        n.ModifiedByName = user.Name;
                    }
                    else
                    {
                        Accountdbhandle db = new Accountdbhandle();

                        var user = db.GetAdminById(n.ModifiedBy);
                        n.ModifiedByName = user.Name;
                    }

                }

            }



            catch (Exception ex)
                {
                    throw;
                }
            return bookingList;
        }

        public Registration GetUserById(int id)
        {


            connection();
            Registration user = new Registration();
            SqlCommand cmd = new SqlCommand("GetUserById", con);
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



      
        public List<Booking> GetBookings()
        {
            connection();
            List<Booking> Bookinglist = new List<Booking>();

            SqlCommand cmd = new SqlCommand("GetBookingDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();

            con.Open();
            sd.Fill(dt);
            con.Close();

            foreach (DataRow dr in dt.Rows)
            {
                Bookinglist.Add(
                    new Booking
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Customer_Name = Convert.ToString(dr["Customer_Name"]),
                        Datetime = Convert.ToDateTime(dr["Datetime"]),
                        Contact = Convert.ToString(dr["Contact"]),
                        Category = Convert.ToString(dr["Category"]),
                        Dinning_Type = Convert.ToString(dr["Dinning_Type"]),
                        NumberofGuest = Convert.ToInt32(dr["NumberofGuest"]),
                        CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
                        CreatedDate = Convert.ToString(dr["CreatedDate"]),
                        ModifiedBy = Convert.ToInt32(dr["ModifiedBy"]),
                        ModifiedDate = Convert.ToString(dr["ModifiedDate"]),

                    });
            }
            return Bookinglist;
        }

        // ***************** UPDATE Booking DETAILS *********************
        public List<Booking> GetById(int id)
        {
            connection();
            List<Booking> bookinglist = new List<Booking>();
            SqlCommand cmd = new SqlCommand("GetBookingById", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            if(con.State == ConnectionState.Open)
            {
                sd.Fill(dt);
                con.Close();
                foreach (DataRow dr in dt.Rows)
                {
                    bookinglist.Add(
                        new Booking
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Customer_Name = Convert.ToString(dr["Customer_Name"]),
                            Datetime = Convert.ToDateTime(dr["Datetime"]),
                            Dinning_Type = Convert.ToString(dr["Dinning_Type"]),
                            NumberofGuest = Convert.ToInt32(dr["NumberofGuest"]),
                            Contact = Convert.ToString(dr["Contact"]),
                            Category = Convert.ToString(dr["Category"]),
                             CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),

                        });
                }
            }
            return bookinglist;
        }
        public bool UpdateDetails(Booking smodel)
        {
            connection();
            SqlCommand cmd = new SqlCommand("UpdateBookingDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", smodel.Id);
            cmd.Parameters.AddWithValue("@Customer_Name", smodel.Customer_Name);
            cmd.Parameters.AddWithValue("@Datetime", smodel.Datetime);
            cmd.Parameters.AddWithValue("@Category", smodel.Category);
            cmd.Parameters.AddWithValue("@Dinning_Type", smodel.Dinning_Type);
            
          cmd.Parameters.AddWithValue("@NumberofGuest", smodel.NumberofGuest);
            cmd.Parameters.AddWithValue("@Contact", smodel.Contact);
            cmd.Parameters.AddWithValue("@CreatedBy", smodel.CreatedBy);
            cmd.Parameters.AddWithValue("@CreatedDate", smodel.CreatedDate);
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

        // ********************** DELETE Booking DETAILS *******************
        public bool DeleteBooking(int id)
        {
            connection();
            SqlCommand cmd = new SqlCommand("DeleteBooking", con);
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

        public int GetCount(string searchValue)
        {
            int count = 0;
            connection();
            SqlCommand cmd = new SqlCommand("GetCount", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@searchValue", searchValue);
            SqlDataAdapter sd = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            con.Open();
            sd.Fill(dt);
            con.Close();
            if(dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                count = Convert.ToInt32(row["Column1"]);
            }
            return count;
        }

        public List<Booking> ListOfBooking()
        {
            connection();
            List<Booking> bookingList = new List<Booking>();

            SqlCommand cmd = new SqlCommand("GetBookingDetails", con);
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
                    bookingList.Add(
                        new Booking
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Customer_Name = Convert.ToString(dr["Customer_Name"]),
                            Datetime = Convert.ToDateTime(dr["Datetime"]),
                            Dinning_Type = Convert.ToString(dr["Dinning_Type"]),
                            NumberofGuest = Convert.ToInt32(dr["NumberofGuest"]),
                            Contact = Convert.ToString(dr["Contact"]),
                            Category = Convert.ToString(dr["Category"]),
                            CreatedBy = Convert.ToInt32(dr["CreatedBy"]),
                            CreatedDate = Convert.ToString(dr["CreatedDate"]),
                            ModifiedBy = Convert.ToInt32(dr["ModifiedBy"]),
                            ModifiedDate = Convert.ToString(dr["ModifiedDate"]),



                        });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return bookingList;
        }

    }


}
