using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Entities
{
    public class MasterRacksEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }


        [Column(TypeName = "numeric(18, 0)")]
        public decimal NoOfShelves { get; set; }

        public Guid StoreDetailId { get; set; }

        public string Description { get; set; }

        public byte IsActive { get; set; }

    }

    public class MasterRacksForCreateEntity
    {
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
    }

    public class MasterRacksForUpdateEntity
    {
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
    }
}
