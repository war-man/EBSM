using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using EBSM.Entities;

namespace WarehouseApp.Models.ViewModels
{
    [NotMapped]
    public class ExpenseViewModel:Expense
    {
        public new int? ExpenseId { get; set; }
        [Display(Name = "Expense Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public new string ExpenseDate { get; set; }

         [Required]
        [Display(Name = "Transaction Mode")]
        public new string TransactionMode { get; set; }
         [Required]
        [Display(Name = "Account No")]
        public new int TransactionModeId { get; set; }
       
    }
}