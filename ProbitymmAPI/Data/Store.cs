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

        public void AddModifyRawMaterialDistribution(int disId, List<ItemQty> item)
        {
           foreach(var it in item)
            {
                it.RawMaterialDisId = disId;
                this.AddModifyRawMaterialDistributionItem(it);
            }
        }

        public void AddModifyRawMaterialDistributionItem(ItemQty itm)
        {
            ReturnValues rv = new ReturnValues();
            using (SqlConnection conn = connect.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand("RawMaterialDistributionItemsLog", conn))//call Stored Procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ItemId", itm.ItemId);
                    cmd.Parameters.AddWithValue("@RawMaterialDisID", itm.RawMaterialDisId);
                    cmd.Parameters.AddWithValue("@rawMaterialId", itm.RawMaterialId);
                    cmd.Parameters.AddWithValue("@Qty", itm.Qty);
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
          //  return rv;

        }

        public ReturnValues AddModifyRawMaterialDistribution(RawDisbursement rd)
        {
            ReturnValues rv = new ReturnValues();
            using (SqlConnection conn = connect.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand("CreateModifyRawMaterialDistribution", conn))//call Stored Procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DistributId", rd.DistributId);
                    cmd.Parameters.AddWithValue("@userid", rd.UserId);
                    cmd.Parameters.AddWithValue("@businessid", rd.BusinessId);
                    cmd.Parameters.AddWithValue("@ProductManagerId", rd.ProductManagerId);
                    cmd.Parameters.Add("@returnvalue", SqlDbType.Int);
                    cmd.Parameters["@returnvalue"].Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@returnvalueString", System.Data.SqlDbType.VarChar, 200);
                    cmd.Parameters["@returnvalueString"].Direction = ParameterDirection.Output;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        
                       
                        rv.StatusCode = Convert.ToInt32(cmd.Parameters["@returnvalue"].Value);
                        this.AddModifyRawMaterialDistribution(rv.StatusCode,rd.Items);
                        rv.StatusMessage = Convert.ToString(cmd.Parameters["@returnvalueString"].Value);
                        rv.StatusCode = 1;
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
 
        public List<RawDisbursement> getRawMaterialDistributionTicket(int businessid)
        {
            List<RawDisbursement> rd = new List<RawDisbursement>();
            using (SqlConnection conn = connect.getConnection())
            {

                using (SqlCommand cmd = new SqlCommand("getRawMaterialDistributionTicket", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@businessid", businessid);
                    try
                    {
                        RawDisbursement _sm = new RawDisbursement();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                _sm = new RawDisbursement
                                {

                                    DistributId = Convert.ToInt32(reader["DistributId"]),
                                    BusinessId = Convert.ToInt32(reader["BusinessId"]),
                                    UserId = Convert.ToInt32(reader["UserId"]),
                                    CreatedBy = reader["createdUser"] is DBNull ? null : (String)reader["createdUser"],
                                    createddate = Convert.ToDateTime(reader["createddate"]),
                                    ProductManagerId = Convert.ToInt32(reader["ProductManagerId"]),
                                    productManagerName = reader["productmanager"] is DBNull ? null : (String)reader["productmanager"],
                                    productmanagerAccept = Convert.ToInt32(reader["productManagerAccept"]),
                                    productManagerAcceptDate = reader["productManagerAcceptDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["productManagerAcceptDate"]),
                                    AdminApprove = Convert.ToInt32(reader["AdminApprove"]),
                                    AdminName = reader["AdminApproveName"] is DBNull ? null : (String)reader["AdminApproveName"],
                                    AdminApproveDate =  reader["AdminApproveDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["AdminApproveDate"]),
                                    Items = getRawMaterialDistributionTicketItems(Convert.ToInt32(reader["DistributId"])),

                                 
                                };

                                rd.Add(_sm);
                            }
                        }
                        else
                        {
                            rd = null;
                        }

                    }
                    catch (Exception ex)
                    {
                        CommonUtilityClass.ExceptionLog(ex);
                    }
                }
            }

            return rd;
        }

        public List<ItemQty> getRawMaterialDistributionTicketItems(int TicketID)
        {
            List<ItemQty> sm = new List<ItemQty>();
            using (SqlConnection conn = connect.getConnection())
            {

                using (SqlCommand cmd = new SqlCommand("getRawMaterialDistributionTicketItems", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ticketID", TicketID);
                    try
                    {
                        ItemQty _sm = new ItemQty();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                _sm = new ItemQty
                                {
                                    ItemId = Convert.ToInt32(reader["ItemId"]),
                                    RawMaterialDisId = Convert.ToInt32(reader["RawMaterialDisID"]),
                                    RawMaterialId = Convert.ToInt32(reader["RawMaterialId"]),
                                    MaterialName = reader["MaterialName"] is DBNull ? null : (String)reader["MaterialName"],
                                    measurement = reader["measurement"] is DBNull ? null : (String)reader["measurement"]  
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