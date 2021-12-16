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
    [Table("Accounts")]
    public class Accounts
    {
        [Key]
        [StringLength(50)]
        public Guid Id { get; set; }

        [Required]
        public string Username { get; set; }

        [IgnoreDataMember]
        [Required]
        public string Password { get; set; }
        
        [Required]
        public byte IsActive { get; set; }

        [IgnoreDataMember]
        [StringLength(50)]
        public Guid CreatedBy { get; set; }

        [IgnoreDataMember]
        [Required]
        public DateTime CreatedDate { get; set; }

        [IgnoreDataMember]
        [StringLength(50)]
        public Guid? ModifyBy { get; set; }

        [IgnoreDataMember]
        public DateTime? ModifyDate { get; set; }

        [Required]
        public Guid RoleId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Email { get; set; }
        
        public string Address { get; set; }

        [Required]
        public Guid CountryId { get; set; }

        [Required]
        public string ContactNo { get; set; }

        [IgnoreDataMember]
        [Required]
        public string Salt { get; set; }

        public byte[] ProfilePicture { get; set; }


        [ForeignKey(nameof(RoleId))]
        public virtual MasterRole Role { get; set; }

        [ForeignKey(nameof(CountryId))]
        public virtual MasterCountry Country { get; set; }
    }
}

