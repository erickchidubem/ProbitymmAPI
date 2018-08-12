using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProbitymmAPI.Models
{
    public class StoreModel
    {

        public int id { get; set; }
        public int BusinessId { get; set; }
        public int userid { get; set; }
        public string createdByName { get; set; }
        public string MaterialName { get; set; }
        public decimal Qty { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int MeasureId { get; set; }
        public string measurementTitle { get; set; }
        public int active { get; set; }
        public decimal QtyAlert { get; set; }
        public DateTime createddate { get; set; }
        public DateTime modifieddate { get; set; }
        public int LastModifiedBy { get; set; }
        public string measurement { get; set; }

    }


    public class StoreMaterial
    {
        public int RawMaterialId { get; set; }
        public int BusinessId { get; set; }
        public int UserId { get; set; }
        public int AddRemove { get; set; }
        public decimal Qty { get; set; }
        public decimal costPrice { get; set; }
        public decimal sellingPrice { get; set; }
        public int updatePrice { get; set; }
    }

    public class RawDisbursement
    {
        public int DistributId { get; set; }
        public int BusinessId { get; set; }
        public int UserId { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<DateTime> createddate { get; set; }
        public int ProductManagerId { get; set; } 
        public string productManagerName { get; set; }
        public int productmanagerAccept { get; set; }
        public Nullable<DateTime> productManagerAcceptDate { get; set; }
        public List<ItemQty> Items { get; set; }  
        public int AdminApprove { get; set; }
        public Nullable<DateTime> AdminApproveDate { get; set; }
        public string AdminName { get; set; }
    }

    public class ItemQty
    {
        public int ItemId { get; set; }
        public int RawMaterialDisId { get; set; }
        public int RawMaterialId { get; set; }
        public string MaterialName { get; set; }
        public decimal Qty { get; set; }
        public string measurement { get; set; }
    }
    
}