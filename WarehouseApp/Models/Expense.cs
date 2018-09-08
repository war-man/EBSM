using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WarehouseApp.Models
{
     [Table("Expenses")]
    public class Expense
    {
             [Key]
        public int ExpenseId { get; set; }
         [Required]
          [Display(Name = "Expense Head")]
        public int ExpenseTypeId { get; set; }
         [ForeignKey("ExpenseTypeId")]
         public virtual ExpenseType ExpenseType { get; set; }

          [Display(Name = "Expense By")]
        public string ExpenseBy { get; set; }

          [Required]
         [Display(Name="Amount")]
         public double Amount{ get; set; }

          [Display(Name = "Description")]
        public string Description { get; set; }
         [Display(Name = "Expense Date")]
          [DataType(DataType.Date)]
          [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
          public DateTime? ExpenseDate { get; set; }

         
         [Display(Name = "Transaction Mode")]
         public string TransactionMode { get; set; }
         
         [Display(Name = "Account No")]
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

     [Table("ExpenseTypes")]
     public class ExpenseType
     {
         [Key]
         public int ExpenseTypeId { get; set; }
         [Display(Name = "Expense Head")]
         public string TypeName { get; set; }
         public int? ParentId { get; set; }
         [ForeignKey("ParentId")]
         public virtual ExpenseType ParentType { get; set; }

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
         public virtual ICollection<ExpenseType> ExpenseTypes { get; set; }
         public virtual ICollection<Expense> Expenses { get; set; }


     }
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//