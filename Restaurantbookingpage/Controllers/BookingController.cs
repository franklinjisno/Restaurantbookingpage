using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookingClass.Enum;
using System.Linq.Dynamic;

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
                return Json(new { data = objbooking }, JsonRequestBehavior.AllowGet);
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
            if (ModelState.IsValid)
            {
                Booking objbooking = objdbhandle.GetById(id).Find(obj => obj.Id == id);
                objbooking.Customer_Name = objbooking.Customer_Name.Trim();
                objbooking.Contact = objbooking.Contact.Trim();
                TempData["Dinning Type"] = objbooking.Dinning_Type;
                TempData["Category"] = objbooking.Category;
                if (operation == Actions.Edit.ToString())
                {
                    objbooking.Actions = Actions.Edit;
                }
                return PartialView("_OperationPartial", objbooking);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateEdit(Booking objbooking)
        {
            Bookingdbhandle objdbhandle = new Bookingdbhandle();
            if (ModelState.IsValid)
            {
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
            return Json(false, JsonRequestBehavior.AllowGet);
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
    }
}
