using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace EBSM.Entities
{
     [Table("Categories")]
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
         [Required]
        [Display(Name="Category")]
        //[Remote("IsNameUsed", "Category", AdditionalFields = "InitialCategoryName", ErrorMessage = "Category name already exist")]
        public string CategoryName { get; set; }

          [Display(Name = "Category Parent")]
        public int? CategoryParentId { get; set; }
         [ForeignKey("CategoryParentId")]
         public virtual Category CategoryParent { get; set; }

          [Display(Name = "Count")]
        public int? Count { get; set; }

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
        public virtual ICollection<Category> ChildCategories { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
        //public virtual ICollection<DamageStock> DamageStocks { get; set; }
        //public virtual ICollection<InvoiceProduct> InvoiceProducts { get; set; }
        //public virtual ICollection<PurchaseProduct> PurchaseProducts { get; set; }
        //public virtual ICollection<Stock> Stocks { get; set; }
    }
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//