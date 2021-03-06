using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Restaurantbookingpage.Models;
using BookingClass.Enum;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Text;
using ArrayToPdf;
using System.Net.Mail;
using System.Net;

namespace Restaurantbookingpage.Controllers
{
   
    public class AccountController : Controller
    {
        SqlConnection con = new SqlConnection("Data Source=ASUSX515;Initial Catalog=Bookingpage;Integrated Security=True");



        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

      
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }


      


        [HttpPost]
        public ActionResult ForgotPassword(ForgotPassword ForgotPasswordModel)
        {
            try
            {

                if (IsValidReset(ForgotPasswordModel.Email, ForgotPasswordModel.ContactNo))
                {
                  
                    return RedirectToAction("ResetPassword","Account");
                } 
                else if(IsValidResetAdmin(ForgotPasswordModel.Email, ForgotPasswordModel.ContactNo))
                    {
                    return RedirectToAction("ResetPasswordAdmin", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Your Email or ContactNo is incorrect");
                }
                return View(ForgotPasswordModel);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e);
            }
            return View();
        }

        [HttpGet]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordModel PasswordObj)
        {
            try
            {
                Accountdbhandle objdbhandle = new Accountdbhandle();
                if (objdbhandle.ResetPassword(PasswordObj.NewPassword, PasswordObj.Email))
                {
                    ViewBag.AlertMsg = "Account Deleted Successfully";
                    return RedirectToAction("Login", "Account");
                }

                else if (objdbhandle.ResetPasswordAdmin(PasswordObj.NewPassword, PasswordObj.Email))
                {
                    ViewBag.AlertMsg = "Success";
                    return RedirectToAction("Login", "Account");
                }
                else
                {

                    return HttpNotFound();
                }



            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult ResetPasswordAdmin()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ResetPasswordAdmin(ResetPasswordModel PasswordObj)
        {
            try
            {
                Accountdbhandle objdbhandle = new Accountdbhandle();
                if (objdbhandle.ResetPasswordAdmin(PasswordObj.NewPassword, PasswordObj.Email))
                {
                    ViewBag.AlertMsg = "Account Deleted Successfully";
                    return RedirectToAction("Login", "Account");
                }
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult List()
        {
            return View();
        }

        
        [Authorize]
        public ActionResult UserInfo()
        {
            return View();
        }


        [HttpPost]
        public ActionResult AccountData()
        {
            try
            {
                Accountdbhandle objdbhandle = new Accountdbhandle();
                var length = Convert.ToInt32(Request.Form["length"]);
                var start = Convert.ToInt32(Request.Form["start"]);
                var searchValue = Request.Form["search[value]"];
                string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][data]"];
                string sortDirection = Request["order[0][dir]"];
                List<Registration> objreg = objdbhandle.GetAccount(start, length, searchValue);
                objreg = objreg.OrderBy(sortColumnName + " " + sortDirection).ToList<Registration>();
                int totalCount = objdbhandle.GetCounts(searchValue);
                return Json(new { data = objreg, recordsTotal = totalCount, recordsFiltered = totalCount }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                string message = ex.Message;
                return View();
            }
        }

        
        [HttpGet]
        public ActionResult Create(string operation)
        {
            Registration objaccount = new Registration();
            if (ModelState.IsValid)
            {
                if (operation == Actions.Create.ToString())
                {
                    objaccount.Actions = Actions.Create;
                }
                return PartialView("_OperationPartial", objaccount);
            }
            else
            {
                return View();
            }
        }

       
        [HttpGet]
        public ActionResult View(string operation, int id)
        {
            Accountdbhandle objdbhandle = new Accountdbhandle();
            Registration objaccount = objdbhandle.GetById(id).Find(obj => obj.Id == id);
            objaccount.Name = objaccount.Name.Trim();
            objaccount.ContactNo = objaccount.ContactNo.Trim();
            if (operation == Actions.View.ToString())
            {
                objaccount.Actions = Actions.View;
            }
            return PartialView("_OperationPartial", objaccount);
        }

        
        [HttpGet]
        public ActionResult Edit(string operation, int id)
        {
            Accountdbhandle objdbhandle = new Accountdbhandle();
            Registration objaccount = objdbhandle.GetById(id).Find(obj => obj.Id == id);
            objaccount.Name = objaccount.Name.Trim();
            objaccount.ContactNo = objaccount.ContactNo.Trim();
            if (operation == Actions.Edit.ToString())
            {
                objaccount.Actions = Actions.Edit;
            }
            return PartialView("_OperationPartial", objaccount);
        }

        
       
        public ActionResult DeletePartial(string operation, int id)
        {
            Accountdbhandle objdbhandle = new Accountdbhandle();
            Registration objaccount = objdbhandle.GetById(id).Find(obj => obj.Id == id);
            if (operation == Actions.Delete.ToString())
            {
                objaccount.Actions = Actions.Delete;
            }
            return PartialView("_DeletePartial", objaccount);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                Accountdbhandle objdbhandle = new Accountdbhandle();
                if (objdbhandle.DeleteAccount(id))
                {
                    ViewBag.AlertMsg = "Account Deleted Successfully";
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

            Accountdbhandle objdbhandle = new Accountdbhandle();
            List<Registration> objaccount = objdbhandle.ListOfAccount();

            var table = new DataTable("Account Report");
            table.Columns.Add("Name", typeof(string));
            table.Columns.Add("Email", typeof(string));
            table.Columns.Add("Password", typeof(string));
            table.Columns.Add("ContactNo", typeof(string));
            table.Columns.Add("CreatedBy", typeof(string));
            table.Columns.Add("CreatedDate", typeof(string));
            table.Columns.Add("ModifiedBy", typeof(string));
            table.Columns.Add("ModifiedDate", typeof(string));


            foreach (Registration account in objaccount)
                table.Rows.Add(account.Name, account.Email, account.Password, account.ContactNo, account.CreatedBy,account.CreatedDate,account.ModifiedBy,account.ModifiedDate);

            var pdf = table.ToPdf();
            System.IO.File.WriteAllBytes(@"C:/Users/91623/Desktop/Demo/Restaurantbookingpage/Restaurantbookingpage/PDF/user.pdf", pdf);

            return PartialView("_PrintView");

        }
  
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("List", "Account");
            }
            ViewBag.ReturnUrl = returnUrl ?? Url.Action("List", "Account");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login LoginViewModel)
        {
            try
            {

                if (IsValidUser(LoginViewModel.Email, LoginViewModel.Password))
            {
                FormsAuthentication.SetAuthCookie(LoginViewModel.Email, false);
                return RedirectToAction("Index", "Booking");
            }
            else if (IsValidAdmin(LoginViewModel.Email, LoginViewModel.Password))
            {
                FormsAuthentication.SetAuthCookie(LoginViewModel.Email, false);
                return RedirectToAction("List", "Account");
            }
            else
            {
                ModelState.AddModelError("", "Your Email or password is incorrect");
            }
            return View(LoginViewModel);
        }
            catch (Exception e)
            {
                ModelState.AddModelError("", e);
            }
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Registration RegistrationViewModel)
        {

            bool authenticated = Request.IsAuthenticated;
            Accountdbhandle db=new Accountdbhandle();
            var AdUser = User.Identity.Name;
          Registration user=db.GetUserByName(AdUser);

            RegistrationViewModel.CreatedBy = user.Id;
            RegistrationViewModel.ModifiedBy = 0;  
            RegistrationViewModel.CreatedDate= DateTime.Now.ToString();
            RegistrationViewModel.ModifiedDate = "";

            try
            {
                if (ModelState.IsValid)
                {

                    if (!IsUserExist(RegistrationViewModel.Email))

                    {
                        if (!RegistrationViewModel.Isadmin)
                        {


                            string query = "insert into TblUsers values (@Name,@Email,@Password,@ContactNo,@CreatedBy,@CreatedDate,@ModifiedBy,@ModifiedDate,@Deleted)";

                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.Connection = con;
                                cmd.Parameters.AddWithValue("@Name", RegistrationViewModel.Name);
                                cmd.Parameters.AddWithValue("@Email", RegistrationViewModel.Email);
                                cmd.Parameters.AddWithValue("@Password", Base64Encode(RegistrationViewModel.Password));
                                cmd.Parameters.AddWithValue("@ContactNo", RegistrationViewModel.ContactNo);
                                cmd.Parameters.AddWithValue("@CreatedBy", RegistrationViewModel.CreatedBy);
                                cmd.Parameters.AddWithValue("@CreatedDate", RegistrationViewModel.CreatedDate);
                                cmd.Parameters.AddWithValue("@ModifiedBy", RegistrationViewModel.ModifiedBy);
                                cmd.Parameters.AddWithValue("@ModifiedDate", RegistrationViewModel.ModifiedDate);
                                cmd.Parameters.AddWithValue("@Deleted", RegistrationViewModel.Deleted);
                                con.Open();
                                int i = cmd.ExecuteNonQuery();
                                con.Close();
                                if (i > 0)
                                {
                                    return RedirectToAction("Index", "Account");
                                }
                                else
                                {
                                    ModelState.AddModelError("", "something went wrong try later!");
                                }
                            }
                        }
                        else if (RegistrationViewModel.Isadmin)
                        {
                            string query = "insert into TblAdmin values (@Name,@Email,@Password,@ContactNo)";

                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                cmd.Connection = con;
                                cmd.Parameters.AddWithValue("@Name", RegistrationViewModel.Name);
                                cmd.Parameters.AddWithValue("@Email", RegistrationViewModel.Email);
                                cmd.Parameters.AddWithValue("@Password", Base64Encode(RegistrationViewModel.Password));
                                cmd.Parameters.AddWithValue("@ContactNo", RegistrationViewModel.ContactNo);
                               
                              
                                con.Open();
                                int i = cmd.ExecuteNonQuery();
                                con.Close();
                                if (i > 0)
                                {
                                    return RedirectToAction("Index", "Account");
                                }
                                else
                                {
                                    ModelState.AddModelError("", "something went wrong try later!");
                                }
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "User with same email already exist!");
                    }

                }
               
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return View();
        }


        


        public ActionResult EditUser(Registration RegistrationViewModel)
        {

            bool authenticated = Request.IsAuthenticated;
            Accountdbhandle db = new Accountdbhandle();
            var AdUser = User.Identity.Name;
            Registration user = db.GetUserByName(AdUser);

            //RegistrationViewModel.CreatedBy = AdUser;
            RegistrationViewModel.ModifiedBy = user.Id;
            //RegistrationViewModel.CreatedDate = DateTime.Now.ToString();
            RegistrationViewModel.ModifiedDate = DateTime.Now.ToString();

            try
            {
                if (ModelState.IsValid)
                {

                        string query = "Update TblUsers set Name=@Name,Email=@Email,Password=@Password,ContactNo=@ContactNo,ModifiedBy=@ModifiedBy,ModifiedDate=@ModifiedDate,Deleted=@Deleted where Id=@Id";
                        
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@Id", RegistrationViewModel.Id);
                        cmd.Parameters.AddWithValue("@Name", RegistrationViewModel.Name);
                            cmd.Parameters.AddWithValue("@Email", RegistrationViewModel.Email);
                            cmd.Parameters.AddWithValue("@Password", Base64Encode(RegistrationViewModel.Password));
                            cmd.Parameters.AddWithValue("@ContactNo", RegistrationViewModel.ContactNo);
                            cmd.Parameters.AddWithValue("@ModifiedBy", RegistrationViewModel.ModifiedBy);
                            cmd.Parameters.AddWithValue("@ModifiedDate", RegistrationViewModel.ModifiedDate);
                            cmd.Parameters.AddWithValue("@Deleted", RegistrationViewModel.Deleted);

                        con.Open();
                            int i = cmd.ExecuteNonQuery();
                            con.Close();
                            if (i > 0)
                            {
                                //FormsAuthentication.SetAuthCookie(RegistrationViewModel.Email, false);
                                return RedirectToAction("Index", "Account");
                            }
                        else
                        {
                            ModelState.AddModelError("", "something went wrong try later!");
                        }
                        }



                }
               
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return View();
        }


        public ActionResult LogOut()
        {
           
         
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        private bool IsUserExist(string email)
        {
            bool IsUserExist = false;
            string query = "select * from TblUsers where Email=@email";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@email", email);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    IsUserExist = true;
                }
            }
            return IsUserExist;
        }
        private bool IsValidUser(string email, string password)
        {
            var encryptpassowrd = Base64Encode(password);
            bool IsValid = false;
            string query = "select * from TblUsers where Email=@email and Password=@Password";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password",encryptpassowrd);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    IsValid = true;
                }
            }
            return IsValid;
        }
        private bool IsValidReset(string email, string contactno)
        {
            
            bool IsValid = false;
            string query = "select * from TblUsers where Email=@email and ContactNo=@contactno and Deleted=0";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@ContactNo",contactno);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    IsValid = true;
                }
            }
            return IsValid;
        }

        private bool IsValidResetAdmin(string email, string contactno)
        {

            bool IsValid = false;
            string query = "select * from TblAdmin where Email=@email and ContactNo=@contactno";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@ContactNo", contactno);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    IsValid = true;
                }
            }
            return IsValid;
        }



        private bool IsValidAdmin(string email, string password)
        {
            //var encryptpassowrd = Base64Encode(password);
            bool IsValid = false;
            string query = "select * from TblAdmin where Email=@email and Password=@Password";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password",password);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                con.Open();
                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (dt.Rows.Count > 0)
                {
                    IsValid = true;
                }
            }
            return IsValid;
        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}