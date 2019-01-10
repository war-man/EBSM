using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EBSM.Entities
{
    [Table("Returns")]
    public class Return
    {
        [Key]
        public int ReturnId { get; set; }
        public int? InvoiceId { get; set; }
        [ForeignKey("InvoiceId")]
        public virtual Invoice Invoice { get; set; }


        [Display(Name = "Total Quantity")]
        public double? TotalQuantity { get; set; }

        [Display(Name = "Total Return")]
        public double? TotalPrice { get; set; }

        public byte? Status { get; set; }

        public int? CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual User CreateUser { get; set; }
          [Display(Name = "Return Date")]
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
        public virtual ICollection<ReturnProduct> ReturnProducts { get; set; }
    }

    [Table("ReturnProducts")]
    public class ReturnProduct
    {
        [Key]
        public int ReturnProductId { get; set; }
        public int? ReturnId { get; set; }
        [ForeignKey("ReturnId")]
        public virtual Return Return { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public int? InvoiceProductId { get; set; }
        [ForeignKey("InvoiceProductId")]
        public virtual InvoiceProduct InvoiceProduct { get; set; }

        [Display(Name = "Quantity")]
        public double? Quantity { get; set; }
        [Display(Name = "Unit Price")]
        public double? UnitPrice { get; set; }

        [Display(Name = "Total Price")]
        public double? TotalPrice { get; set; }
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
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//