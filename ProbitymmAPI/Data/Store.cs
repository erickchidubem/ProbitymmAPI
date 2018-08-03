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
                    cmd.Parameters.AddWithValue("@userid", str.userid);
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

        public ReturnValues AddRemove(StoreMaterial sm)
        {     
            ReturnValues rv = new ReturnValues();
            using (SqlConnection conn = connect.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand("AddRemoveSellRawMaterial", conn))//call Stored Procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@rawMaterialID", sm.RawMaterialId);
                    cmd.Parameters.AddWithValue("@userid", sm.UserId);
                    cmd.Parameters.AddWithValue("@businessid", sm.BusinessId);
                    cmd.Parameters.AddWithValue("@addRemove", sm.AddRemove);
                    cmd.Parameters.AddWithValue("@Qty", sm.Qty);
                    cmd.Parameters.AddWithValue("@costPrice", sm.costPrice);
                    cmd.Parameters.AddWithValue("@sellingPrice", sm.sellingPrice);
                    cmd.Parameters.AddWithValue("@updatePrice", sm.updatePrice);

                    cmd.Parameters.Add("@returnvalue", SqlDbType.Int);
                    cmd.Parameters["@returnvalue"].Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@returnvalueString", System.Data.SqlDbType.VarChar, 200);
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

        public List<StoreModel> GetAllMaterials(int businessid)
        {
            List<StoreModel> sm = new List<StoreModel>();
            using (SqlConnection conn = connect.getConnection())
            {

                using (SqlCommand cmd = new SqlCommand("getBusinessRawMaterials", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@businessid", businessid);
                    try
                    {
                        StoreModel _sm = new StoreModel();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                _sm = new StoreModel {
                                    id = Convert.ToInt32(reader["id"]),
                                    BusinessId = Convert.ToInt32(reader["businessID"]),
                                    userid = Convert.ToInt32(reader["createdBy"]),
                                    createdByName = reader["createdByName"] is DBNull ? null : (String)reader["createdByName"],
                                    MaterialName = reader["MaterialName"] is DBNull ? null : (String)reader["MaterialName"],
                                    Qty = Convert.ToDecimal(reader["Qty"]),
                                    CostPrice = Convert.ToDecimal(reader["CostPrice"]),
                                    SellingPrice = Convert.ToDecimal(reader["SellingPrice"]),
                                    MeasureId = Convert.ToInt32(reader["measureID"]),
                                    measurementTitle = reader["measurement"] is DBNull ? null : (String)reader["measurement"],
                                    active = Convert.ToInt32(reader["active"]),
                                    QtyAlert = Convert.ToDecimal(reader["QtyAlert"])
                                };

                                sm.Add(_sm);
                            }
                        }
                        else
                        {
                            sm = null;
                        }

                    }
                    catch (Exception ex)
                    {
                        CommonUtilityClass.ExceptionLog(ex);
                    }
                }
            }

            return sm;
        }
    }
}