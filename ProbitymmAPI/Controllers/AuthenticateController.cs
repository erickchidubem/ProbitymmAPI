using ProbitymmAPI.Data;
using ProbitymmAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace ProbitymmAPI.Controllers
{
    public class AuthenticateController : ApiController
    {
        CommonUtilityClass cuc = new CommonUtilityClass();
 
        // GET: api/Test
         [HttpGet]
        public IHttpActionResult New()
        {
            string[] n = { "value1", "value2" };
            //return NotFound();

            return Content(HttpStatusCode.NotFound, "Not Really found");
        }

        [HttpGet]
        public IHttpActionResult GetBusinessInfo([FromUri]int businessid)
        {

            return Ok();
        }

        //Post API : Login Registered Users
        [HttpPost]
        public IHttpActionResult Login([FromBody]LoginData loginData)
        {
            var result = (Object)null;
            var ReturnedData = (Object)null;
            if (Request.Headers.Contains("API-KEY"))
            {
                string apikey = Request.Headers.GetValues("API-KEY").First();
                if(apikey == CommonUtilityClass.apikey)
                {
                    Authentication at = new Authentication();
                    UserData ud = at.Login(loginData);
                    if (ud != null)
                    {   
                        if(ud.UserID > 0)
                        {
                            result = cuc.GetJsonObject(ud, 1, "Successful Login");
                        }
                        else
                        {
                            result = cuc.GetJsonObject(ReturnedData,2, "Invalid username or password");
                        }
                       
                        return Ok(result);                   
                    }
                    else
                    {
                        result = cuc.GetJsonObject(new{ }, 3,"Invalid Login");
                        return Content(HttpStatusCode.NotFound, result);
                    }
                }
                else
                {
                    return Content(HttpStatusCode.Unauthorized, cuc.GetJsonObject(ReturnedData, 100, "Wrong API Key"));
                }                
            }
            else
            {
                return Content(HttpStatusCode.Forbidden, cuc.GetJsonObject(ReturnedData, 1000, "Incorrect header specification"));
            }
        }


        //POST API : Register Business Account
        [HttpPost]
        public IHttpActionResult Register([FromBody]BizRegModel bizRegModel)
        {           
            var result = (Object)null;
            var ReturnedData = (Object)null;
            if (Request.Headers.Contains("API-KEY"))
            {
                string apikey = Request.Headers.GetValues("API-KEY").First();
                if (apikey == CommonUtilityClass.apikey)
                {
                    Authentication at = new Authentication();
                    int i = at.RegisterBusiness(bizRegModel);
                   
                    if (i == 3)
                    {
                      result =  cuc.GetJsonObject(ReturnedData, i, "Business Successfully registered");
                      return Ok(result);
                    }
                    else if(i == 2)
                    {
                        result = cuc.GetJsonObject(ReturnedData, i, "Email address already exist");
                        return Ok(result);  
                    }
                    else if (i == 1)
                    {
                        result = cuc.GetJsonObject(ReturnedData, i, "Business name already exist on this platform");
                        return Ok(result);
                    }
                    return Ok();
                }
                else
                {
                    return Content(HttpStatusCode.Unauthorized, cuc.GetJsonObject(ReturnedData,100, "Wrong API Key"));
                }
            }
            else
            {
                return Content(HttpStatusCode.Forbidden, cuc.GetJsonObject(ReturnedData, 1000, "Incorrect header specification"));
            }
        }

        [HttpPost]
        public IHttpActionResult ChangePassword([FromBody]ChangePassword cp)
        {
            var result = (Object)null;
            var ReturnedData = (Object)null;
            if (Request.Headers.Contains("API-KEY"))
            {
                string apikey = Request.Headers.GetValues("API-KEY").First();
                if (apikey == CommonUtilityClass.apikey)
                {
                    Authentication at = new Authentication();
                    int i = at.ChangePassword(cp);

                    if (i == 3)
                    {
                        result = cuc.GetJsonObject(ReturnedData, i, "No such user exists");
                        return Ok(result);
                    }
                    else if (i == 2)
                    {
                        result = cuc.GetJsonObject(ReturnedData, i, "Old Password did not match");
                        return Ok(result);
                    }
                    else if (i == 1)
                    {
                        result = cuc.GetJsonObject(ReturnedData, i, "password successfully updated");
                        return Ok(result);
                    }
                    return Ok();
                }
                else
                {
                    return Content(HttpStatusCode.Unauthorized, cuc.GetJsonObject(ReturnedData, 100, "Wrong API Key"));
                }
            }
            else
            {
                return Content(HttpStatusCode.Forbidden, cuc.GetJsonObject(ReturnedData, 1000, "Incorrect header specification"));
            }
        }

        //POST API : Register Business Staff
        [HttpPost]
        public IHttpActionResult RegisterStaff([FromBody]UserData userData)
        {
            var result = (Object)null;
            var ReturnedData = (Object)null;

            if (Request.Headers.Contains("API-KEY"))
            {
                string apikey = Request.Headers.GetValues("API-KEY").First();
                if (apikey == CommonUtilityClass.apikey)
                {
                    Authentication at = new Authentication();
                    int i = at.RegisterBusinessStaff(userData);

                    if (i == 4)
                    {
                        result = cuc.GetJsonObject(ReturnedData, i, "Staff information successfully updated");
                        return Ok(result);
                    }
                    else if (i == 3){
                        result = cuc.GetJsonObject(ReturnedData, i, "Staff registration is successful");
                        return Ok(result);                      
                    }
                    else if (i == 2)
                    {
                        result = cuc.GetJsonObject(ReturnedData, i, "Phone number already exists");
                        return Ok(result);
                    }
                    else if (i == 1)
                    {
                        result = cuc.GetJsonObject(ReturnedData, i, "email address already exists");
                        return Ok(result);
                    }
                    return Ok();
                }
                else
                {
                    return Content(HttpStatusCode.Unauthorized, cuc.GetJsonObject(ReturnedData, 100, "Wrong API Key"));
                }
            }
            else
            {
                return Content(HttpStatusCode.Forbidden, cuc.GetJsonObject(ReturnedData, 1000, "Incorrect header specification"));
            }
        }
    }
}
