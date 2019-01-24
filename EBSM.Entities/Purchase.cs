using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EBSM.Entities
{
    [Table("Purchases")]
    public class Purchase
    {
        [Key]
        public int PurchaseId { get; set; }

        [Display(Name = "Purchase Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PurchaseDate { get; set; }
         [Display(Name = "Purchase Number")]
        public string   PurchaseNumber { get; set; }
        [Display(Name = "Purchase Cost")]
        public double? PurchaseCost { get; set; }

        [Display(Name = "Total Quantity")]
        public double? TotalQuantity { get; set; }

        [Display(Name="Total Price")]
        public double? TotalPrice { get; set; }
        [Display(Name = "Discount")]
        public double? PurchaseDiscount { get; set; }
        [Display(Name = "Paid Amount")]
        public double? PaidAmount { get; set; }
        
        [Display(Name = "Transaction Mode")]
        public string TransactionMode { get; set; }
        [Display(Name = "Transaction Mode Id")]
        public int? TransactionModeId { get; set; }
        public int? SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Supplier Supplier { get; set; }
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
        public virtual ICollection<PurchaseProduct> PurchaseProducts { get; set; }
        public virtual ICollection<PurchaseCost> PurchaseCosts { get; set; }
        public virtual ICollection<PurchasePayment> PurchasePayments { get; set; }
        //public virtual ICollection<PurchaseOrderRelation> PurchaseOrderRelationCollection { get; set; }


    }
    [Table("PurchaseProducts")]
    public class PurchaseProduct
    {
        [Key]
        public int PurchaseProductId { get; set; }
        public int PurchaseId { get; set; }
        [ForeignKey("PurchaseId")]
        public virtual Purchase Purchase { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [Display(Name = "Batch No")]
        public string BatchNo { get; set; }
        [Display(Name = "Barcode")]
        public string Barcode { get; set; }
        [Display(Name = "Quantity")]
        [Range(0, int.MaxValue, ErrorMessage = "Invalid Input")]
        public double Quantity { get; set; }

        [Display(Name = "Unit Price")]
        [Range(0, double.MaxValue, ErrorMessage = "Invalid Input")]
        public double? UnitPrice { get; set; }

        [Display(Name = "Total Price")]
        public double? TotalPrice { get; set; }
        [Display(Name = "Zone")]
        public int? ZoneId { get; set; }
        [ForeignKey("ZoneId")]
        public virtual WarehouseZone WarehouseZone { get; set; }
        public byte? Status { get; set; }
        public int? CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual User CreateUser { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedDate { get; set; }
        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual CompanyProfile CompanyProfile { get; set; }
    }

    [Table("PurchaseCosts")]
    public class PurchaseCost
    {
        [Key]
        public int PurchaseCostId { get; set; }

        [Display(Name = "Purchase")]
        public int PurchaseId { get; set; }
        [ForeignKey("PurchaseId")]
        public virtual Purchase Purchase { get; set; }

        [Required]
        [Display(Name = "Amount")]
        public double Amount { get; set; }

        [Display(Name = "Paid")]
        public double? PaidAmount { get; set; }

        [Display(Name = "Note")]
        public string Note { get; set; }

        [Display(Name = "Transaction Mode")]
        public string TransactionMode { get; set; }
        [Display(Name = "Account Id")]
        public int? TransactionModeId { get; set; }
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
    }
    [Table("PurchasePayments")]
    public class PurchasePayment
    {
        [Key]
        public int PurchasePaymentId { get; set; }

        [Display(Name = "Payment Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "Purchase")]
        public int PurchaseId { get; set; }
        [ForeignKey("PurchaseId")]
        public virtual Purchase Purchase { get; set; }

        public int? PurchaseCostId { get; set; }
        [ForeignKey("PurchaseCostId")]
        public virtual PurchaseCost PurchaseCost { get; set; }

        [Display(Name = "Paid Amount")]
        public double? PaidAmount { get; set; }

        [Display(Name = "Note")]
        public string Note { get; set; }

        [Display(Name = "Transaction Mode")]
        public string TransactionMode { get; set; }

        [Display(Name = "Account Id")]
        public int? TransactionModeId { get; set; }

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
    }
    //[Table("PurchaseOrderRelation")]
    //public class PurchaseOrderRelation
    //{
    //    [Key]
    //    public int PurchaseOrderRelationId { get; set; }

    //    public int PurchaseId { get; set; }
    //    [ForeignKey("PurchaseId")]
    //    public virtual Purchase Purchase { get; set; }
    //    public int PurchaseOrderId { get; set; }
    //    [ForeignKey("PurchaseOrderId")]
    //    public virtual PurchaseOrder PurchaseOrder { get; set; }


    //}
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//