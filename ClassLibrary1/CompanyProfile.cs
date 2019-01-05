using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EBSM.Entities
{
     [Table("CompanyProfiles")]
    public class CompanyProfile
    {
        [Key]
        public int CompanyId { get; set; }
         [Required]
        
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Address")]
        public string CompanyAddress { get; set; }

        [EmailAddress(ErrorMessage = "Please provide valid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Phone")]
        [Phone(ErrorMessage = "Phone number is not valid")]
        public string Phone { get; set; }

        [Display(Name = "TIN")]
        public string Tin { get; set; }
        [Display(Name = "Vat Reg No.")]
        public string VatRegNo { get; set; }
        [Display(Name = "Website")]
        public string WebSite { get; set; }
        public string CompanyLogo { get; set; }

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

    }
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : December 2016
//=======================================================================================//