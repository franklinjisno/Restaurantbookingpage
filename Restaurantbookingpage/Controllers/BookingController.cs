using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookingClass.Enum;
using System.Linq.Dynamic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using ArrayToPdf;
using System.Data;
using Restaurantbookingpage.Models;
using System.Data.SqlClient;

namespace Restaurantbookingpage.Controllers
{
    public class BookingController : Controller
    {
        SqlConnection con = new SqlConnection("Data Source=ASUSX515;Initial Catalog=Bookingpage;Integrated Security=True");

        // View Bookings
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize]
        public ActionResult List()
        {
            return View();
        }


        [HttpPost]
        public ActionResult BookingData()
        {
            try
            {
                Bookingdbhandle objdbhandle = new Bookingdbhandle();
                var length = Convert.ToInt32(Request.Form["length"]);
                var start = Convert.ToInt32(Request.Form["start"]);
                var searchValue = Request.Form["search[value]"];
                string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][data]"];
                string sortDirection = Request["order[0][dir]"];
                List<Booking> objbooking = objdbhandle.GetBooking(start, length, searchValue);
                objbooking = objbooking.OrderBy(sortColumnName + " " + sortDirection).ToList<Booking>();
                int totalCount = objdbhandle.GetCount(searchValue);
                return Json(new { data = objbooking, recordsTotal = totalCount, recordsFiltered = totalCount }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }

      
        // Add new booking
        [HttpGet]
        public ActionResult Create(string operation)
        {
            Booking objbooking = new Booking();
            if (ModelState.IsValid)
            {
                if (operation == Actions.Create.ToString())
                {
                    objbooking.Actions = Actions.Create;
                }
                return PartialView("_OperationPartial", objbooking);
            }
            else
            {
                return View();
            }
        }

        // View Booking
        [HttpGet]
        public ActionResult View(string operation, int id)
        {
            Bookingdbhandle objdbhandle = new Bookingdbhandle();
            Booking objbooking = objdbhandle.GetById(id).Find(obj => obj.Id == id);
            TempData["Dinning Type"] = objbooking.Dinning_Type;
            TempData["Category"] = objbooking.Category;
            objbooking.Customer_Name = objbooking.Customer_Name.Trim();
            objbooking.Contact = objbooking.Contact.Trim();
            if (operation == Actions.View.ToString())
            {
                objbooking.Actions = Actions.View;
            }
            return PartialView("_OperationPartial", objbooking);
        }

        // Edit Booking
        [HttpGet]
        public ActionResult Edit(string operation, int id)
        {
            Bookingdbhandle objdbhandle = new Bookingdbhandle();
            Booking objbooking = objdbhandle.GetById(id).Find(obj => obj.Id == id);
            TempData["Dinning Type"] = objbooking.Dinning_Type;
            TempData["Category"] = objbooking.Category;
            objbooking.Customer_Name = objbooking.Customer_Name.Trim();
                objbooking.Contact = objbooking.Contact.Trim();
                if (operation == Actions.Edit.ToString())
                {
                    objbooking.Actions = Actions.Edit;
                }
                return PartialView("_OperationPartial", objbooking);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEdit(Booking objbooking)
        {
            Bookingdbhandle objdbhandle = new Bookingdbhandle();
            objbooking.Datetime = DateTime.Now;
            var AdUser = User.Identity.Name;
            var admin = GetUserByName(AdUser);
            if(admin.Id==0)
            {
                admin=objdbhandle.GetUsersByName(AdUser);
            }

            if (objbooking.Id > 0)
                {
                bool authenticated = Request.IsAuthenticated;

              objbooking.ModifiedByName=User.Identity.Name;
                objbooking.ModifiedBy = admin.Id;
                objbooking.ModifiedDate = DateTime.Now.ToString();
            
                
                    objdbhandle.UpdateDetails(objbooking);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                bool authenticated = Request.IsAuthenticated;
                objbooking.CreatedBy = admin.Id;
                objbooking.CreatedByName = User.Identity.Name;
                objbooking.CreatedDate= DateTime.Now.ToString();
                objdbhandle.UpdateDetails(objbooking);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
           
        }

        public Registration GetUserByName(string Email)
        {



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



        // Delete Booking
        public ActionResult DeletePartial(string operation, int id)
        {
            Bookingdbhandle objdbhandle = new Bookingdbhandle();
            Booking objbooking = objdbhandle.GetById(id).Find(obj => obj.Id == id);
            if (operation == Actions.Delete.ToString())
            {
                objbooking.Actions = Actions.Delete;
            }
            return PartialView("_DeletePartial", objbooking);
        }
        public ActionResult Delete(int id)
        {
            try
            {
                Bookingdbhandle objdbhandle = new Bookingdbhandle();
                if (objdbhandle.DeleteBooking(id))
                {
                    ViewBag.AlertMsg = "Booking Deleted Successfully";
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Print()
        {

            Bookingdbhandle objdbhandle = new Bookingdbhandle();
            List<Booking> objbooking = objdbhandle.ListOfBooking();

            var table = new DataTable("Booking Report");
            table.Columns.Add("Customer Name", typeof(string));
            table.Columns.Add("DateTime", typeof(DateTime));
            table.Columns.Add("DiningType", typeof(string));
            table.Columns.Add("NumberOfGuest", typeof(int));
            table.Columns.Add("Contact", typeof(Int64));
            table.Columns.Add("Category", typeof(string));

            foreach (Booking booking in objbooking)
                table.Rows.Add(booking.Customer_Name,booking.Datetime.Date, booking.Dinning_Type,booking.NumberofGuest,booking.Contact,booking.Category);

            var pdf = table.ToPdf();
            System.IO.File.WriteAllBytes(@"C:/Users/91623/Desktop/Demo/Restaurantbookingpage/Restaurantbookingpage/PDF/bookings.pdf", pdf);

            return PartialView("_PrintView");
            
        }
    }
}
