using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ProbitymmAPI.Data
{
    public class CommonUtilityClass
    {
        public static string apikey = ConfigurationManager.AppSettings["API-KEY"].ToString();

        public static void ExceptionLog(Exception ex)
        {
            
        }

        public Object GetJsonObject(Object _ReturnedData,int _StatusCode,string _StatusMessage)
        {
            var result = (Object)null;          
            result = new { StatusCode = _StatusCode, StatusMessage = _StatusMessage, ReturnedData = _ReturnedData };
            return result;         
        }
    }
}