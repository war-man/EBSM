using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace WarehouseApp.Models.ViewModels
{
    public class BillInvoicesViewModel
    {
        [Display(Name = "Bill Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public string BillDate { get; set; }
        public Bill Bill { get; set; }
        public List<InvoiceCheckboxViewModel> InvoiceCheckboxes { get; set; }
    }
    public class InvoiceCheckboxViewModel
    {
        public int InvoiceId { get; set; }

        public Invoice Invoice { get; set; }
        public bool IsChecked { get; set; }
    }
    public class BillPaymentViewModel
    {
        public int CustomerId { get; set; }
        [Display(Name = "Payment Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public string PaymentDate { get; set; }

        [Required]
        //[Range(0.01,double.MaxValue)]
        [Display(Name = "Paid Amount")]
        public double PaidAmount { get; set; }
        [Required]
        [Display(Name = "Transaction Mode")]
        public string TransactionMode { get; set; }
        [Required]
        [Display(Name = "Account")]
        public int TransactionModeId { get; set; }

        [Display(Name = "Note")]
        public string Note { get; set; }
    }
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//