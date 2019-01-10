using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EBSM.Entities
{
    public class AuditLog
    {
        [Key]
        public Guid AuditLogId { get; set; }
        public string EventType { get; set; }
        [Required]
        public string TableName { get; set; }
        //[Required]
        //public string RecordId { get; set; }
        public string PrimaryKeyName { get; set; }
        public string PrimaryKeyValue { get; set; }
        //[Required]
        public string ColumnName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }

        public int? CreatedUser { get; set; }
        [ForeignKey("CreatedUser")]
        public virtual User User { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual CompanyProfile CompanyProfile { get; set; }
    }
}