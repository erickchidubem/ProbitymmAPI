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
        public string MaterialName { get; set; }
        public int Qty { get; set; }
        public int CostPrice { get; set; }
        public int SellingPrice { get; set; }
        public int MeasureId { get; set; }
        public int active { get; set; }
        public int QtyAlert { get; set; }

    }

    
}