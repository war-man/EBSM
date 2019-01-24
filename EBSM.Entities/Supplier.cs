using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace EBSM.Entities
{
    [Table("Suppliers")]
    public class Supplier
    {
        [Key]
        public int SupplierId { get; set; }
        [Required]
        [Display(Name = "Supplier")]
        public string SupplierName { get; set; }
        [Display(Name = "Supplier Type")]      // 1=Manufacturer,2=Distributor,3=Both
        public int? SupplierType { get; set; }

        [Display(Name = "Address")]
        public string SupplierAddress { get; set; }

        [Display(Name = "Contact Person")]
        public string ContactPersonName { get; set; }
        [Display(Name = "Position")]
        public string Position { get; set; }

        [EmailAddress(ErrorMessage = "Please provide valid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Contact No")]
        public string ContactNo { get; set; }

        [Display(Name = "Balance")]
        public double? Balance { get; set; }

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

        public virtual ICollection<Purchase> Purchases { get; set; }

    }
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//