using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace POS.Data.Models
{
    [Table("ProductCategory")]

    public class ProductCategory
    {
        [Key]
        [StringLength(50)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public int SortOrder { get; set; }
          
        public byte IsActive { get; set; }

    }
}
