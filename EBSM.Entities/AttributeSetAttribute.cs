using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace EBSM.Entities
{
    [Table("ProductAttributes")]
    public class ProductAttribute
    {
        [Key]
        public int AttributeId { get; set; }
        [Required]
       // [Remote("IsNameUsed", "ProductAttribute", AdditionalFields = "InitialAttributeName", ErrorMessage = "Attribute name already exist")]
        [Display(Name = "Attribute Name")]
        public string AttributeName { get; set; }
        [Required]
        [Display(Name = "Attribute Type")]
        public string AttributeType { get; set; }

        [Display(Name = "Values")]
        public string Values { get; set; }
        [Display(Name = "Showing Text")]
        public string ShowingText { get; set; }
        [Display(Name = "Default Value")]
        public string DefaultValue { get; set; }
        [Display(Name = "Required")]
        public bool IsRequired { get; set; }
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
        public virtual ICollection<AttributeSetAttribute> AttributeSetAttributes { get; set; }
        public virtual ICollection<ProductAttributeRelation> ProductAttributeRelations { get; set; }

    }

    [Table("ProductAttributeSets")]
    public class ProductAttributeSet
    {
        [Key]
        public int AttributeSetId { get; set; }
        [Required]

        [Display(Name = "Attribute Set Name")]
        public string AttributeSetName { get; set; }

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
        public virtual ICollection<AttributeSetAttribute> AttributeSetAttributes { get; set; }
        public virtual ICollection<Product> Products { get; set; }


    }
     [Table("AttributeSetAttributes")]
    public class AttributeSetAttribute
    {
        [Key]
        public int AttributeSetAttributeId { get; set; }
        public int AttributeSetId { get; set; }
        [ForeignKey("AttributeSetId")]
        public virtual ProductAttributeSet AttributeSet { get; set; }
        public int AttributeId { get; set; }
        [ForeignKey("AttributeId")]
        public virtual ProductAttribute Attribute { get; set; }
    }
}

//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//