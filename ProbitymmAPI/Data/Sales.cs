using ProbitymmAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProbitymmAPI.Data
{
    public class Sales
    {
        public ReturnValues ShopKeeperAcceptApproveSendToShop(AdminApprove adap)
        {
            ReturnValues rv = new ReturnValues();
            using (SqlConnection conn = connect.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand("ShopKeeperAcceptsFinishProductToShop", conn))//call Stored Procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@UserId", adap.UserId);
                    cmd.Parameters.AddWithValue("@BusinessId", adap.BusinessId);
                    cmd.Parameters.AddWithValue("@ItemApprovalId", adap.ItemApprovalId);
                    cmd.Parameters.Add("@returnvalue", SqlDbType.Int);
                    cmd.Parameters["@returnvalue"].Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@returnvalueString", System.Data.SqlDbType.VarChar, 100);
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

        public ReturnValues CreateEditStore(ShopModel sm)
        {

            ReturnValues rv = new ReturnValues();
            using (SqlConnection conn = connect.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand("CreateNewStore", conn))//call Stored Procedure
                {
                    
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StoreId", sm.StoreId);
                    cmd.Parameters.AddWithValue("@userId", sm.UserId);
                    cmd.Parameters.AddWithValue("@businessId", sm.BusinessId);
                    cmd.Parameters.AddWithValue("@StoreName",sm.StoreName);

                    cmd.Parameters.Add("@returnvalue", SqlDbType.Int);
                    cmd.Parameters["@returnvalue"].Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@returnvalueString", System.Data.SqlDbType.VarChar, 100);
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

        public List<ShopModel> GetStoreLists(int BusinessId)
        {
            List<ShopModel> prl = new List<ShopModel>();
            using (SqlConnection conn = connect.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand("GetAllStores", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@businessId", BusinessId);
                    try
                    {
                        ShopModel _prl = new ShopModel();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                _prl = new ShopModel
                                {
                                    StoreId = Convert.ToInt32(reader["StoreId"]),
                                    BusinessId = Convert.ToInt32(reader["BusinessId"]),
                                    UserId = Convert.ToInt32(reader["adminId"]),
                                    StoreName = reader["StoreName"] is DBNull ? null : (String)reader["StoreName"],
                                    CreatedBy = reader["fullname"] is DBNull ? null : (String)reader["fullname"],
                                    Createddate = string.IsNullOrEmpty(reader["Createddate"].ToString()) ? (DateTime?)null : DateTime.Parse(reader["Createddate"].ToString()),
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