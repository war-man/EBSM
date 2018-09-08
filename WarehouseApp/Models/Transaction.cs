using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace WarehouseApp.Models
{
     [Table("Transactions")]
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
          [Display(Name = "Transaction Head")]
        public string TransactionHead { get; set; }
         [Display(Name="Transaction Mode")]
        public string TransactionMode { get; set; }
         [Display(Name = "Transaction Mode Id")]
        public int? TransactionModeId { get; set; }

         [Required]
         [Display(Name = "Amount")]
         public double Amount { get; set; }

         [Required]
         [Display(Name = "Transaction Type")]
         public string TypeOfTransaction { get; set; }
         [Display(Name = "Transaction Date")]
         [DataType(DataType.Date)]
         [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
         public DateTime TransactionDate { get; set; }

         [Required]
         [Display(Name = "Table Name")]
         public string TableName { get; set; }
         [Required]
         [Display(Name = "Primary Key Name")]
         public string PrimaryKeyName { get; set; }
         [Display(Name = "Primary Key Value")]
         public int PrimaryKeyValue { get; set; }
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