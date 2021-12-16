using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Models
{

    [Table("MasterProduct")]
    [Index(nameof(Barcode), Name = "IX_Product_Barcode", IsUnique = true)]
    [Index(nameof(Code), Name = "IX_Product_Code", IsUnique = true)]
    public class MasterProduct
    {
        [Key]
        public Guid Id { get; set; }

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


        [ForeignKey(nameof(ManufacturerId))]
        public virtual MasterManufacturers Manufacturers { get; set; }

        [ForeignKey(nameof(SubCategoryId))]
        public virtual ProductSubCategory SubCategory { get; set; }

        [ForeignKey(nameof(TaxDetailId))]
        public virtual MasterTaxDetail TaxDetail { get; set; }

        [ForeignKey(nameof(UnitId))]
        public virtual MasterUnit Unit { get; set; }
    }
}
