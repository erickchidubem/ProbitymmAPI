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
        public ReturnValues CreateModifyRawMaterial(StoreModel str)
        {
            ReturnValues rv = new ReturnValues();
            using (SqlConnection conn = connect.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand("RegisterModifyRawMaterial", conn))//call Stored Procedure
                {                   
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", str.id);
                    cmd.Parameters.AddWithValue("@businessid", str.BusinessId);
                    cmd.Parameters.AddWithValue("@materialName", str.MaterialName);
                    cmd.Parameters.AddWithValue("@costPrice", str.CostPrice);
                    cmd.Parameters.AddWithValue("@sellingPrice", str.SellingPrice);
                    cmd.Parameters.AddWithValue("@measureID", str.MeasureId);
                    cmd.Parameters.AddWithValue("@active", str.active);
                    cmd.Parameters.AddWithValue("@qtyAlert", str.QtyAlert);
                    cmd.Parameters.Add("@returnvalue", SqlDbType.Int);
                    cmd.Parameters["@returnvalue"].Direction = ParameterDirection.Output;
                   cmd.Parameters.Add("@returnvalueString", System.Data.SqlDbType.VarChar,200);
                    cmd.Parameters["@returnvalueString"].Direction = ParameterDirection.Output;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        rv.StatusCode = Convert.ToInt32(cmd.Parameters["@returnvalue"].Value);
                        rv.StatusMessage = Convert.ToString(cmd.Parameters["@returnvalueString"].Value);
                    }
                    catch (Exception ex)
                    {
                        CommonUtilityClass.ExceptionLog(ex);
                        rv.StatusCode = 2000;
                        rv.StatusMessage = "An Error Occured";
                    }
                }
            }
            return rv;
        }
    }
}