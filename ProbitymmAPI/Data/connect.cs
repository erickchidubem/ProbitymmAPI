using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProbitymmAPI.Data
{
   
        public class connect
        {
            private static SqlConnection con;
            public static string connectionstring()
            {
                //dbconnectionLive, dbconnectionDev
                string DBCon = ConfigurationManager.ConnectionStrings["dbconnectionLive"].ConnectionString;
                return DBCon;
            }

            public static SqlConnection getConnection()
            {
                try
                {
                    con = new SqlConnection(connectionstring());
                    con.Open();
                }
                catch (Exception ex)
                {
                   CommonUtilityClass.ExceptionLog(ex);
                }
                return con;
            }
        }  
}