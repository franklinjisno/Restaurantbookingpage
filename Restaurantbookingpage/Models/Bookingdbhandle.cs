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
            //string constring = ConfigurationManager.ConnectionStrings["BookingConn"].ToString();
            string constring = "Data Source=DESKTOP-46PCH8R;Initial Catalog=BookingpageDB;Integrated Security=True;Pooling=False";
            con = new SqlConnection(constring);
        }

        // **************** ADD NEW Booking *********************
        public bool AddBooking(Booking smodel)
        {
            connection();
            SqlCommand cmd = new SqlCommand("AddNewBooking", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Customer_Name", smodel.Customer_Name);
            cmd.Parameters.AddWithValue("@Datetime", smodel.Datetime);
            cmd.Parameters.AddWithValue("@Dinning_Type", smodel.Dinning_Type);
            cmd.Parameters.AddWithValue("@NumberofGuest", smodel.NumberofGuest);

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
                            Category = Convert.ToString(dr["Category"])
                        });
                }
            }
                catch (Exception ex)
                {
                    throw;
                }
            return bookingList;
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
                        Dinning_Type = Convert.ToString(dr["Dinning_Type"]),
                        NumberofGuest = Convert.ToInt32(dr["NumberofGuest"])
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
                            Category = Convert.ToString(dr["Category"])
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
            cmd.Parameters.AddWithValue("@Dinning_Type", smodel.Dinning_Type);
            cmd.Parameters.AddWithValue("@NumberofGuest", smodel.NumberofGuest);
            cmd.Parameters.AddWithValue("@Contact", smodel.Contact);
            cmd.Parameters.AddWithValue("@Category", smodel.Category);

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
    }
}
