using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EBSM.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Display(Name="Address")]
        public string Address { get; set; }
        [EmailAddress(ErrorMessage = "Please provide valid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name="Contact No")]
        public string ContactNo { get; set; }
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        [Display(Name="National ID")]
        public string NationalId { get; set; }
        //public int DepartmentId { get; set; }
        //[ForeignKey("DepartmentId")]
        //public virtual Department Department { get; set; }

        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
        [Required(ErrorMessage = "User Name field is required")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }


        [Display(Name = "Lasr Password Change Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? LastPassChangeDate { get; set; }
        public int? PasswordChangedCount { get; set; }

        public bool SupUser { get; set; }
        public byte? Status { get; set; }


        //public string CreatedBy { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime? CreatedDate { get; set; }
        //public string UpdatedBy { get; set; }
        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        //public DateTime? UpdatedDate { get; set; }

        public int? CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual User CreatedUser { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedDate { get; set; }

        public string UpdatedBy { get; set; }
        //[ForeignKey("UpdatedBy")]
        //public virtual User UpdateUser { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? UpdatedDate { get; set; }

        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual CompanyProfile CompanyProfile { get; set; }
        
    }


    public class Logins
    {
        [Key]
        public int LoginsId { get; set; }
        public string UserId { get; set; }
        public string SessionId { get; set; }
        public bool LoggedIn { get; set; }
        public DateTime? LoggedInDateTime { get; set; }
    }
}