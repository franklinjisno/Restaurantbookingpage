using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BookingClass;

namespace Bookingwebapi.Controllers
{
    public class BookingwebapiController : ApiController
    {
        // GET: api/Bookingwebapi
        [HttpGet]
        public List<Bookingmodel> Get(int pageIndex, int pageSize, string searchValue)
        {
            try
            {
                BookingDBHandle dbhandle = new BookingDBHandle();
                List<Bookingmodel> bookingList = dbhandle.GetBooking(pageIndex, pageSize, searchValue);
                return bookingList;
            }
            catch
            {
                throw;
            }
        }
        // POST: api/Bookingwebapi
        [HttpPost]
        public bool Create(Bookingmodel smodel)
        {
            try
            {
                BookingDBHandle sdb = new BookingDBHandle();
                 var message = sdb.AddBooking(smodel);
                
                return message;
            }
            catch
            {
                throw;
            }
        }

        // PUT: api/Bookingwebapi/5
        [HttpPut]
        public List<Bookingmodel> GetById(int id)
        {
            try
            {
                BookingDBHandle db = new BookingDBHandle();
                List<Bookingmodel> bookingmodel = db.GetById(id);
                return bookingmodel;
            }
            catch
            {
                throw;
            }
        }
        [HttpPut]
        public bool Edit( Bookingmodel smodel)
        {
            try
            {
                BookingDBHandle sdb = new BookingDBHandle();
                var message = sdb.UpdateDetails(smodel);
                return message;
            }
            catch
            {
                throw;
            }
        }

        // DELETE: api/Bookingwebapi/5
        [HttpDelete]
        public bool Delete(int id)
        {
            try
            {
                BookingDBHandle sdb = new BookingDBHandle();
                var status = sdb.DeleteBooking(id);
                return status;
            }
            catch
            {
                throw;
            }
        }
    }
}
