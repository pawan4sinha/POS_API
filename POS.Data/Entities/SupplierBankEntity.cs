using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Entities
{
    public class SupplierBankEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string AccountHolderName { get; set; }

        public string AccountNo { get; set; }

        public string BranchAddress { get; set; }

        public string IFSCode { get; set; }

        public Guid SupplierId { get; set; }
    }

    public class SupplierBankForCreateEntity
    {
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
    }

    public class SupplierBankForUpdateEntity
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
    }
}
