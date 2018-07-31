using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ProbitymmAPI.Models
{
    public class AuthenticateModel
    {

    }


    public class LoginData
    {
        public string email { get; set; }
        public string password { get; set; }
    }

    public class UserData
    {
        public int businessid { get; set; }
        public int UserID { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public int departmentID { get; set; }
        public int levelID { get; set; }
        public string password { get; set; }
        public DateTime lastUpdatePassword { get; set; }
        public int active { get; set; }
        public int loggedIn { get; set; }
    }
   
    public class BizRegModel
    {
        public string businessName { get; set; }
        public string businessAddress { get; set; }
        public string fullname { get; set; }
        public string email { get; set; }
        public string phone {get;set;}
        public string password { get; set; }
        public string logoImage { get; set; }
    }

    public class ChangePassword
    {
        public int businessid { get; set; }
        public int UserID { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
    
    public class Department
    {
        public int DepartmentId { get; set; }
        public int DepartmentName { get; set; }
    }

}