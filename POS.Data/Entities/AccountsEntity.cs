using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Entities
{
    public class AccountsEntity
    {
        public Guid Id { get; set; }


        public string Username { get; set; }


        public string Password { get; set; }


        public byte IsActive { get; set; }

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

        public byte[] ProfilePicture { get; set; }
    }

    public class AccountsForCreateEntity : BaseEntity
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public byte IsActive { get; set; }

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

        public byte[] ProfilePicture { get; set; }
    }

    public class AccountsForLoginEntity : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }
    }

    public class AccountsForUpdateEntity : BaseEntity
    {
        [Required]
        public byte IsActive { get; set; }

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

        public byte[] ProfilePicture { get; set; }
    }
}
