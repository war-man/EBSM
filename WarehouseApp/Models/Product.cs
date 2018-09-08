using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WarehouseApp.Models
{
    [Table("Products")]
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [Display(Name="Product Code")]
        public string ProductCode { get; set; }
        [Required]
        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Display(Name = "Product Full Name")]
        public string ProductFullName { get; set; }

        [AllowHtml]
        [Display(Name="Description")]
        public string Details { get; set; }


        [Display(Name = "Purchase Price")]
        public double? Tp { get; set; }


        [Display(Name = "Retail Price")]
        public double? Dp { get; set; }

        [Display(Name = "Per Carton")]
        public int? PerCarton { get; set; }

        [Display(Name = "Discount Type")]
        public string DiscountType { get; set; }
        [Display(Name = "Discount Amount")]
        public double? DiscountAmount { get; set; }


        [Display(Name = "VAT")]
        public double? Vat { get; set; }
        [Display(Name = "Expiry Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? ExpiryDate { get; set; }
        [Display(Name = "Minimum Stock for Warning")]
        public double? MinStockLimit { get; set; }
        public string ProductImage { get; set; }
        public byte? Status { get; set; }
        public int? AttributeSetId { get; set; }
        [ForeignKey("AttributeSetId")]
        public virtual ProductAttributeSet AttributeSet { get; set; }
        public int? GroupNameId { get; set; }
        [ForeignKey("GroupNameId")]
        public virtual Group Group { get; set; }

        [Display(Name = "Manufacturer")]
        public int? ManufacturerId { get; set; }
        [ForeignKey("ManufacturerId")]
        public virtual Supplier Supplier { get; set; } 

        [Display(Name = "Default Zone")]
        public int? DefaultZoneId { get; set; }
        [ForeignKey("DefaultZoneId")]
        public virtual WarehouseZone WarehouseZone { get; set; } 

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
    
        public virtual ICollection<Damage> Damages { get; set; }
        public virtual ICollection<DamageStock> DamageStocks { get; set; }
        public virtual ICollection<InvoiceProduct> InvoiceProducts { get; set; }
        public virtual ICollection<OrderProduct> OrderProducts { get; set; }
        public virtual ICollection<PurchaseProduct> PurchaseProducts { get; set; }
        public virtual ICollection<ReturnProduct> ReturnProducts { get; set; }
        public virtual ICollection<Stock> Stocks { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
        public virtual ICollection<ProductAttributeRelation> ProductAttributeRelations { get; set; }
        public virtual ICollection<ProductCustomerRelation> CustomerOptions { get; set; }
    }
    [Table("ProductAttributeRelations")]
    public class ProductAttributeRelation
    {
        [Key]
        public int ProductAttributeRelationId { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public int AttributeId { get; set; }
        [ForeignKey("AttributeId")]
        public virtual ProductAttribute Attribute { get; set; }

        [Display(Name = "Value")]
        public string Value { get; set; }
    }
    [Table("ProductCategories")]
    public class ProductCategory
    {
        [Key]
        public int ProductCategoryId { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

    }


    [Table("ProductCustomerRelations")]
    public class ProductCustomerRelation
    {
        [Key]
        public int ProductCustomerRelationId { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }

        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        [Display(Name = "Product Description")]
        public string ProductDescription { get; set; }
        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }
        [Display(Name = "Unit Price")]
        public double? UnitPrice { get; set; }
        [Display(Name = "MRP")]
        public double? Mrp { get; set; }


    }
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//