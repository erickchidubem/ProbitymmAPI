using ProbitymmAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProbitymmAPI.Data
{
    public class Store
    {
        public int CreateRawMaterial(StoreModel store)
        {
            int returnResult = 0;
            using (SqlConnection conn = connect.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand("RegisterEditRawMaterial", conn))//call Stored Procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@businessname", bzm.businessName);
                    cmd.Parameters.AddWithValue("@businessAddress", bzm.businessAddress);
                    cmd.Parameters.AddWithValue("@fullname", bzm.fullname);
                    cmd.Parameters.AddWithValue("@email", bzm.email);
                    cmd.Parameters.AddWithValue("@phone", bzm.phone);
                    cmd.Parameters.AddWithValue("@password", bzm.password);
                    cmd.Parameters.Add("@returnvalue", System.Data.SqlDbType.Int);
                    cmd.Parameters["@returnvalue"].Direction = ParameterDirection.Output;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        returnResult = Convert.ToInt32(cmd.Parameters["@returnvalue"].Value);
                    }
                    catch (Exception ex)
                    {
                        CommonUtilityClass.ExceptionLog(ex);
                    }
                }
            }
            return returnResult;
        }
    }
}