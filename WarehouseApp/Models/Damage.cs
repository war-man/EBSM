using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WarehouseApp.Models
{
     [Table("Damages")]
    public class Damage
    {
        [Key]
        public int DamageId { get; set; }

        public int StockId { get; set; }
        [ForeignKey("StockId")]
        public virtual Stock Stock { get; set; }

        [Display(Name = "Quantity")]
        public double Quantity { get; set; }
         [Display(Name="Note")]
         public string Note { get; set; }
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
        public virtual ICollection<DamageDismiss> DamageDismisses { get; set; }
        public virtual ICollection<DamageReturn> DamageReturns { get; set; }

    }

     [Table("DamageStocks")]
     public class DamageStock
     {
         [Key]
         public int DamageStockId { get; set; }
         public int StockId { get; set; }
         [ForeignKey("StockId")]
         public virtual Stock Stock { get; set; }

         [Display(Name = "Quantity")]
         public double? Quantity { get; set; }

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

     [Table("DamageReturns")]
     public class DamageReturn
     {
         [Key]
         public int DamageReturnId { get; set; }

         [Display(Name = "Dismiss Date")]
         [DataType(DataType.Date)]
         [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
         public DateTime ReturnDate { get; set; }

         public int? DamageId { get; set; }
         [ForeignKey("DamageId")]
         public virtual Damage Damage { get; set; }

         [Display(Name = "Quantity")]
         public double? Quantity { get; set; }

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

     [Table("DamageDismisses")]
     public class DamageDismiss
     {
         [Key]
         public int DamageDismissId { get; set; }
         [Display(Name = "Dismiss Date")]
         [DataType(DataType.Date)]
         [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
         public DateTime DismissDate { get; set; }
         public int? DamageId { get; set; }
         [ForeignKey("DamageId")]
         public virtual Damage Damage { get; set; }

         [Display(Name = "Quantity")]
         public double Quantity { get; set; }

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