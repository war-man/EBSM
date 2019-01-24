using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EBSM.Entities
{
    [Table("TransferProducts")]
    public class TransferProduct
    {
        [Key]
        public int ProductTransferId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime TransferDate { get; set; }
         [Display(Name = "Product")]
        public int StockId { get; set; }
        [ForeignKey("StockId")]
        public virtual Stock Stock { get; set; }
        [Display(Name = "Zone From")]
        public int ZoneFromId { get; set; }
        [ForeignKey("ZoneFromId")]
        public virtual WarehouseZone ZoneFrom { get; set; }
        [Display(Name = "Zone To")]
        public int ZoneToId { get; set; }
        [ForeignKey("ZoneToId")]
        public virtual WarehouseZone ZoneTo { get; set; }
        [Display(Name = "Quantity")]
        public double TransferQuantity { get; set; }
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
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//