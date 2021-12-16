using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Models
{
    [Table("SupplierBank")]
    public class SupplierBank
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string AccountHolderName { get; set; }

        [Required]
        [StringLength(100)]
        public string AccountNo { get; set; }

        [StringLength(100)]
        public string BranchAddress { get; set; }

        [Required]
        [StringLength(50)]
        public string IFSCode { get; set; }

        public Guid SupplierId { get; set; }

        [ForeignKey(nameof(SupplierId))]
        public virtual Supplier Supplier { get; set; }
    }
}
