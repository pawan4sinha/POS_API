using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Models
{
    [Table("MasterRacks")]
    public class MasterRacks
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "numeric(18, 0)")]
        public decimal NoOfShelves { get; set; }

        public Guid StoreDetailId { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public byte IsActive { get; set; }

        [ForeignKey(nameof(StoreDetailId))]
        public virtual MasterStoreDetail StoreDetail { get; set; }
    }
}
