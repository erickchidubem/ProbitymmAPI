using ProbitymmAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProbitymmAPI.Data
{
    public class CommonUtilityClass
    {
        public static string apikey = ConfigurationManager.AppSettings["API-KEY"].ToString();
        public static string apiEncryptKey = "0123456789ABCDEF";

        public async static void ExceptionLog(Exception ex)
        {
            
        }


        public int ConfirmRawMaterialIDAgainstBusinessID(int BusinessId, int anyID,int CheckValueType)
        {
            /*check value type 
             ------------------
             1 - Raw Material
             2 - User Id
             3 - Product Id
             4 - 
             */
            int value = 0;
            using (SqlConnection conn = connect.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand("ConfirmAgainstBusinessID", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@businessID", BusinessId);
                    cmd.Parameters.AddWithValue("@anyID", anyID);
                    cmd.Parameters.AddWithValue("@CheckValueType", CheckValueType);
                    cmd.Parameters.Add("@returnvalue", SqlDbType.Int);
                    cmd.Parameters["@returnvalue"].Direction = ParameterDirection.Output;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        value = Convert.ToInt32(cmd.Parameters["@returnvalue"].Value);  
                    }
                    catch (Exception ex)
                    {
                        ExceptionLog(ex);
                    }
                }
            }

            return value;
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
            else if (i == 3)
            {
                rv.StatusCode = 3000; rv.StatusMessage = "User ID or Business ID is not matching";
            }
            return rv;
        }
    }
}