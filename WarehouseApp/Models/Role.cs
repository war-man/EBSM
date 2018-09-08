using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WarehouseApp.Models
{
     [Table("Roles")]
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
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

        public virtual ICollection<RoleTask> RoleTasks { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }

     [Table("RoleTasks")]
     public class RoleTask
     {
         [Key]
         public int RoleTaskId { get; set; }

         public int RoleId { get; set; }
         [ForeignKey("RoleId")]
         public virtual Role Role { get; set; }
         public string Task { get; set; }

         public int? CreatedBy { get; set; }
         [ForeignKey("CreatedBy")]
         public virtual User CreateUser { get; set; }
         [DataType(DataType.Date)]
         [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
         public DateTime? CreatedDate { get; set; }

     }
}