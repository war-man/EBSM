using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace WarehouseApp.Models
{
     [Table("Customers")]
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        [EmailAddress(ErrorMessage = "Please provide valid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Contact No")]
        public string ContactNo { get; set; }
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        [Display(Name = "Age")]
        public int? Age { get; set; }
        [Display(Name = "Previous Balance")]
        public double? PreviousBalance { get; set; }
        public byte? Status { get; set; }
        public int? CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual User CreateUser { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        [ForeignKey("UpdatedBy")]
        public virtual User UpdateUser { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? UpdatedDate { get; set; }
        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual CompanyProfile CompanyProfile { get; set; }
    
        public virtual ICollection<CustomerProject> CustomerBranches { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }

    }
     [Table("CustomerProjects")]
     public class CustomerProject
     {
         [Key]
         public int CustomerProjectId { get; set; }
         [Required]
         [Display(Name = "Branch Name")]
         [StringLength(80)]
         public string ProjectName { get; set; }

         [Display(Name = "Phone No")]
         [StringLength(20)]
         public string ProjectPhoneNo { get; set; }
         [Display(Name = "Address")]
         [StringLength(255)]
         public string ProjectAddress { get; set; }
         [Display(Name = "Description")]
         public string ProjectDescription { get; set; }

         public int CustomerId { get; set; }
         [ForeignKey("CustomerId")]
         public virtual Customer Customer { get; set; }
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


     }

     [Table("CustomerBalances")]
     public class CustomerBalance
     {
         [Key]
         public int CustomerBalanceId { get; set; }
         public int? DepartmentId { get; set; }
         [ForeignKey("DepartmentId")]
         public virtual Department Department { get; set; }
         public int? CustomerId { get; set; }
         [ForeignKey("CustomerId")]
         public virtual Customer Customer { get; set; }

         public double? Balance { get; set; }
         public byte? Status { get; set; }

         public int? CreatedBy { get; set; }
         [ForeignKey("CreatedBy")]
         public virtual User CreateUser { get; set; }
         [DataType(DataType.Date)]
         [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
         public DateTime? CreatedDate { get; set; }
         public int? UpdatedBy { get; set; }
         [ForeignKey("UpdatedBy")]
         public virtual User UpdateUser { get; set; }
         [DataType(DataType.Date)]
         [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
         public DateTime? UpdatedDate { get; set; }
         public int? CompanyId { get; set; }
         [ForeignKey("CompanyId")]
         public virtual CompanyProfile CompanyProfile { get; set; }
     }
}