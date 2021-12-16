using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Models
{
    [Table("MasterTaxDetail")]
    public class MasterTaxDetail
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public Guid TaxTypeId { get; set; }

        [Column(TypeName = "numeric(18, 2)")]
        public decimal TaxRate { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [ForeignKey(nameof(TaxTypeId))]
        public virtual MasterTaxType TaxType { get; set; }
    }
}
