using ProbitymmAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProbitymmAPI.Data
{
    public class Product
    {
        public ReturnValues CreateEditFinishRawMaterial(ProductModel pm)
        {
            ReturnValues rv = new ReturnValues();
            using (SqlConnection conn = connect.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand("CreateFinishedProducts", conn))//call Stored Procedure
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@mainProductId", pm.ProductId);
                    cmd.Parameters.AddWithValue("@ProductName", pm.ProductName);
                    cmd.Parameters.AddWithValue("@BusinessId", pm.BusinessId);
                    cmd.Parameters.AddWithValue("@userId", pm.UserId);
                    cmd.Parameters.AddWithValue("@categoryID", pm.CategoryId);
                    cmd.Parameters.AddWithValue("@SellingPrice", pm.SellingPrice);
                    cmd.Parameters.AddWithValue("@Code", pm.Code);
                    cmd.Parameters.AddWithValue("@active", 1);
                    cmd.Parameters.AddWithValue("@SellingPercentage", pm.SellingPercentage);

                    cmd.Parameters.Add("@returnvalue", SqlDbType.Int);
                    cmd.Parameters["@returnvalue"].Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@productID", SqlDbType.Int);
                    cmd.Parameters["@productID"].Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@returnvalueString", System.Data.SqlDbType.VarChar, 200);
                    cmd.Parameters["@returnvalueString"].Direction = ParameterDirection.Output;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        int myProductId = Convert.ToInt32(cmd.Parameters["@productID"].Value);
                        rv.StatusCode = Convert.ToInt32(cmd.Parameters["@returnvalue"].Value);                   
                        this.AddModifyRawMaterialForFinishedProducts(myProductId, pm.BusinessId, pm.productRaws);
                        rv.StatusMessage = Convert.ToString(cmd.Parameters["@returnvalueString"].Value);
                    }
                    catch (Exception ex)
                    {
                        CommonUtilityClass.ExceptionLog(ex);
                        Console.WriteLine(ex.Message);
                        rv.StatusCode = 2000;
                        rv.StatusMessage = "An Error Occured";
                    }
                }
            }
            return rv;
        }

        public void AddModifyRawMaterialForFinishedProducts(int productId, int businessId, List<ProductRawMaterial> item)
        {
            foreach (var it in item)
            {
                it.productId = productId;
                it.businessId = businessId;
                this.AddModifyRawMaterialForFinishedProductsItems(it);
            }
        }

        public void AddModifyRawMaterialForFinishedProductsItems(ProductRawMaterial itm)
        {
            using (SqlConnection conn = connect.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand("AddFinishedProductRawMaterials", conn))//call Stored Procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@businessid", itm.businessId);
                    cmd.Parameters.AddWithValue("@productID", itm.productId);
                    cmd.Parameters.AddWithValue("@rawMaterialID", itm.rawMaterialID);
                    cmd.Parameters.AddWithValue("@rawMaterialQty", itm.rawMaterialQty);
                    cmd.Parameters.Add("@returnvalue", SqlDbType.Int);
                    cmd.Parameters["@returnvalue"].Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@returnvalueString", System.Data.SqlDbType.VarChar, 200);
                    cmd.Parameters["@returnvalueString"].Direction = ParameterDirection.Output; 
                    try
                    {
                        cmd.ExecuteNonQuery();
                        /*  rv.StatusCode = Convert.ToInt32(cmd.Parameters["@returnvalue"].Value);
                          rv.StatusMessage = Convert.ToString(cmd.Parameters["@returnvalueString"].Value); */
                    }
                    catch (Exception ex)
                    {
                        CommonUtilityClass.ExceptionLog(ex);
                        /*    rv.StatusCode = 2000;
                            rv.StatusMessage = "An Error Occured"; */
                    }
                }
            }

        }

        public ReturnValues AddRemoveSellProductQty(AddRemoveQty pr)
        {

            ReturnValues rv = new ReturnValues();
            using (SqlConnection conn = connect.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand("AddRemoveProductQty", conn))//call Stored Procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@productId", pr.productId);
                    cmd.Parameters.AddWithValue("@userid", pr.userId);
                    cmd.Parameters.AddWithValue("@businessid", pr.businessId);
                    cmd.Parameters.AddWithValue("@addRemove", pr.ActionType);
                    cmd.Parameters.AddWithValue("@Qty", pr.Qty);

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
                return rv;
            }


        }

        public ReturnValues SendFinishProductToShop(FinishedProductToShop fp)
        {
            ReturnValues rv = new ReturnValues();
            using (SqlConnection conn = connect.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand("SendFinishedProductToStore", conn))//call Stored Procedure
                {
                    //fp.ActionType --- 1 -> Add to Store, 2-> Moved from store, 3-> sold from store
                    
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BusinessId", fp.BusinessId);
                    cmd.Parameters.AddWithValue("@StoreId", fp.StoreId);
                    cmd.Parameters.AddWithValue("@ProductId", fp.ProductId);
                    cmd.Parameters.AddWithValue("@ActionQty", fp.ActionQty);
                    cmd.Parameters.AddWithValue("@ActionType", "Add To Store");
                    cmd.Parameters.AddWithValue("@ProductionManagerId", fp.ProductionManagerId);
                    cmd.Parameters.AddWithValue("@StoreManagerId", fp.StoreManagerId);

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
                        Console.WriteLine("We are catching an exception");
                        CommonUtilityClass.ExceptionLog(ex);
                        Console.WriteLine(ex.Message);
                        rv.StatusCode = 2000;
                        rv.StatusMessage = "An Error Occured";
                    }
                }
            }
            return rv;
        }


        public ReturnValues AdminApproveSendToShop(AdminApprove adap)
        {
            ReturnValues rv = new ReturnValues();
            using (SqlConnection conn = connect.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand("ApproveMovingProductToShop", conn))//call Stored Procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", adap.UserId);
                    cmd.Parameters.AddWithValue("@BusinessId", adap.BusinessId);
                    cmd.Parameters.AddWithValue("@ItemApprovalId", adap.ItemApprovalId);                  
                    cmd.Parameters.Add("@returnvalue", SqlDbType.Int);
                    cmd.Parameters["@returnvalue"].Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@returnvalueString", System.Data.SqlDbType.VarChar, 50);
                    cmd.Parameters["@returnvalueString"].Direction = ParameterDirection.Output;
                    try
                    {
                        cmd.ExecuteNonQuery();
                        rv.StatusCode = Convert.ToInt32(cmd.Parameters["@returnvalue"].Value);
                        rv.StatusMessage = Convert.ToString(cmd.Parameters["@returnvalueString"].Value);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("We are catching an exception");
                        CommonUtilityClass.ExceptionLog(ex);
                        Console.WriteLine(ex.Message);
                        rv.StatusCode = 2000;
                        rv.StatusMessage = "An Error Occured";
                    }
                }
            }
            return rv;
        }

        public List<ProductRawMaterial> GetProductsRawMaterial(int proid,int busId)
        {
            List<ProductRawMaterial> prl = new List<ProductRawMaterial>();
            using (SqlConnection conn = connect.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand("getRawMaterialByProductId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@productId", proid);
                    try
                    {
                        ProductRawMaterial _prl = new ProductRawMaterial();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                _prl = new ProductRawMaterial
                                {
                                    productId = proid,
                                    businessId = busId,
                                    rawMaterialID = Convert.ToInt32(reader["rawMaterialID"]),
                                    rawMaterialName = reader["MaterialName"] is DBNull ? null : (String)reader["MaterialName"],
                                    rawMaterialQty = Convert.ToDecimal(reader["rawMaterialQty"]),                            
                                };

                                prl.Add(_prl);
                            }
                        }
                        else
                        {
                            prl = null;
                        }

                    }
                    catch (Exception ex)
                    {
                        CommonUtilityClass.ExceptionLog(ex);
                    }
                }
            }
            return prl;
        }

        public List<productsList> GetProductsLists(int BusinessId)
        {
            List<productsList> prl = new List<productsList>();
            using (SqlConnection conn = connect.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand("GetAllProducts", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@businessId", BusinessId);
                    try
                    {
                        productsList _prl = new productsList();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                _prl = new productsList
                                {
                                    ProductId = Convert.ToInt32(reader["id"]),
                                    ProductName = reader["ProductName"] is DBNull ? null : (String)reader["ProductName"],
                                    Code = reader["Code"] is DBNull ? null : (String)reader["Code"],
                                    SellingPercentage = Convert.ToDecimal(reader["SellingPercentage"]),
                                    Qty = Convert.ToDecimal(reader["Qty"]),
                                    CostOfRawMaterial = Convert.ToDecimal(reader["CostOfRawMaterial"]),
                                    LastSellingPrice = Convert.ToDecimal(reader["LastSellingPrice"]),
                                    ProductRawMaterials = GetProductsRawMaterial(Convert.ToInt32(reader["id"]),BusinessId)
                                };

                                prl.Add(_prl);
                            }
                        }
                        else
                        {
                            prl = null;
                        }

                    }
                    catch (Exception ex)
                    {
                        CommonUtilityClass.ExceptionLog(ex);
                    }
                }
            }
            return prl;
        }

        public List<PushedProductToStore> GetAllFinishedProductSentToShop(int businessid,int userid,int type)
        {
            List<PushedProductToStore> prl = new List<PushedProductToStore>();
            using (SqlConnection conn = connect.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand("getAllProductPushedToShop", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@businessid", businessid);
                    cmd.Parameters.AddWithValue("@userid", userid);
                    cmd.Parameters.AddWithValue("@type", type);
                    try
                    {
                        PushedProductToStore _prl;
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                _prl = new PushedProductToStore
                                {
                                 
                                    ProductId = Convert.ToInt32(reader["ProductId"]),
                                    ProductName = reader["ProductName"] is DBNull ? null : (String)reader["ProductName"],
                                    Qty = Convert.ToDecimal(reader["Qty"]),
                                    StoreId = Convert.ToInt32(reader["StoreId"]),
                                    StoreName = reader["StoreName"] is DBNull ? null : (String)reader["StoreName"],
                                    ActionType = reader["ActionType"] is DBNull ? null : (String)reader["ActionType"],

                                    ProductionManagerId = Convert.ToInt32(reader["ProductionManagerId"]),
                                    ProductionMangerName = reader["ProductionMangerName"] is DBNull ? null : (String)reader["ProductionMangerName"],
                                    AdminId = Convert.ToInt32(reader["AdminId"]),
                                    AdminWhoApproved = reader["AdminWhoApproved"] is DBNull ? null : (String)reader["AdminWhoApproved"],
                                    StoreManagerId = Convert.ToInt32(reader["StoreManagerId"]),
                                    StoreManagerAssignTo = reader["StoreManagerAssignTo"] is DBNull ? null : (String)reader["StoreManagerAssignTo"],
                                    StoreManagerAcceptDate = string.IsNullOrEmpty(reader["StoreManagerAcceptDate"].ToString()) ? (DateTime?)null : DateTime.Parse(reader["StoreManagerAcceptDate"].ToString()),
                                    StoreManagerAccept = Convert.ToInt32(reader["StoreManagerAccept"]),
                                    YesAsAdmin = Convert.ToInt32(reader["YesAsAdmin"]),                                   
                                };

                                prl.Add(_prl);
                            }
                        }
                        else
                        {
                            prl = null;
                        }

                    }
                    catch (Exception ex)
                    {
                        CommonUtilityClass.ExceptionLog(ex);
                    }
                }
            }
            return prl;

        }
    }
}