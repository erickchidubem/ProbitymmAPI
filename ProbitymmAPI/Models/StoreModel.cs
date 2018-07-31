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
        public string MaterialName { get; set; }
        public decimal Qty { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SellingPrice { get; set; }
        public int MeasureId { get; set; }
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

    
}