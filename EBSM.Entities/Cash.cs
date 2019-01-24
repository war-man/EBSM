using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EBSM.Entities
{
    [Table("Cash")]
    public class Cash
    {
            [Key]
            public int CashId { get; set; }

            [Required]
            [Display(Name = "Cash")]
            public string CashName { get; set; }

            [Display(Name = "Balance")]
            public double? Balance { get; set; }

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
        
    }
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//