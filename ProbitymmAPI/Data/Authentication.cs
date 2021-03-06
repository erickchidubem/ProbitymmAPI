﻿using ProbitymmAPI.Models;
using ProbitymmAPI.Security;
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
        public CryptoEngine ce = new CryptoEngine();

        public string GenerateUserToken(int userid,int businessid)
        {
            string tokenString = ce.GenerateRandomeStrings(20);
            string returnToken = "";
            using (SqlConnection conn = connect.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand("GenerateToken", conn))//call Stored Procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@token", tokenString);
                    cmd.Parameters.AddWithValue("@userId", userid);
                    cmd.Parameters.AddWithValue("@businessId",businessid);     
                    cmd.Parameters.Add("@returnvalue", System.Data.SqlDbType.Int);
                    cmd.Parameters["@returnvalue"].Direction = ParameterDirection.Output;
                   
                    try
                    {
                        cmd.ExecuteNonQuery();
                        if(Convert.ToInt32(cmd.Parameters["@returnvalue"].Value) == 1)
                        {
                            returnToken = tokenString;// ce.Encrypt(tokenString, CommonUtilityClass.apiEncryptKey);
                        }                      
                    }
                    catch (Exception ex)
                    {
                        CommonUtilityClass.ExceptionLog(ex);  
                    }
                }
            }
            return returnToken;
            
        }

        public static ReturnValuesBool ValidateToken(string token, int userid,int businessid)
        {
            ReturnValuesBool rvb = new ReturnValuesBool();
            rvb.StatusFlag = false;
            using (SqlConnection conn = connect.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand("ValidateToken", conn))//call Stored Procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@token", token);
                    cmd.Parameters.AddWithValue("@businessId", businessid);
                    cmd.Parameters.AddWithValue("@userId", userid);
                    cmd.Parameters.Add("@returnvalue", System.Data.SqlDbType.Int);
                    cmd.Parameters["@returnvalue"].Direction = ParameterDirection.Output;
                    
                    try
                    {
                        cmd.ExecuteNonQuery();
                        rvb.StatusCode = Convert.ToInt32(cmd.Parameters["@returnvalue"].Value);
                        if (rvb.StatusCode == 1)
                        { rvb.StatusFlag = true; }
                        else
                        {
                            if(rvb.StatusCode == 2) { rvb.StatusMessage = "Token has expired"; }
                            if (rvb.StatusCode == 0) { rvb.StatusMessage = "Another user loggedIn with your account"; }
                            rvb.StatusFlag = false;
                        }
                            
                        
                    }
                    catch (Exception ex)
                    {
                        CommonUtilityClass.ExceptionLog(ex);
                        
                    }
                }
            }
            return rvb;
        }

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
                                       token = GenerateUserToken(Convert.ToInt32(reader["id"]), Convert.ToInt32(reader["businessID"]))
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

        public int CheckLoginCredentials(UserData ld)
        {
            int n = 0;
            using (SqlConnection conn = connect.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand("CheckCredentialsLogin", conn))
                {
                   // @businessid = 1,@userid = 1,@lastpassword = '',@returnvalue = 1
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userid", ld.UserID);
                    cmd.Parameters.AddWithValue("@businessid", ld.businessid);
                    cmd.Parameters.AddWithValue("@lastpassword", ld.lastUpdatePassword);
                    cmd.Parameters.Add("@returnvalue", SqlDbType.Int);
                    cmd.Parameters["@returnvalue"].Direction = ParameterDirection.Output;

                    try
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                n = Convert.ToInt32(cmd.Parameters["@returnvalue"].Value);
                            }
                        }       
                    }
                    catch (Exception ex)
                    {
                        CommonUtilityClass.ExceptionLog(ex);
                    }
                }
            }
            return n;
        }

        public ReturnValues RegisterBusiness(BizRegModel bzm)
        {
            ReturnValues rv = new ReturnValues();
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

        public ReturnValues RegisterBusinessStaff(UserData usd)
        {
            ReturnValues rv = new ReturnValues();
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

        public ReturnValues ChangePassword(ChangePassword cp)
        {
            ReturnValues rv = new ReturnValues();
            using (SqlConnection conn = connect.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand("ChangePassword", conn))//call Stored Procedure
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@businessid", cp.businessid);
                    cmd.Parameters.AddWithValue("@userid", cp.UserID);
                    cmd.Parameters.AddWithValue("@oldpass", cp.OldPassword);
                    cmd.Parameters.AddWithValue("@newpass", cp.NewPassword);

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

        public UserData UserInformation(int userID)
        {
            UserData ud = new UserData();
            using (SqlConnection conn = connect.getConnection())
            {
                using (SqlCommand cmd = new SqlCommand("UserDetails", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@userID", userID);
                    try
                    {
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