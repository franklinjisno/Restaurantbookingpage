using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookingClass;

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
                BookingDBHandle dbhandle = new BookingDBHandle();
                var length = Convert.ToInt32(Request.Form["length"]);
                var start = Convert.ToInt32(Request.Form["start"]);
                var searchValue = Request.Form["search[value]"];
                List<Bookingmodel> bookings = dbhandle.GetBooking(start, length, searchValue);
                return Json(new { data = bookings }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
        }
        
        // Add new booking
        [HttpGet]
        public ActionResult Create()
        {
            Bookingmodel model = new Bookingmodel();
            return PartialView("_Createpartial", model);
        }

        // Edit Booking
        public ActionResult Edit(int id)
        {
            BookingDBHandle db = new BookingDBHandle();
            var model = db.GetById(id).Find(smodel => smodel.Id == id);
            TempData["Example"] = model.Dinning_Type;
            return PartialView("_Createpartial", model);
        }

        [HttpPost]
        public ActionResult CreateEdit(Bookingmodel smodel)
        {
            BookingDBHandle sdb = new BookingDBHandle();
                if(smodel.Id >0)
                {
                    sdb.UpdateDetails(smodel);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    sdb.UpdateDetails(smodel);
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
        }

        // Delete Booking
        public ActionResult DeletePartial(int id)
        {
            BookingDBHandle db = new BookingDBHandle();
            var model = db.GetById(id).Find(smodel => smodel.Id == id);
            return PartialView("_DeletePartial", model);
        }
        public ActionResult Delete(int id)
        {
            try
            {
                BookingDBHandle db = new BookingDBHandle();
                if (db.DeleteBooking(id))
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
    }
}
