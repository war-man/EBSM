using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EBSM.Entities;

namespace WarehouseApp.Models.ViewModels
{
    public class BarcodeViewModel
    {
   
        [Display(Name = "Product")]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        [Display(Name = "Batch")]
        public string BatchNo { get; set; }
        [Display(Name = "Barcode")]
        [Remote("IsBarcodeExist", "Batch", AdditionalFields = "InitialBarcode", ErrorMessage = "Barcode is already exist")]
        public string Barcode { get; set; }

        [Display(Name = "Purchase Price")]
        public double? PurchasePrice { get; set; }
        [Display(Name = "Sale Price")]
        public double? SalePrice { get; set; }

       
        [Display(Name = "Manufacture Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public string Mfg { get; set; } 

        [Display(Name = "Expire Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public string Exp { get; set; }

        public int? PrintQuantity { get; set; }
    }

    public class BarcodePrintViewModel
    {
        public BarcodeViewModel ProductBarcode { get; set; }
        public int PrintQuantity { get; set; }

    }
    public class BarcodeGenerateViewModel
    {
        public List<BarcodeViewModel> ProductBarcodes { get; set; }
    } 
    public class ExistBarcodesPrintViewModel
    {
        public List<BarcodeViewModel> ProductBarcodes { get; set; }
    }
}