using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace WarehouseApp.Models.ViewModels
{
    public class TicketViewModel
    {
        public int? CustomerId { get; set; }

        [Required(ErrorMessage = "Name field is required", AllowEmptyStrings = false)]
        [Display(Name="Full Name")]
        public string FullName { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        [EmailAddress(ErrorMessage = "Please provide valid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Contact No")]
        //[DataType(DataType.PhoneNumber)] //either of this or next line don't work
        [Phone(ErrorMessage = "Contact number is not valid")]
        public string ContactNo { get; set; }

        [Display(Name = "Gender")]
        public string Gender { get; set; }

          [Display(Name = "Age")]
          [Range(0, Int32.MaxValue, ErrorMessage = "Age value must be possitive number")]
        public int? Age { get; set; }

        [Display(Name = "Ticket No")]
        public string TicketNo { get; set; }
        [Display(Name = "Price")]
        public double PriceBeforeDeiscount { get; set; }
        [Display(Name = "Discount")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Discount Amount must be positive number")]
        public double? DiscountAmount { get; set; }
        [Display(Name = "Discount Type")]
        public string DiscountType { get; set; }

        [Display(Name = "Price (Discounted)")]
        public double PriceAfterDiscount { get; set; }
        public double? VatPercentage { get; set; }
        [Display(Name = "VAT")]
        public double? VatAmount { get; set; }
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Number of ticket must be greater than 1")]
        [Display(Name = "Number of Ticket")]
        public int NumberOfTicket { get; set; }
         [Required]
        [Display(Name = "Total Price")]
        public double TotalPrice { get; set; }


    }




    //---TicketPrintModel-------------------------------
    public class TicketPrintModel
    {

        public string TicketType { get; set; }
        public string TicketNo { get; set; }
        public string FullName { get; set; }
        public string ContactNo { get; set; }
        public double PriceBeforeDeiscount { get; set; }
        public double? DiscountAmount { get; set; }
        public string DiscountType { get; set; }
        public double PriceAfterDiscount { get; set; }
        public double? VatPercentage { get; set; }
        public double? VatAmount { get; set; }
        public double TotalPrice { get; set; }


    }
    public class InvoicePrintModel
    {

        public string InvoiceNumber{ get; set; }
        public string InvoiceDateTime { get; set; }
        public string CashierName { get; set; }
        public string PosNumber { get; set; }
        public List<ProductPrintModel> Items { get; set; }
        public double NetTotal { get; set; }
        public double? DiscountAmount { get; set; }
        public string DiscountType { get; set; }
        public double? VatPercentage { get; set; }
        public double? VatAmount { get; set; }
        public double Total { get; set; }
        public double? CashPaid { get; set; }
        public double? ReturnAmount { get; set; }
        public double? RoundingAdj { get; set; }


    }

    public class ProductPrintModel
    {
        public int Sl { get; set; }
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public double? UnitPrice { get; set; }
        public double? Quantity { get; set; }
        public double? Total { get; set; }
    }

    public class PrintText
    {
        public PrintText(string text, Font font) : this(text, font, new StringFormat()) { }

        public PrintText(string text, Font font, StringFormat stringFormat)
        {
            Text = text;
            Font = font;
            StringFormat = stringFormat;
        }

        public string Text { get; set; }

        public Font Font { get; set; }

        /// <summary> Default is horizontal string formatting </summary>
        public StringFormat StringFormat { get; set; }
    }
}


//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : December 2016
//=======================================================================================//