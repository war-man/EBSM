using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WarehouseApp.Models
{
     [Table("Warehouses")]
    public class Warehouse
    {
        [Key]
         public int WarehouseId { get; set; }
        [Required]
        [Display(Name = "Warehouse Name")]
        public string WarehouseName { get; set; }
        [Display(Name = "Warehouse No")]
        public string WarehouseNo { get; set; }
        public bool IsDefault { get; set; }
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

        public virtual ICollection<WarehouseZone> WarehouseZones { get; set; }

    }

     [Table("WarehouseZones")]
     public class WarehouseZone
     {
         [Key]
         public int ZoneId { get; set; }
         [Required]
         [Remote("IsNameUsed", "WarehouseZone", AdditionalFields = "InitialZoneName", ErrorMessage = "Zone name already exist")]
         [Display(Name = "Zone Name")]
         public string ZoneName { get; set; }
         [Display(Name = "Capacity")]
         public double? Capacity { get; set; }

         public int? WarehouseId { get; set; }
         [ForeignKey("WarehouseId")]
         public virtual Warehouse Warehouse { get; set; }

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
         public virtual ICollection<Product> Products { get; set; }
         public virtual ICollection<StockWarehouseRelation> StockWarehouseRelations { get; set; }

     }

     [Table("WarehouseRows")]
     public class WarehouseRow
     {
         [Key]
         public int WarehouseRowId { get; set; }
         [Required]
         [Display(Name = "Row")]
         public string RowName { get; set; }
         [Display(Name = "Capacity")]
         public double? Capacity { get; set; }

         public int? ZoneId { get; set; }
         [ForeignKey("ZoneId")]
         public virtual WarehouseZone WarehouseZone { get; set; }
         public int? WarehouseId { get; set; }
         [ForeignKey("WarehouseId")]
         public virtual Warehouse Warehouse { get; set; }

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

     [Table("WarehouseTracks")]
     public class WarehouseTrack
     {
         [Key]
         public int TrackId { get; set; }
         [Required]
         [Display(Name = "Track")]
         public string TrackName { get; set; }
         [Display(Name = "Capacity")]
         public double? Capacity { get; set; }
         public int? ZoneId { get; set; }
         [ForeignKey("ZoneId")]
         public virtual WarehouseZone WarehouseZone { get; set; }
         public int? WarehouseId { get; set; }
         [ForeignKey("WarehouseId")]
         public virtual Warehouse Warehouse { get; set; }

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