using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Restaurantbookingpage.Models;

namespace Restaurantbookingpage.Controllers
{
    public class AccountController : Controller
    {
        SqlConnection con = new SqlConnection("Data Source=ASUSX515;Initial Catalog=BookingpageDB;Integrated Security=True");

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
       
        [HttpGet]
        
        public ActionResult Login()
        {
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
                return RedirectToAction("Index", "Account");
            }
            else
            {
                ModelState.AddModelError("", "Your Email or password is incorrect");
            }
            return View(LoginViewModel);
        }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Registration RegistrationViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                  
                    if (!IsUserExist(RegistrationViewModel.Email))
                    {
                        string query = "insert into TblAdmins values (@Name,@Email,@Password,@ContactNo)";
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
                                FormsAuthentication.SetAuthCookie(RegistrationViewModel.Email, false);
                                return RedirectToAction("Index", "Account");
                            }
                            else
                            {
                                ModelState.AddModelError("", "something went wrong try later!");
                            }
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "User with same email already exist!");
                    }

                }
                else
                {
                    ModelState.AddModelError("", "Data is not correct");
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
            return RedirectToAction("login", "Account");
        }

        private bool IsUserExist(string email)
        {
            bool IsUserExist = false;
            string query = "select * from TblUser where Email=@email";
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
            string query = "select * from TblAdmins where Email=@email and Password=@Password";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", encryptpassowrd);
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
            string query = "select * from TblUser where Email=@email and Password=@Password";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);
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