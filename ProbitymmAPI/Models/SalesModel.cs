using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProbitymmAPI.Models
{
    public class SalesModel
    {
        
    }

    public class ShopModel
    {
        public int StoreId { get; set; }
        public int BusinessId { get; set; }
        public string StoreName { get; set; }
        public int UserId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ? Createddate { get; set; }
    }

    public class CustomerInfo
    {
        public int CustomerId { get; set; }
        public int BusinessId { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public string email { get; set; }
        public string ContactPerson { get; set; }
        public string BirthInformation { get; set; }
        public string Address { get; set; }
        public string AltPhone { get; set; }
        public string AltEmail { get; set; }
        public int CreatedBy { get; set; }

        public  DateTime createddate {get;set;}
        public  DateTime modifieddate {get;set;}
        public  DateTime LastTimePaid {get;set;}
    }
}