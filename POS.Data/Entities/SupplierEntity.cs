using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Entities
{
    public class SupplierEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Code { get; set; }

        public string Address { get; set; }

        public string ContactNo { get; set; }

        public string Email { get; set; }

        public string GST { get; set; }

        public string Remarks { get; set; }
    }

    public class SupplierForCreateEntity: BaseEntity
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        public int Code { get; set; }

        [Required]
        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(200)]
        public string ContactNo { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(200)]
        public string GST { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }
    }

    public class SupplierForUpdateEntity : BaseEntity
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        public int Code { get; set; }

        [Required]
        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(200)]
        public string ContactNo { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(200)]
        public string GST { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }
    }
}
