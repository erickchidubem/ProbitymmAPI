using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProbitymmAPI.Controllers
{
    public class BaseController : ApiController
    {
        public bool VerifyUserBusinessID(int _businessid, int _userid)
        {
            bool value = false;
            int userId = Convert.ToInt32(Request.Headers.GetValues("USERID").FirstOrDefault());
            int businessid = Convert.ToInt32(Request.Headers.GetValues("BUSINESSID").FirstOrDefault());
            if (userId == _userid && businessid == _businessid)
            {
                value = true;
            }

            return value;
        }
    }
}
