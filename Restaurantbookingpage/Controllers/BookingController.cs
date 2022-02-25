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

namespace Restaurantbookingpage.Controllers
{
    public class BookingController : Controller
    {
        // View Bookings
        public ActionResult Index()
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
            
                if (objbooking.Id > 0)
                {
                    objdbhandle.UpdateDetails(objbooking);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    objdbhandle.UpdateDetails(objbooking);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
           
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

            var table = new DataTable("Example Table");
            table.Columns.Add("Customer Name", typeof(string));
            table.Columns.Add("DateTime", typeof(DateTime));
            table.Columns.Add("DiningType", typeof(string));
            table.Columns.Add("NumberOfGuest", typeof(int));
            table.Columns.Add("Contact", typeof(Int64));
            table.Columns.Add("Category", typeof(string));

            foreach (Booking booking in objbooking)
                table.Rows.Add(booking.Customer_Name,booking.Datetime.Date, booking.Dinning_Type,booking.NumberofGuest,booking.Contact,booking.Category);

            var pdf = table.ToPdf();
            System.IO.File.WriteAllBytes(@"D:\Project-Franklin\Restaurantbookingpage\PDF\result", pdf);

            return PartialView("_PrintView");

            //Encoding u8 = Encoding.UTF8;
            //byte[] result = objbooking.SelectMany(x => u8.GetBytes(x.Customer_Name + "   " + x.Category + "\n")).ToArray();
            //byte[] data = Encoding.ASCII.GetBytes(objbooking.ToString()).ToArray();
            //byte[] data = new byte[100] ;
            //var pdf = objbooking.ToPdf(schema => schema.ColumnName(m => m. != "Prop2"));
            //byte[] pdf = objbooking.ToPdf();
           
            //foreach (Booking booking in objbooking) {
            //   //data.Add(u8.GetBytes(booking.Customer_Name));
            //   data[0] = u8.GetAllBytes(booking.Customer_Name);
            //               }
            // string s = u8.GetString(result);
            //System.IO.File.ReadAllBytes(@"Downloads")
            //System.IO.File.WriteAllBytes(@"F:\testpdf.pdf", result);
           


            //string s = u8.GetString(result);
            //byte[] bytes = objbooking.SelectMany(x => x).SelectMany(BitConverter.GetBytes).ToArray();
            //var binFormatter = new BinaryFormatter();
            //var mStream = new MemoryStream();
            //binFormatter.Serialize(mStream, objbooking);
            //mStream.ToArray();
            //return new FileStreamResult(mStream, "application/pdf");


            //var report = new ActionAsPdf(objdbhandle.ListOfBooking);
            //return PartialView("_PrintView",report);


            //byte[] bytes;
            //BinaryFormatter bf = new BinaryFormatter();
            //MemoryStream ms = new MemoryStream();
            //bf.Serialize(ms, objbooking);
            //bytes = ms.ToArray();
            //System.IO.File.WriteAllBytes(@"F:\testpdf.pdf", bytes);
            //return PartialView("_PartialView");
        }
    }
}
