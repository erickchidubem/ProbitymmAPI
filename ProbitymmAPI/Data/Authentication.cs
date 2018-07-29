using ProbitymmAPI.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProbitymmAPI.Data
{
    public class Authentication
    {
        public UserData Login(LoginData ld)
        {
            UserData ud = new UserData();
            using (SqlConnection conn = connect.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand("LoginUsers", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@email", ld.email);
                    cmd.Parameters.AddWithValue("@password", ld.password);
                    try
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ud = new UserData {
                                        businessid = Convert.ToInt32(reader["businessID"]),
                                        UserID = Convert.ToInt32(reader["id"]),
                                       email = reader["email"] is DBNull ? null : (String)reader["email"],
                                       phone = reader["phoneNumber"] is DBNull ? null : (String)reader["phoneNumber"],
                                       departmentID = Convert.ToInt32(reader["DepartmentID"]),
                                       levelID = Convert.ToInt32(reader["levelID"]),
                                       fullname = reader["fullname"] is DBNull ? null : (String)reader["fullname"],
                                       active = Convert.ToInt32(reader["active"]),
                                       lastUpdatePassword = Convert.ToDateTime(reader["lastUpdatePassword"]),
                                       loggedIn = Convert.ToInt32(reader["loggedIn"]),


                                };
                            }
                        }
                        else
                        {
                            ud = null;
                        }

                    }
                    catch (Exception ex)
                    {
                         CommonUtilityClass.ExceptionLog(ex);
                    }
                }
            }
            return ud;
        }

        public int RegisterBusiness(BizRegModel bzm)
        {
            int returnResult = 0;
            using (SqlConnection conn = connect.getConnection())
            { 
                using (SqlCommand cmd = new SqlCommand("RegisterBusinessAccount", conn))//call Stored Procedure
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

        public int RegisterBusinessStaff(UserData usd)
        {
            int returnResult = 0;
            using (SqlConnection conn = connect.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand("RegisterBusinessStaff", conn))//call Stored Procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@businessid", usd.businessid);
                    cmd.Parameters.AddWithValue("@userid", usd.UserID);
                    cmd.Parameters.AddWithValue("@fullname", usd.fullname);
                    cmd.Parameters.AddWithValue("@email", usd.email);
                    cmd.Parameters.AddWithValue("@phone", usd.phone);
                    cmd.Parameters.AddWithValue("@departmentID", usd.departmentID);
                    cmd.Parameters.AddWithValue("@levelId", usd.levelID);
                    cmd.Parameters.AddWithValue("@password", usd.password);  

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

        public int ChangePassword(ChangePassword cp)
        {
            int returnResult = 0;
            using (SqlConnection conn = connect.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand("ChangePassword", conn))//call Stored Procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@businessid", cp.businessid);
                    cmd.Parameters.AddWithValue("@userid", cp.UserID);
                    cmd.Parameters.AddWithValue("@oldpass", cp.OldPassword);
                    cmd.Parameters.AddWithValue("@newpass", cp.NewPassword);
                    
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

        public List<UserData> AllBusinessStaffList(int businessid){
            List<UserData> AllStaff = new List<UserData>();
            using (SqlConnection conn = connect.getConnection())
            {
                
                using (SqlCommand cmd = new SqlCommand("SelectBusinessStaff", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@businessid", businessid);
                    try
                    {
                        UserData ud = new UserData();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                ud = new UserData
                                {
                                    businessid = Convert.ToInt32(reader["businessID"]),
                                    UserID = Convert.ToInt32(reader["id"]),
                                    email = reader["email"] is DBNull ? null : (String)reader["email"],
                                    phone = reader["phoneNumber"] is DBNull ? null : (String)reader["phoneNumber"],
                                    departmentID = Convert.ToInt32(reader["DepartmentID"]),
                                    levelID = Convert.ToInt32(reader["levelID"]),
                                    fullname = reader["fullname"] is DBNull ? null : (String)reader["fullname"],
                                    active = Convert.ToInt32(reader["active"]),
                                    lastUpdatePassword = Convert.ToDateTime(reader["lastUpdatePassword"]),
                                    loggedIn = Convert.ToInt32(reader["loggedIn"]),

                                };
                                AllStaff.Add(ud);
                            }
                        }
                        else
                        {
                            ud = null;
                        }

                    }
                    catch (Exception ex)
                    {
                        CommonUtilityClass.ExceptionLog(ex);
                    }
                }
            }
            return AllStaff;
        }
        
    }
}