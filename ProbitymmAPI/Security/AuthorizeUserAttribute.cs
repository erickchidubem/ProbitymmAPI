using ProbitymmAPI.Data;
using ProbitymmAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace ProbitymmAPI.Security
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {

        static string apikey;
        static int userId;
        static int businessId;
        HttpResponseMessage Hrm = new HttpResponseMessage();
        CryptoEngine ce = new CryptoEngine();
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            
            if (actionContext.Request.Headers.Contains("API-KEY") && actionContext.Request.Headers.Contains("BUSINESSID") && actionContext.Request.Headers.Contains("USERID"))
            {
                 apikey = actionContext.Request.Headers.GetValues("API-KEY").FirstOrDefault();
                 userId = Convert.ToInt32(actionContext.Request.Headers.GetValues("USERID").FirstOrDefault());
                 businessId = Convert.ToInt32(actionContext.Request.Headers.GetValues("BUSINESSID").FirstOrDefault());

                ReturnValuesBool rvb = Authentication.ValidateToken(apikey, userId, businessId);
                if (!rvb.StatusFlag)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, new { StatusMessage = rvb.StatusMessage, StatusCode = 100+rvb.StatusCode });
                }
            }
            else
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new { StatusMessage = "Incorrect Header specifications", StatusCode = 1000 });
            }
        }
    }
}