using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace EBSM.Entities
{
    public class ProductImage
    {
        [Key]
        public Guid ProductImageFileId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual CompanyProfile CompanyProfile { get; set; }
    }

    public class CompanyLogo
    {
        [Key]
        public Guid CompanyLogoId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public int CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual CompanyProfile CompanyProfile { get; set; }

    }

    public class NoticeAttachment
    {
        [Key]
        public Guid NoticeAttachmentId { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public int NoticeId { get; set; }
        [ForeignKey("NoticeId")]
        public virtual Notice Notice { get; set; }
        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual CompanyProfile CompanyProfile { get; set; }

    }
}

//=======================================================================================//
//Author : Md. Mahid Choudhury
//Creation Date : January 2017
//=======================================================================================//