using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EBSM.Entities
{
     [Table("Bills")]
    public class Bill
    {
        [Key]
        public int BillId { get; set; }
        [Display(Name = "Bill No")]
        [StringLength(80)]
        public string BillNo { get; set; }
          [Display(Name = "Bill Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? BillDate { get; set; }
         [Display(Name = "Bill Amount")]
         public double BillAmount { get; set; }
         [Display(Name = "Bill Paid")]
         public double? BillPaid { get; set; }
           [Display(Name = "Project")]
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

        public virtual ICollection<InvoiceBill> InvoiceBills { get; set; }

    }
     [Table("InvoiceBills")]
     public class InvoiceBill
     {

         // public int InvoiceBillId { get; set; }
         [Key, Column(Order = 1)]

         public int BillId { get; set; }
         [ForeignKey("BillId")]
         public virtual Bill Bill { get; set; }

         [Key, Column(Order = 2)]
         public int InvoiceId { get; set; }
         [ForeignKey("InvoiceId")]
         public virtual Invoice Invoice { get; set; }
         public int? CreatedBy { get; set; }
         [ForeignKey("CreatedBy")]
         public virtual User CreateUser { get; set; }
         [DataType(DataType.Date)]
         [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
         public DateTime? CreatedDate { get; set; }

     }
}