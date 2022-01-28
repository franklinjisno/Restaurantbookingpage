using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookingClass.Enum;

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
                BookingDataBase dbhandle = new BookingDataBase();
                var length = Convert.ToInt32(Request.Form["length"]);
                var start = Convert.ToInt32(Request.Form["start"]);
                var searchValue = Request.Form["search[value]"];
                List<Booking> bookings = dbhandle.GetBooking(start, length, searchValue);
                return Json(new { data = bookings }, JsonRequestBehavior.AllowGet);
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
            Booking model = new Booking();
            if (operation == Actions.Create.ToString())
            {
                model.Actions = Actions.Create;
            }
            return PartialView("_OperationPartial", model);
        }

        // Edit Booking
        public ActionResult View(string operation, int id)
        {
            BookingDataBase db = new BookingDataBase();
            Booking model = db.GetById(id).Find(smodel => smodel.Id == id);
            if(operation == Actions.Edit.ToString())
            {
                model.Actions = Actions.Edit;
            }
            return PartialView("_OperationPartial", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEdit(Booking smodel)
        {
            BookingDataBase sdb = new BookingDataBase();
            if (ModelState.IsValid)
            {
                if (smodel.Id > 0)
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
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        // Delete Booking
        public ActionResult DeletePartial(string operation, int id)
        {
            BookingDataBase db = new BookingDataBase();
            Booking model = db.GetById(id).Find(smodel => smodel.Id == id);
            if (operation == Actions.Delete.ToString())
            {
                model.Actions = Actions.Delete;
            }
            return PartialView("_DeletePartial", model);
        }
        public ActionResult Delete(int id)
        {
            try
            {
                BookingDataBase db = new BookingDataBase();
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
