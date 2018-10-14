using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace WarehouseApp.Models.ViewModels
{
    [NotMapped]
    public class InvoiceViewModel:Invoice
    {
        public new int? InvoiceId { get; set; }
        [Display(Name = "Invoice Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public new string InvoiceDate { get; set; }
         [Display(Name = "Project")]
        public new int CustomerId { get; set; }
        //[ForeignKey("CustomerId")]
        //public virtual Customer Customer { get; set; }
        [Display(Name = "Project Name")]
        public string CustomerName { get; set; }
        [Display(Name = "Contact No")]
        [Phone(ErrorMessage = "Contact number is not valid")]
        public string CustomerContactNo { get; set; }
        [EmailAddress(ErrorMessage = "Please provide valid email address")]
        [Display(Name = "Customer Email")]
        public string CustomerEmail { get; set; }
        public int? OrderId { get; set; }
        public byte? FormSubmitType { get; set; } //1=save only, 2=save and print
        public new List<InvoiceProductViewModel> InvoiceProducts { get; set; }

    }
    [NotMapped]
    public class InvoiceProductViewModel:InvoiceProduct
    {
        public new int? InvoiceProductId { get; set; }
        public int? DefaultZoneId { get; set; }
        [Display(Name = "Unit Price")]
        [Range(0, double.MaxValue, ErrorMessage = "Invalid Input")]
        public double? UnitPrice { get; set; }
        [Display(Name = "VAT")]
        public double? Vat { get; set; }
    }

    public class PoImportViewModel
    {
        [Required]
        [Display(Name = "PO Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public string PoDate { get; set; }

        [Display(Name = "Project")]
        public int CustomerId { get; set; }
        [Display(Name = " Branch")]
        public int? CustomerBranchId { get; set; }
           [Display(Name = "File")]
        [Required]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase PoExcelFile { get; set; }
        //public List<PoProductViewModel> PoProducts { get; set; }
    }
    public class PoProductViewModel
    {
        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }

        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }
        public double? Quantity { get; set; }

        [Display(Name = "Retail Price")]
        public double? RetailPrice { get; set; }
        public double? TotalPrice { get; set; }

      
    }

    [NotMapped]
    public class SalesReturnViewModel : Return
    {
        public new int? ReturnId { get; set; }
        [Display(Name = "Return Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public  string ReturnDate { get; set; }
        [Display(Name = "Total Return")]
        public double? TotalReturn { get; set; }
        public new int InvoiceId { get; set; }
        public byte? FormSubmitType { get; set; } //1=save only, 2=save and print
        public new List<ReturnProductViewModel> ReturnProducts { get; set; }

    }
    [NotMapped]
    public class ReturnProductViewModel : ReturnProduct
    {
        public new int? InvoiceProductId { get; set; }
        public int? DefaultZoneId { get; set; }
        [Display(Name = "Unit Price")]
        [Range(0, double.MaxValue, ErrorMessage = "Invalid Input")]
        public new double? UnitPrice { get; set; }
        [Display(Name = "Batch No")]
        public string BatchNo { get; set; }
        [Display(Name = "Barcode")]
        public string Barcode { get; set; }
        [Display(Name = "Return Quantity")]
        [Range(0, double.MaxValue, ErrorMessage = "Invalid Input")]
        public double? ReturnQty { get; set; }
        [Display(Name = "Return Amount")]
        [Range(0, double.MaxValue, ErrorMessage = "Invalid Input")]
        public double? ReturnAmount { get; set; }
    }

}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//