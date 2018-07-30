using ProbitymmAPI.Models;
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

        public Object GetJsonObject(Object _ReturnedData,ReturnValues rv)
        {
            var result = (Object)null;          
            result = new { StatusCode = rv.StatusCode, StatusMessage =rv.StatusMessage, ReturnedData = _ReturnedData };
            return result;         
        }

        public ReturnValues Error(int i)
        {
            ReturnValues rv = new ReturnValues();
            if (i == 1)
            {
                rv.StatusCode = 100; rv.StatusMessage = "Wrong API Key";
            }
            else if (i == 2)
            {
                rv.StatusCode = 1000; rv.StatusMessage = "Incorrect header specification";
            }
            return rv;
        }
    }
}