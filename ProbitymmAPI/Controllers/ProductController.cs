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
   
    public class ProductController : BaseController
    {
        CommonUtilityClass cuc = new CommonUtilityClass();
        ReturnValues rv = new ReturnValues();
        Product pr = new Product();

        [HttpPost]
        public IHttpActionResult CreateEditFinishedProduct([FromBody]ProductModel product)
        {
            var result = (Object)null;
            var ReturnedData = (Object)null;      
            if (product.BusinessId > 0)
            {
                rv = pr.CreateEditFinishRawMaterial(product);
                result = cuc.GetJsonObject(ReturnedData, rv);
            }
            else
            {
                rv.StatusCode = 10; rv.StatusMessage = "you did not supply businessid";
                result = cuc.GetJsonObject(ReturnedData, rv);
            }
            return Ok(result);
               
        }

        [HttpPost]
        public IHttpActionResult AddRemoveFinishedProductQty([FromBody]AddRemoveQty arq)
        {
            var result = (Object)null;
            var ReturnedData = (Object)null;
            arq.businessId = Convert.ToInt32(Request.Headers.GetValues("BUSINESSID").FirstOrDefault());
            arq.userId = Convert.ToInt32(Request.Headers.GetValues("USERID").FirstOrDefault());

            if (arq.ActionType > 0 && arq.productId > 0 && arq.ActionType > 0)
            {
                rv = pr.AddRemoveSellProductQty(arq);
                result = cuc.GetJsonObject(ReturnedData, rv);
            }
            else
            {
                rv.StatusCode = 10; rv.StatusMessage = "you did not supply a vital identity";
                result = cuc.GetJsonObject(ReturnedData, rv);
            }

            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult FinishProductToShop([FromBody]FinishedProductToShop fpts)
        {
            var result = (Object)null;
            var ReturnedData = (Object)null;           
            if (fpts.BusinessId > 0 && fpts.StoreId > 0 && fpts.ProductionManagerId > 0 && fpts.StoreManagerId > 0)
            {
                rv = pr.SendFinishProductToShop(fpts);
                result = cuc.GetJsonObject(ReturnedData, rv);
            }
            else
            {
                rv.StatusCode = 10; rv.StatusMessage = "you did not supply a vital identity";
                result = cuc.GetJsonObject(ReturnedData, rv);
            }
            return Ok(result);
               
        }

        [HttpPost]
        public IHttpActionResult AdminApproveFinishProductToShop([FromBody] AdminApprove adap)
        {
            var result = (Object)null;
            var ReturnedData = (Object)null;
            if (VerifyUserBusinessID(adap.BusinessId, adap.UserId))
            {
                if (adap.BusinessId > 0 && adap.UserId > 0 && adap.ItemApprovalId > 0)
                {
                    rv = pr.AdminApproveSendToShop(adap);
                    result = cuc.GetJsonObject(ReturnedData, rv);
                }
                else
                {
                    rv.StatusCode = 10; rv.StatusMessage = "you did not supply a vital identity";
                    result = cuc.GetJsonObject(ReturnedData, rv);
                }
            }
            else
            {
                result = cuc.Error(3);
            }
            return Ok(result);
            
        }

      
        [HttpGet]
        public IHttpActionResult GetAllProducts()
        {
            var result = (Object)null;
            var ReturnedData = (Object)null;
            int businessid = Convert.ToInt32(Request.Headers.GetValues("BUSINESSID").FirstOrDefault());

            ReturnedData = pr.GetProductsLists(businessid);
            rv.StatusCode = 1;
            rv.StatusMessage = "Your product List";
            result = cuc.GetJsonObject(ReturnedData, rv);          
            return Ok(result);
        }


        [HttpGet]
        public IHttpActionResult GetAllProductPushedByProductManager([FromUri]int getType)
        {
            var result = (Object)null;
            var ReturnedData = (Object)null;
            int businessid = Convert.ToInt32(Request.Headers.GetValues("BUSINESSID").FirstOrDefault());
            int userid = Convert.ToInt32(Request.Headers.GetValues("USERID").FirstOrDefault());

            ReturnedData = pr.GetAllFinishedProductSentToShop(businessid,userid,getType);
            rv.StatusCode = 1;
            rv.StatusMessage = "Your pushed product List";
            result = cuc.GetJsonObject(ReturnedData, rv);
            return Ok(result);
        }
    }
}
