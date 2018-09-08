using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WarehouseApp.Models
{
    [Table("BankAccounts")]
    public class BankAccount
    {
        [Key]
        public int BankId { get; set; }

        [Required]
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [Display(Name = "Branch")]
        public string Branch { get; set; }

        [Required]
        [Display(Name = "Account Name")]
        public string AccountName { get; set; }
        [Required]
        [Display(Name = "Account No")]
        public string AccountNo { get; set; }

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
    }

    [Table("MobileBangking")]
    public class MobileBanking
    {

        [Key]
        public int AccountId { get; set; }

        [Required]
        [Display(Name = "Account Name")]
        public string AccountName { get; set; }
        [Display(Name = "Account Owner")]
        public string AccountOwner { get; set; }
        [Required]
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }

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
    }
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//