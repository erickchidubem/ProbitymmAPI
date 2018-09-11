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
    public class StoreController : ApiController
    {
        private CommonUtilityClass cuc = new CommonUtilityClass();
        ReturnValues rv = new ReturnValues();
        Store st = new Store();


        [HttpPost]
        public IHttpActionResult RegisterModifyRawMaterial([FromBody]StoreModel store)
        {
            var result = (Object)null;
            var ReturnedData = (Object)null;        
                    
            if (store.BusinessId > 0)
            {
                       
                rv = st.CreateModifyRawMaterial(store);
                result = cuc.GetJsonObject(ReturnedData, rv);                                                                      
            }
            else
            {
                rv.StatusCode = 6; rv.StatusMessage = "you did not supply businessid";
                result = cuc.GetJsonObject(ReturnedData,rv);
            }
            return Ok(result);
        }

        [HttpPost]
        public IHttpActionResult AddRemoveRawMaterials([FromBody]StoreMaterial sm)
        {
            var result = (Object)null;
            var ReturnedData = (Object)null;
            
            if (sm.UserId > 0 && sm.BusinessId > 0)
            {
                rv = st.AddRemove(sm);
                result = cuc.GetJsonObject(ReturnedData, rv);
            }
            else
            {
                rv.StatusCode = 6; rv.StatusMessage = "you did not supply businessid or userid";
                result = cuc.GetJsonObject(ReturnedData, rv);
            }
            return Ok(result);
               
        }

        [HttpGet]
        public IHttpActionResult GetAllRawMaterials([FromUri]int BusinessId)
        {

            var result = (Object)null;
            var ReturnedData = (Object)null;
            
            if (BusinessId > 0)
            {
                rv.StatusCode = 1; rv.StatusMessage = "Your raw material list";
                ReturnedData = st.GetAllMaterials(BusinessId);
                result = cuc.GetJsonObject(ReturnedData, rv);
            }
            else
            {
                rv.StatusCode = 0; rv.StatusMessage = "you did not supply businessid";
                result = cuc.GetJsonObject(ReturnedData, rv);
            }
            return Ok(result);
               
        }
       
        [HttpPost]
        public IHttpActionResult PostRawMaterialDisbursement([FromBody]RawDisbursement rd)
        {
            var result = (Object)null;
            var ReturnedData = (Object)null;
           
            if (rd.BusinessId > 0  && rd.UserId > 0 && rd.ProductManagerId > 0)
            {
                if (cuc.ConfirmRawMaterialIDAgainstBusinessID(rd.BusinessId, rd.ProductManagerId, 2) == 1)
                {
                    rv = st.AddModifyRawMaterialDistribution(rd);
                    result = cuc.GetJsonObject(ReturnedData, rv);
                }
                else
                {
                    rv.StatusCode = 7; rv.StatusMessage = "This product manager does not exist in your business";
                    result = cuc.GetJsonObject(ReturnedData, rv);
                }
            }
            else
            {
                rv.StatusCode = 6; rv.StatusMessage = "you did not supply businessid or userid or productManagerId";
                result = cuc.GetJsonObject(ReturnedData, rv);
            }
            return Ok(result);
               
        }

        [HttpGet]
        public IHttpActionResult GetAllRawMaterialDisbursement([FromUri]int BusinessId)
        {           
            var result = (Object)null;
            var ReturnedData = (Object)null;
           
            if (BusinessId > 0)
            {
                rv.StatusCode = 1; rv.StatusMessage = "Your raw material list";
                ReturnedData = st.getRawMaterialDistributionTicket(BusinessId);
                result = cuc.GetJsonObject(ReturnedData, rv);   
            }
            else
            {
                rv.StatusCode = 0; rv.StatusMessage = "you did not supply businessid";
                result = cuc.GetJsonObject(ReturnedData, rv);
            }
            return Ok(result);
              
        }
    }
}
