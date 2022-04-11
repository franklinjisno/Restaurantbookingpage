using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using Restaurantbookingpage.Models;
using Newtonsoft.Json;
using Restaurantbookingpage;

namespace RestaurantWebAPI.Controllers
{

    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {

        SqlConnection con = new SqlConnection("Data Source=ASUSX515;Initial Catalog=Bookingpage;Integrated Security=True");


        [HttpGet]
        [Route("AccountData")]
        public string AccountData(int start, int length, string searchValue)
        {
            try
            {
                Accountdbhandle objdbhandle = new Accountdbhandle();

                List<Registration> objreg = objdbhandle.GetAccount(start, length, searchValue);
                int totalCount = objdbhandle.GetCounts(searchValue);
                return JsonConvert.SerializeObject(objreg);
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return message;
            }
        }


        // View Registration
        [HttpGet]
        [Route("GetById")]
        public string GetById(int id)
        {
            Accountdbhandle objdbhandle = new Accountdbhandle();
            Registration objaccount = objdbhandle.GetById(id).Find(obj => obj.Id == id);
            objaccount.Name = objaccount.Name.Trim();
            objaccount.ContactNo = objaccount.ContactNo.Trim();
            return JsonConvert.SerializeObject(objaccount);
        }


      [HttpPut]
      [Route("EditUser")]
        public string EditUser(Registration RegistrationViewModel)
        {

          
            Accountdbhandle db = new Accountdbhandle();
           

            //RegistrationViewModel.CreatedBy = AdUser;
         
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
                            return "Successfull";
                        }
                        else
                        {
                            return "Something went wrong";
                        }



                    }

                }
                else
                {
                    return "Model Error";
                }
            }
            catch (Exception e)
            {
               return e.Message;
            }
           
        }




        [HttpDelete]
        [Route("Delete")]
        public string Delete(int id)
        {

            try
            {
                Accountdbhandle objdbhandle = new Accountdbhandle();
                if (objdbhandle.DeleteAccount(id))
                {
                    string AlertMsg = "Account Deleted Successfully";
                    return AlertMsg;
                }
                return "Success";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }





        [HttpPost]
        [Route("Register")]

        public string Register(Registration RegistrationViewModel)
        {

            var AdUser = User.Identity.Name;
            
            RegistrationViewModel.CreatedDate = DateTime.Now.ToString();
           
            RegistrationViewModel.ModifiedDate = DateTime.Now.ToString();
            try

            {
                if (ModelState.IsValid)
                {

                    if (!IsUserExist(RegistrationViewModel.Email))
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
                                //FormsAuthentication.SetAuthCookie(RegistrationViewModel.Email, false);
                                return "Success";
                            }
                            else
                            {
                                return "Error";
                            }
                        }
                    }
                    else
                    {
                        return "User with same email already exist!";
                    }

                }

            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return "Success";
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