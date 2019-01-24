//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Web;

//namespace EBSM.Entities
//{
//    [Table("PurchaseOrders")]
//    public class PurchaseOrder
//    {
//        [Key]
//        public int PurchaseOrderId { get; set; }

//        [Display(Name = "Purchase Order Date")]
//        [DataType(DataType.Date)]
//        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
//        public DateTime PurchaseOrderDate { get; set; }
//         [Display(Name = "Purchase Order Number")]
//        public string PurchaseOrderNumber { get; set; }

//        [Display(Name = "Total Quantity")]
//        public double? TotalQuantity { get; set; }

//        [Display(Name="Total Price")]
//        public double? TotalPrice { get; set; }
//        [Display(Name = "Discount")]
//        public double? PurchaseDiscount { get; set; }
//        [Display(Name = "Paid Amount")]
//        public double? PaidAmount { get; set; }
        
//        [Display(Name = "Transaction Mode")]
//        public string TransactionMode { get; set; }
//        [Display(Name = "Transaction Mode Id")]
//        public int? TransactionModeId { get; set; }
//        public int? SupplierId { get; set; }
//        [ForeignKey("SupplierId")]
//        public virtual Supplier Supplier { get; set; }
//        public byte? Status { get; set; }
//        public int? CreatedBy { get; set; }
//        [ForeignKey("CreatedBy")]
//        public virtual User CreateUser { get; set; }
//        [DataType(DataType.Date)]
//        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
//        public DateTime? CreatedDate { get; set; }
//        public int? UpdatedBy { get; set; }
//        [ForeignKey("UpdatedBy")]
//        public virtual User UpdateUser { get; set; }
//        [DataType(DataType.Date)]
//        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
//        public DateTime? UpdatedDate { get; set; }
//        public int? CompanyId { get; set; }
//        [ForeignKey("CompanyId")]
//        public virtual CompanyProfile CompanyProfile { get; set; }
//        public virtual ICollection<PurchaseOrderProduct> PurchaseOrderProductCollection { get; set; }
//        public virtual ICollection<PurchaseOrderRelation> PurchaseOrderRelationCollection { get; set; }
        
//    }
//    [Table("PurchaseOrderProducts")]
//    public class PurchaseOrderProduct
//    {
//        [Key]
//        public int PurchaseOrderProductId { get; set; }
//        public int PurchaseOrderId { get; set; }
//        [ForeignKey("PurchaseOrderId")]
//        public virtual PurchaseOrder PurchaseOrder { get; set; }

//        public int ProductId { get; set; }
//        [ForeignKey("ProductId")]
//        public virtual Product Product { get; set; }
//        [Display(Name = "Batch No")]
//        public string BatchNo { get; set; }
//        [Display(Name = "Barcode")]
//        public string Barcode { get; set; }
//        [Display(Name = "Quantity")]
//        [Range(0, int.MaxValue, ErrorMessage = "Invalid Input")]
//        public double Quantity { get; set; }

//        [Display(Name = "Unit Price")]
//        [Range(0, double.MaxValue, ErrorMessage = "Invalid Input")]
//        public double? UnitPrice { get; set; }

//        [Display(Name = "Total Price")]
//        public double? TotalPrice { get; set; }
//        [Display(Name = "Zone")]
//        public int? ZoneId { get; set; }
//        [ForeignKey("ZoneId")]
//        public virtual WarehouseZone WarehouseZone { get; set; }
//        public byte? Status { get; set; }
//        public int? CreatedBy { get; set; }
//        [ForeignKey("CreatedBy")]
//        public virtual User CreateUser { get; set; }
//        [DataType(DataType.Date)]
//        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
//        public DateTime? CreatedDate { get; set; }
//        public int? CompanyId { get; set; }
//        [ForeignKey("CompanyId")]
//        public virtual CompanyProfile CompanyProfile { get; set; }
//    }

   
//}
////=======================================================================================//
////Author : Md. Mahid Choudhury
////Creation Date : January 2017
////=======================================================================================//