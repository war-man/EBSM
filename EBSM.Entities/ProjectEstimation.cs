using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EBSM.Entities
{
    [Table("ProjectEstimations")]
    public class ProjectEstimation
    {
        [Key]
        public int ProjectEstimationId { get; set; }

        [Display(Name = "Order Number")]
        public string EstimationNumber { get; set; }

        [Display(Name = "Order Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EstimationDate { get; set; }
        [Display(Name = "Project")]
        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        [Display(Name = "Project Branch")]
        public int? CustomerBranchId { get; set; }
        [ForeignKey("CustomerBranchId")]
        public virtual CustomerProject CustomerBranch { get; set; }

        [Display(Name = "Total Price")]
        public double? TotalPrice { get; set; }

        [Display(Name = "Total Quantity")]
        public double? TotalQuantity { get; set; }

        [Display(Name = "Note")]
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
        public virtual ICollection<EstimationProductRelation> EstimationProductRelationCollection { get; set; }

    }

    [Table("EstimationProductRelation")]
    public class EstimationProductRelation
    {
        [Key]
        public int EstimationProductId { get; set; }
        public int ProjectEstimationId { get; set; }
        [ForeignKey("ProjectEstimationId")]
        public virtual ProjectEstimation ProjectEstimation { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [Display(Name = "Batch No")]
        public string BatchNo { get; set; }
        [Display(Name = "Barcode")]
        public string Barcode { get; set; }

        [Display(Name = "Distribution Price")]
        public double Dp { get; set; }

        [Display(Name = "Quantity")]
        public double Quantity { get; set; }

        [Display(Name = "Total Price")]
        public double? TotalPrice { get; set; }

        public byte? Status { get; set; }
        [Display(Name = "Zone")]
        public int? ZoneId { get; set; }
        [ForeignKey("ZoneId")]
        public virtual WarehouseZone WarehouseZone { get; set; }
        public int? CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual User CreateUser { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? CreatedDate { get; set; }
        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual CompanyProfile CompanyProfile { get; set; }

    }
}
//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//