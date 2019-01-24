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
    [NotMapped]
    public class AttributeSetViewModel:ProductAttributeSet
    {
        [Required]
        [Remote("IsAttributeSetNameUsed", "ProductAttribute", AdditionalFields = "InitialAttributeSetName", ErrorMessage = "Attribute Set name already exist")]

        [Display(Name = "Attribute Set Name")]
        public new string AttributeSetName { get; set; }

        public int[] AttributeIds {get; set; }
       public List<ProductAttribute> SelectedAttribteList{ get; set; }

       // public List<AttributeCheckBoxViewModel> AttributeSelectedCheckbox{ get; set; }
    }

    public class AttributeCheckBoxViewModel
    {
        public int AttributeId { get; set; }
        [Display(Name = "Attribute Name")]
        public string AttributeName { get; set; }
        public bool IsChecked { get; set; }
    }

    [NotMapped]
    public class AttributeViewModel:ProductAttribute
    {
        [Required]
        [Remote("IsNameUsed", "ProductAttribute", AdditionalFields = "InitialAttributeName", ErrorMessage = "Attribute name already exist")]
        [Display(Name = "Attribute Name")]
        public new string AttributeName { get; set; }

        public new List<AttributeValue> Values { get; set; }

    }

    public class AttributeValue
    {
        [Display(Name = "Value")]
        public string Value { get; set; }

        [Display(Name = "Default")]
        public bool IsDefault{ get; set; }
    }
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//