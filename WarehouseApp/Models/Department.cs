using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WarehouseApp.Models
{
      [Table("Departments")]
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }
       [Required]
        [Display(Name="Department Name")]
        public string DepartmentName { get; set; }
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
       public virtual ICollection<DeptRoleConfig> DeptRoleConfigs { get; set; }
       public virtual ICollection<User> Users { get; set; }
    }
      [Table("DeptRoleConfigs")]
      public class DeptRoleConfig
      {
          [Key]
          public int DeptRoleConfigId { get; set; }

          public int DepartmentId { get; set; }
          [ForeignKey("DepartmentId")]
          public virtual Department Department { get; set; }

          public int RoleId { get; set; }
          [ForeignKey("RoleId")]
          public virtual Role Role { get; set; }

          public string DepartmentRole { get; set; }

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
      }
}