using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProbitymmAPI.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int BusinessId { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public decimal SellingPrice { get; set; }
        public decimal LastSellingPrice { get; set; }
        public decimal SellingPercentage { get; set; }
        public string Code { get; set; }
        public int active { get; set; }
        public decimal Qty { get; set; }
        public List<ProductRawMaterial> productRaws { get; set; }
    }

    public class AddRemoveQty
    {
        public int userId { get; set; }
        public int businessId { get; set; }
        public int productId { get; set; }
        public decimal Qty { get; set; }
        public int ActionType { get; set; }
    }

    public class productsList
    {

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Qty { get; set; }
        public decimal SellingPercentage { get; set; }
        public decimal CostOfRawMaterial { get; set; }
        public decimal LastSellingPrice { get; set; }
    }

    public class ProductRawMaterial
    {
        public int businessId { get; set; }
        public int productId { get; set; }
        public int rawMaterialID { get; set; }
        public decimal rawMaterialQty { get; set; }
    }

   

    public class FinishedProductToShop
    {
        //@BusinessId int,@StoreId int,@ProductId int,@ActionQty decimal,@ActionType varchar(20),
        //@ProductionManagerId int,@StoreManagerId int,@returnvalue int output
        public int BusinessId { get; set; }
        public int StoreId { get; set; }
        public int ProductId { get; set; }
        public decimal ActionQty { get; set; }
        public int ActionType { get; set; }
        public int ProductionManagerId { get; set; }
        public int StoreManagerId { get; set; }
        
    }

    public class AdminApprove
    {
        public int UserId { get; set; }
        public int BusinessId { get; set; }
        public int ItemApprovalId { get; set; }
    }

    public class PushedProductToStore
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Qty { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public string ActionType { get; set; }
        public int ProductionManagerId { get; set; }
        public string ProductionMangerName { get; set; }
        public int AdminId { get; set; }
        public string AdminWhoApproved { get; set; }
        public int StoreManagerId { get; set; }
        public string StoreManagerAssignTo { get; set; }
        public DateTime ? StoreManagerAcceptDate { get; set; }
        public int StoreManagerAccept { get; set; }
        public int YesAsAdmin { get; set; }

    }
}