using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EBSM.Entities
{
     [Table("Settings")]
    public class Settings
    {
        [Key]
         public int SettingsId { get; set; }
        [Display(Name = "Returns Day")]
        public int? ReturnsDayAllow { get; set; }

        [Display(Name = "Printing On/Off")]
        public bool  Printing{ get; set; }


        [Display(Name = "Printer Name")]

        public string PrinterName { get; set; }

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

    }
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : April 2016
//=======================================================================================//