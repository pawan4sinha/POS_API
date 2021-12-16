using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Models
{
    [Table("MasterStoreDetail")]
    public class MasterStoreDetail
    {
        [Key]
        public Guid Id { get; set; }

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

        [ForeignKey(nameof(StoreTypeId))]
        public virtual MasterStoreType StoreType{ get; set; }
    }
}
