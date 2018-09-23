using ProbitymmAPI.Data;
using ProbitymmAPI.Models;
using ProbitymmAPI.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProbitymmAPI.Controllers
{
    [AuthorizeUserAttribute]
    public class SalesController : ApiController
    {
        CommonUtilityClass cuc = new CommonUtilityClass();
        ReturnValues rv = new ReturnValues();
        Sales sl = new Sales();

        [HttpPost]
        public IHttpActionResult CreateCustomers([FromBody]CustomerInfo ci)
        {
            return Ok();
        }

        [HttpPost]
        public IHttpActionResult CreateEditShops([FromBody]ShopModel sm)
        {

            var result = (Object)null;
            var ReturnedData = (Object)null;
            sm.BusinessId = Convert.ToInt32(Request.Headers.GetValues("BUSINESSID").FirstOrDefault());
            sm.UserId = Convert.ToInt32(Request.Headers.GetValues("USERID").FirstOrDefault());

            if (sm.StoreName != "")
            {
                rv = sl.CreateEditStore(sm);
                result = cuc.GetJsonObject(ReturnedData, rv);
            }
            else
            {
                rv.StatusCode = 10; rv.StatusMessage = "you did not supply a vital identity";
                result = cuc.GetJsonObject(ReturnedData, rv);
            }

            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetStoreList()
        {
            //GetStoreLists
            var result = (Object)null;
            var ReturnedData = (Object)null;
            int BusinessId = Convert.ToInt32(Request.Headers.GetValues("BUSINESSID").FirstOrDefault());

            rv.StatusCode = 1; rv.StatusMessage = "your shop lists";
            ReturnedData = sl.GetStoreLists(BusinessId);
            result = cuc.GetJsonObject(ReturnedData, rv);
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult ShopKeeperAcceptsFinishProductToShop([FromBody] AdminApprove adap)
        {
            var result = (Object)null;
            var ReturnedData = (Object)null;
            adap.BusinessId = Convert.ToInt32(Request.Headers.GetValues("BUSINESSID").FirstOrDefault());
            adap.UserId = Convert.ToInt32(Request.Headers.GetValues("USERID").FirstOrDefault());

            if (adap.BusinessId > 0 && adap.UserId > 0 && adap.ItemApprovalId > 0)
                {
                    rv = sl.ShopKeeperAcceptApproveSendToShop(adap);
                    result = cuc.GetJsonObject(ReturnedData, rv);
                }
                else
                {
                    rv.StatusCode = 10; rv.StatusMessage = "you did not supply a vital identity";
                    result = cuc.GetJsonObject(ReturnedData, rv);
                }
            
            return Ok(result);

        }

    }
}
