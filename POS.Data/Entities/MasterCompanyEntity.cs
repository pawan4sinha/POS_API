using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Entities
{
    public class MasterCompanyEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string MailingName { get; set; }

        public string Address { get; set; }
        public Guid CountryId { get; set; }

        public string ContactNo { get; set; }

        public string Email { get; set; }

        public string Website { get; set; }

        public string Gst { get; set; }

        public string LicenseNo { get; set; }

        public string CurrencyCode { get; set; }

        public byte ShowLogoInReceipts { get; set; }

        public byte[] Logo { get; set; }

        public byte SetDefault { get; set; }

    }

    public class CompanyForCreateEntity : BaseEntity
    {
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

        [StringLength(200)]
        public string GST { get; set; }

        [StringLength(200)]
        public string LicenseNo { get; set; }

        [Required]
        [StringLength(50)]
        public string CurrencyCode { get; set; }

        [Required]
        public byte ShowLogoInReceipts { get; set; }

        public byte[] Logo { get; set; }

        public byte SetDefault { get; set; }

    }

    public class CompanyForUpdateEntity : BaseEntity
    {
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

        [StringLength(200)]
        public string GST { get; set; }

        [StringLength(200)]
        public string LicenseNo { get; set; }

        [Required]
        [StringLength(50)]
        public string CurrencyCode { get; set; }

        [Required]
        public byte ShowLogoInReceipts { get; set; }

        public byte[] Logo { get; set; }

        public byte SetDefault { get; set; }
    }
}
