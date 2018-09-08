using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WarehouseApp.Models.ViewModels
{
    public class WithdrawDepositViewModel
    {
        [Required]
        [Display(Name="Transaction Type")]
        public string TransactionType { get; set; }

        [Required]
        [Display(Name = "Transaction Mode")]
        public string TransactionMode { get; set; }

        [Required]
        [Display(Name = "Account")]
        public int TransactionModeId { get; set; }

        [Required]
        [Display(Name = "Amount")]
        [Range(1, double.MaxValue, ErrorMessage = "Invalid Input")]
        public double Amount { get; set; }

    }
}

//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//