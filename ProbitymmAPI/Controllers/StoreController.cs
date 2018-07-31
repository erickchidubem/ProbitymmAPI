using ProbitymmAPI.Data;
using ProbitymmAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ProbitymmAPI.Controllers
{
    public class StoreController : ApiController
    {
        CommonUtilityClass cuc = new CommonUtilityClass();
        ReturnValues rv = new ReturnValues();

        [HttpPost]
        public IHttpActionResult RegisterModifyRawMaterial([FromBody]StoreModel store)
        {
            var result = (Object)null;
            var ReturnedData = (Object)null;        
            if (Request.Headers.Contains("API-KEY"))
            {
                string apikey = Request.Headers.GetValues("API-KEY").First();
                if (apikey == CommonUtilityClass.apikey)
                {
                    Store st = new Store();
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
                else
                {
                    return Content(HttpStatusCode.Unauthorized, cuc.GetJsonObject(ReturnedData,cuc.Error(1)));
                }
            }
            else
            {
                return Content(HttpStatusCode.Forbidden, cuc.GetJsonObject(ReturnedData, cuc.Error(2)));
            }
        }


        [HttpPost]
        public IHttpActionResult AddRemoveRawMaterials([FromBody]StoreMaterial sm)
        {
            var result = (Object)null;
            var ReturnedData = (Object)null;
            if (Request.Headers.Contains("API-KEY"))
            {
                string apikey = Request.Headers.GetValues("API-KEY").First();
                if (apikey == CommonUtilityClass.apikey)
                {
                    Store st = new Store();
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
    }
}
