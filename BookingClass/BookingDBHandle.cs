using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingClass
{
    public class BookingDBHandle
    {
        private SqlConnection con;
        private void connection()
        {
            //string constring = ConfigurationManager.ConnectionStrings["BookingConn"].ToString();
            string constring = "Data Source=DESKTOP-46PCH8R;Initial Catalog=BookingpageDB;Integrated Security=True;Pooling=False";
            con = new SqlConnection(constring);
        }

        // **************** ADD NEW Booking *********************
        public bool AddBooking(Bookingmodel smodel)
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

        public List<Bookingmodel> GetBooking(int pageIndex, int pageSize, string searchValue)
        {
            connection();
            List<Bookingmodel> bookingList = new List<Bookingmodel>();

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
                        new Bookingmodel
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Customer_Name = Convert.ToString(dr["Customer_Name"]),
                            Datetime = Convert.ToDateTime(dr["Datetime"]),
                            Dinning_Type = Convert.ToString(dr["Dinning_Type"]),
                            NumberofGuest = Convert.ToInt32(dr["NumberofGuest"])
                        });
                }
            }
                catch (Exception ex)
                {
                    throw;
                }
            return bookingList;
        }
        public List<Bookingmodel> GetBookings()
        {
            connection();
            List<Bookingmodel> Bookinglist = new List<Bookingmodel>();

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
                    new Bookingmodel
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
        public List<Bookingmodel> GetById(int id)
        {
            connection();
            List<Bookingmodel> bookinglist = new List<Bookingmodel>();
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
                        new Bookingmodel
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Customer_Name = Convert.ToString(dr["Customer_Name"]),
                            Datetime = Convert.ToDateTime(dr["Datetime"]),
                            Dinning_Type = Convert.ToString(dr["Dinning_Type"]),
                            NumberofGuest = Convert.ToInt32(dr["NumberofGuest"])
                        });
                }
            }
            return bookinglist;
        }
        public bool UpdateDetails(Bookingmodel smodel)
        {
            connection();
            SqlCommand cmd = new SqlCommand("UpdateBookingDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@Id", smodel.Id);
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
    }
}
