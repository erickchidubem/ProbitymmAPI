using ProbitymmAPI.Data;
using ProbitymmAPI.Models;
using ProbitymmAPI.Security;
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
        ReturnValues rv = new ReturnValues();
        Authentication at = new Authentication();

        //GET API  : GetBusinessInfo
        [AuthorizeUserAttribute]
        [HttpGet]
        public IHttpActionResult GetBusinessInfo([FromUri]int businessid)
        {
            return Ok();
        }
        
        //POST API : Login Registered Users
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
                    UserData ud = at.Login(loginData);
                    if (ud != null)
                    {   
                        if(ud.UserID > 0)
                        {
                            if(ud.active == 0)
                            {
                                rv.StatusCode = 4; rv.StatusMessage = "Account has been deactivated";                               
                            }
                            else
                            {
                                rv.StatusCode = 1; rv.StatusMessage = "Successful Login";
                                ReturnedData = ud;
                            }
                           
                        }
                        else
                        {
                            rv.StatusCode = 2; rv.StatusMessage = "Invalid username or password";                          
                        }
                        result = cuc.GetJsonObject(ReturnedData, rv);            
                        return Ok(result);                   
                    }
                    else
                    {
                        rv.StatusCode = 3; rv.StatusMessage = "Invalid Login";                                            
                    }

                    result = cuc.GetJsonObject(ReturnedData, rv);
                    return Ok(result);
                }
                else
                {
                    return Content(HttpStatusCode.Unauthorized, cuc.GetJsonObject(ReturnedData, cuc.Error(1)));
                }
            }
            else
            {
                return Content(HttpStatusCode.Forbidden, cuc.GetJsonObject(ReturnedData, cuc.Error(2)));
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
                    rv = at.RegisterBusiness(bizRegModel);                    
                    result = cuc.GetJsonObject(ReturnedData, rv);
                    return Ok(result);
                }
                else
                {
                    return Content(HttpStatusCode.Unauthorized, cuc.GetJsonObject(ReturnedData, cuc.Error(1)));
                }
            }
            else
            {
                return Content(HttpStatusCode.Forbidden, cuc.GetJsonObject(ReturnedData, cuc.Error(2)));
            }
        }

        //POST API :
        [AuthorizeUserAttribute]
        [HttpPost]
        public IHttpActionResult ChangePassword([FromBody]ChangePassword cp)
        {
            var result = (Object)null;
            var ReturnedData = (Object)null;
  
            rv = at.ChangePassword(cp);
            result = cuc.GetJsonObject(ReturnedData, rv);
            return Ok(result);
              
        }

        //POST API : Register Business Staff
        [AuthorizeUserAttribute]
        [HttpPost]
        public IHttpActionResult RegisterEditStaff([FromBody]UserData userData)
        {
            var result = (Object)null;
            var ReturnedData = (Object)null;
            
            if (userData.businessid > 0)
            {
                rv = at.RegisterBusinessStaff(userData);
                result = cuc.GetJsonObject(ReturnedData, rv);                       
            }
            else
            {
                rv.StatusCode = 6;rv.StatusMessage = "you did not supply businessid";
                result = cuc.GetJsonObject(ReturnedData,rv);
            }
            return Ok(result);
                
        }

        //GET API : Get a user information
        [AuthorizeUserAttribute]
        [HttpGet]
        public IHttpActionResult UserInfo([FromUri]int userid)
        {
            var result = (Object)null;
            var ReturnedData = (Object)null;

           if (userid > 0)
            {
                UserData ud = new UserData();
                ud = at.UserInformation(userid);
                rv.StatusCode = 1; rv.StatusMessage = "User Information";
                result = cuc.GetJsonObject(ud, rv);
            }
            else
            {
                rv.StatusCode = 3; rv.StatusMessage = "you did not supply userid";
                result = cuc.GetJsonObject(ReturnedData, rv);
            }
            return Ok(result);
        }

        //GET API : Get all staff in a business
        [AuthorizeUserAttribute]
        [HttpGet]
        public IHttpActionResult AllBusinessStaff([FromUri]int BusinessId)
        {
            var result = (Object)null;
            var ReturnedData = (Object)null;
            
            Authentication at = new Authentication();
            rv.StatusCode = 1;rv.StatusMessage = "All staff";
            result = cuc.GetJsonObject(at.AllBusinessStaffList(BusinessId),rv);
            return Ok(result);
        }

        //POST API : Modify business information
        [AuthorizeUserAttribute]
        [HttpPost]
        public IHttpActionResult ModifyBusinessInformation([FromBody]BizRegModel brm)
        {
            return Ok();
        }

        [AuthorizeUserAttribute]
        [HttpPost]
        public IHttpActionResult CheckLoginCredentials([FromBody]UserData ud)
        {

            var result = (Object)null;
            var ReturnedData = (Object)null;           
            int ifix = at.CheckLoginCredentials(ud);
                    
                if (ifix == 0)
                {
                    rv.StatusCode = ifix; rv.StatusMessage = "Incorrect";
                }
                else if (ifix == 1)
                {
                    rv.StatusCode = ifix; rv.StatusMessage = "Correct";
                }
                result = cuc.GetJsonObject(ReturnedData, rv);
                return Ok(result);
        }

    }
}
