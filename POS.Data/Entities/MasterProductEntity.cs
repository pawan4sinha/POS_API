using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Entities
{
    public class MasterProductEntity
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Code { get; set; }

        public Guid? ManufacturerId { get; set; }

        public Guid SubCategoryId { get; set; }

        public Guid? TaxDetailId { get; set; }

        public string Barcode { get; set; }

        public string Description { get; set; }

        public Guid UnitId { get; set; }

        public byte[] ProductImage { get; set; }
    }

    public class MasterProductForCreateEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int Code { get; set; }

        public Guid? ManufacturerId { get; set; }

        public Guid SubCategoryId { get; set; }

        public Guid? TaxDetailId { get; set; }

        [Required]
        [StringLength(50)]
        public string Barcode { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public Guid UnitId { get; set; }

        public byte[] ProductImage { get; set; }
    }

    public class MasterProductForUpdateEntity
    {

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int Code { get; set; }

        public Guid? ManufacturerId { get; set; }

        public Guid SubCategoryId { get; set; }

        public Guid? TaxDetailId { get; set; }

        [Required]
        [StringLength(50)]
        public string Barcode { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        public Guid UnitId { get; set; }

        public byte[] ProductImage { get; set; }
    }

    public class MasterProductForSearch
    {
        public string Name { get; set; }

        public int? Code { get; set; }

        public string Barcode { get; set; }

        public Guid? CategoryId { get; set; }

        public Guid? SubCategoryId { get; set; }
    }
}
