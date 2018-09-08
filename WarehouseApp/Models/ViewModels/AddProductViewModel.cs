using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WarehouseApp.Models.ViewModels
{
    [NotMapped]
    public class AddProductViewModel:Product
    {

        public new int? ProductId { get; set; }
       public new int AttributeSetId { get; set; }
        [ForeignKey("AttributeSetId")]
       public new virtual ProductAttributeSet AttributeSet { get; set; }
        [Display(Name = "Product Code")]
        [Remote("IsProductCodeExist", "Product", AdditionalFields = "InitialProductCode", ErrorMessage = "Product code already exist")]
        public new string ProductCode { get; set; }
        [Display(Name = "Expiry Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public new string ExpiryDate { get; set; }
        [Required]
        public new int DefaultZoneId { get; set; }
        [ForeignKey("DefaultZoneId")]
        public new virtual WarehouseZone WarehouseZone { get; set; } 
        public List<ProductAttributeViewModel> ProductAtts { get; set; }
        public List<int> CategoryIds { get; set; }
        public List<ProductCustomerOption> CustomerOptionList { get; set; }
        public new virtual ICollection<PurchaseProduct> PurchaseProducts { get; set; }
    }


    public class ProductAttributeViewModel
    {
        public int AttributeId { get; set; }
       
        public string Value { get; set; }
        //public int? SortOrder { get; set; }
      
        public List<ProdAttCheckBoxViewModel> ProdAttCheckboxes { get; set; }

    }

    public class ProductAttributeValueViewModel

    {
        public ProductAttribute Attribute { get; set; }
        public string Value { get; set; }
    }

    public class ProdAttCheckBoxViewModel
    {
        public string Value { get; set; }
        public bool IsChecked { get; set; }
    }
    [NotMapped]
    public class ProductCustomerOption:ProductCustomerRelation
    {

        public new int? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public new virtual Customer Customer { get; set; }

        
    }
    public class ProductImportViewModel
    {
       
        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        public string GroupName { get; set; }
        public int? GroupNameId { get; set; }
      

        [Display(Name = "Purchase Price")]
        public double? Tp { get; set; }


        [Display(Name = "Retail Price")]
        public double? Dp { get; set; }

        public double? RetailPrice { get; set; }

        [Display(Name = "Minimum Stock for Warning")]
        public double? MinimumStockWarning { get; set; }

        public string ManufacturerName { get; set; }
        [Display(Name = "Manufacturer")]
        public int? ManufacturerId { get; set; }
       
        public string Category { get; set; }
        public int? CategoryId { get; set; }

        public string AttributeValue { get; set; }
        public string UoM { get; set; }
        public string DefaultStockZone { get; set; }
        public int? DefaultStockZoneId { get; set; }

        //Swapno
        public string SwapnoCode { get; set; }
        public string SwapnoItemDescription { get; set; }
        public double? SwapnoRetailPrice { get; set; }
        public double? SwapnoMrp { get; set; }

        //Agora
        public string AgoraCode { get; set; }
        public string AgoraItemDescription { get; set; }
        public double? AgoraRetailPrice { get; set; }
        public double? AgoraMrp { get; set; }

        //Nandan
        public string NandanCode { get; set; }
        public string NandanItemDescription { get; set; }
        public double? NandanRetailPrice { get; set; }
        public double? NandanMrp { get; set; }

        //csd Super shop
        public string CsdSupperShopCode { get; set; }
        public string CsdSupperShopItemDescription { get; set; }
        public double? CsdSupperShopRetailPrice { get; set; }
        public double? CsdSupperShopMrp { get; set; }

        //csd Excluesive
        public string CsdExclusiveShopCode { get; set; }
        public string CsdExclusiveShopItemDescription { get; set; }
        public double? CsdExclusiveShopRetailPrice { get; set; }
        public double? CsdExclusiveShopMrp { get; set; }

        //Happy Mart
        public string HappyMartCode { get; set; }
        public string HappyMartItemDescription { get; set; }
        public double? HappyMartMrp { get; set; }
        public double? HappyMartRetailPrice { get; set; }

        //one Stop
        public string OneStopCode { get; set; }
        public string OneStopItemDescription { get; set; }
        public double? OneStopMrp { get; set; }
        public double? OneStopRetailPrice { get; set; }

    }



}

//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//