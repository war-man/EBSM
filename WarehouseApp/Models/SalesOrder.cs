using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WarehouseApp.Models
{
    [Table("SalesOrders")]
    public class SalesOrder
    {
            [Key]
        public int SalesOrderId { get; set; }

        [Display(Name="Order Number")]
        public string OrderNumber { get; set; }

        [Display(Name = "Order Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime OrderDate { get; set; }

        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        public int? CustomerBranchId { get; set; }
        [ForeignKey("CustomerBranchId")]
        public virtual CustomerProject CustomerBranch { get; set; }

        [Display(Name="Total Price")]
        public double? TotalPrice { get; set; }

       [Display(Name = "Total Quantity")]
        public double? TotalQuantity { get; set; }


       [Display(Name = "Discount Type")]
       public string DiscountType { get; set; }
        [Display(Name="Discount Amount")]
        public double? DiscountAmount { get; set; }
        [Display(Name = "Total VAT")]
        public double? TotalVat { get; set; }
        [Display(Name = "Paid Amount")]
        public double? AdvancePaid { get; set; }
       

        [Display(Name = "Transaction Mode")]
        public string TransactionMode { get; set; }
        [Display(Name = "Transaction Mode Id")]
        public int? TransactionModeId { get; set; }

        [Display(Name="Note")]
        public string Note { get; set; }
        [Display(Name = "Cash Paid")]
        public double? CashPaid { get; set; }

        [Display(Name = "WorkOrder Number")]
        [StringLength(30)]
        public string WorkOrderNumber { get; set; }

        [Display(Name = "WorkOrder Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? WorkOrderDate { get; set; }
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
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }

    }

    [Table("OrderProducts")]
    public class OrderProduct
    {
        [Key]
        public int OrderProductId { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual SalesOrder SalesOrder { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [Display(Name = "Batch No")]
        public string BatchNo { get; set; }
        [Display(Name = "Barcode")]
        public string Barcode { get; set; }

        [Display(Name = "Distribution Price")]
        public double Dp { get; set; }
        
        [Display(Name = "Quantity")]
        public double Quantity { get; set; }

        [Display(Name = "Total Price")]
        public double? TotalPrice { get; set; }

        public byte? Status { get; set; }
        [Display(Name = "Zone")]
        public int? ZoneId { get; set; }
        [ForeignKey("ZoneId")]
        public virtual WarehouseZone WarehouseZone { get; set; }
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
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//