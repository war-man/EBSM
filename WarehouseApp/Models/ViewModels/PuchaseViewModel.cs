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
    public class PuchaseViewModel:Purchase
    {
        public new int? PurchaseId { get; set; }

        [Display(Name = "Purchase Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public new string PurchaseDate { get; set; }



        [Display(Name = "Total Price")]
        [Range(0, double.MaxValue, ErrorMessage = "Invalid Input")]
        public double? TotalPurchasePrice { get; set; }
        

        [Display(Name = "Paid Amount")]
        [Range(0, double.MaxValue, ErrorMessage = "Invalid Input")]
        public new double? PaidAmount { get; set; }

       

       //purchase cost realted data
        public int? PurchaseCostId { get; set; }
        [Display(Name = "Puchase Cost")]
        [Range(0, double.MaxValue, ErrorMessage = "Invalid Input")]
        public new double? PurchaseCost { get; set; }
        [Display(Name = "Purchase Cost Paid")]
        [Range(0, double.MaxValue, ErrorMessage = "Invalid Input")]
        public double? PaidPurchaseCost { get; set; }
        [Display(Name = "Transaction Mode")]
        public string PcTransactionMode { get; set; }

        [Display(Name = "Account Id")]
        public int? PcTransactionModeId { get; set; }

        public byte? FormSubmitType { get; set; } //1=save only, 2=save and print
        //all purchase products
        public new List<PurchaseProductViewModel> PurchaseProducts { get; set; }

    }

    [NotMapped]
    public class PurchaseProductViewModel:PurchaseProduct
    {
        public new int? PurchaseId { get; set; }
        [ForeignKey("PurchaseId")]
        public new virtual Purchase Purchase { get; set; }


        [Display(Name = "Expiry Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ExpiryDate { get; set; }
    }

    public class PrchaseDuePaymentVm
    {
        public int PurchaseId { get; set; }
         [Display(Name = "Payment For")]
        public string PaymentFor { get; set; }
         public int? PurchaseCostId { get; set; }
        [Display(Name = "Payment Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public string DuePaymentDate { get; set; }

        [Display(Name = "Paid Amount")]
        public double? PaidAmount { get; set; }

        public string DueTransactionMode { get; set; }
        public int TransactionModeId { get; set; }


    }
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//