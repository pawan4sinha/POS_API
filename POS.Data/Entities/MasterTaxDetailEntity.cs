using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Entities
{
    public class MasterTaxDetailEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid TaxTypeId { get; set; }

        public decimal TaxRate { get; set; }

        public string Description { get; set; }
    }

    public class MasterTaxDetailForCreateEntity
    {
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public Guid TaxTypeId { get; set; }

        //[Column(TypeName = "numeric(18, 2)")]
        public decimal TaxRate { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
    }

    public class MasterTaxDetailForUpdateEntity
    {
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public Guid TaxTypeId { get; set; }

        //[Column(TypeName = "numeric(18, 2)")]
        public decimal TaxRate { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
    }
}
