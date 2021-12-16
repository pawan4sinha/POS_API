using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Entities
{
    public class MasterStoreDetailEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid StoreTypeId { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public DateTime? ActiveDate { get; set; }

        public DateTime? InActiveDate { get; set; }

        public byte IsActive { get; set; }
    }

    public class MasterStoreDetailForCreateEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public Guid StoreTypeId { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        public DateTime? ActiveDate { get; set; }

        public DateTime? InActiveDate { get; set; }

        [Required]
        public byte IsActive { get; set; }
    }

    public class MasterStoreDetailForUpdateEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public Guid StoreTypeId { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(50)]
        public string City { get; set; }

        public DateTime? ActiveDate { get; set; }

        public DateTime? InActiveDate { get; set; }

        [Required]
        public byte IsActive { get; set; }
    }
}
