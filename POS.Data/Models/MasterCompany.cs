using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Models
{
    [Table("MasterCompany")]
    public partial class MasterCompany
    {
        
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [Required]
        [StringLength(200)]
        public string MailingName { get; set; }
        [Required]
        [StringLength(500)]
        public string Address { get; set; }

        [Required]
        public Guid CountryId { get; set; }

        [StringLength(50)]
        public string ContactNo { get; set; }
        [StringLength(200)]
        public string Email { get; set; }
        [StringLength(200)]
        public string Website { get; set; }
        [Column("GST")]
        [StringLength(200)]
        public string Gst { get; set; }
        [StringLength(200)]
        public string LicenseNo { get; set; }
        [Required]
        [StringLength(50)]
        public string CurrencyCode { get; set; }
        public byte ShowLogoInReceipts { get; set; }

        [Column(TypeName = "image")]
        public byte[] Logo { get; set; }

        [IgnoreDataMember]
        public Guid CreatedBy { get; set; }

        [IgnoreDataMember]
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        [IgnoreDataMember]
        public Guid? ModifyBy { get; set; }

        [IgnoreDataMember]
        [Column(TypeName = "datetime")]
        public DateTime? ModifyDate { get; set; }

        public byte SetDefault { get; set; }

        [ForeignKey(nameof(CountryId))]
        public virtual MasterCountry Country { get; set; }

        //[ForeignKey(nameof(CreatedBy))]
        //public virtual Accounts CreatedByNavigation { get; set; }
        
        //[ForeignKey(nameof(ModifyBy))]
        //public virtual Accounts ModifyByNavigation { get; set; }
    }
}
