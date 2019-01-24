using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using EBSM.Entities;

namespace WarehouseApp.Models.ViewModels
{
    public class DamageProductsViewModel
    {
        public List<DamageViewModel> DamageProducts { get; set; }
    }
    [NotMapped]
    public class DamageViewModel:Damage
    {
        public int? DefaultZoneId { get; set; }
    }
    [NotMapped]
    public class DamageDismissViewModel:DamageDismiss
    {
        public new int? DamageDismissId { get; set; }
        [Display(Name = "Dismiss Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public string ActionDate { get; set; }

        public string ActionType { get; set; }

        public int? StockId { get; set; }
        [ForeignKey("StockId")]
        public virtual Stock Stock { get; set; }

        public int? ZoneId { get; set; }

    }
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//