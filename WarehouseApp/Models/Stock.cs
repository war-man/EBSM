using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WarehouseApp.Models
{
     [Table("Stocks")]
    public class Stock
    {
         [Key]
        public int StockId { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
         [Display(Name="Batch No")]
         public string BatchNo { get; set; }
         [Display(Name = "Barcode")]
         public string Barcode { get; set; }
         [Display(Name = "Manufacture Date")]
         [DataType(DataType.Date)]
         [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
         public DateTime? Mfg { get; set; } 
         [Display(Name = "Expire Date")]
         [DataType(DataType.Date)]
         [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
         public DateTime? Exp { get; set; }

         [Display(Name = "Warranty Date")]
         [DataType(DataType.Date)]
         [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
         public DateTime? Warranty { get; set; }
         [Display(Name = "Purchase Price")]
        public double? PurchasePrice { get; set; }
         [Display(Name = "Sale Price")]
         public double? SalePrice { get; set; }
         [Display(Name = "Total Quantity")]
        public double? TotalQuantity { get; set; }
        
         //[Display(Name = "Zone")]
         //public int? ZoneId { get; set; }
         //[ForeignKey("ZoneId")]
         //public virtual WarehouseZone WarehouseZone { get; set; } 
        public byte? Status { get; set; }

        public int? CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual User CreateUser { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        [ForeignKey("UpdatedBy")]
        public virtual User UpdateUser { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? UpdatedDate { get; set; }
        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual CompanyProfile CompanyProfile { get; set; }
        public virtual ICollection<StockWarehouseRelation> StockWarehouseRelations { get; set; }

    }

     [Table("StockWarehouseRelations")]
     public class StockWarehouseRelation
     {
         [Key]
         public int StockWarehouseRelationId { get; set; }
         public int StockId { get; set; }
         [ForeignKey("StockId")]
         public virtual Stock Stock { get; set; }

         public int ZoneId { get; set; }
         [ForeignKey("ZoneId")]
         public virtual WarehouseZone WarehouseZone { get; set; }

         [Display(Name = "Quantity")]
         public double? Quantity { get; set; }

     }
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//