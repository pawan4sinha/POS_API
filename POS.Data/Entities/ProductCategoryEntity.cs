using System;
using System.ComponentModel.DataAnnotations;

namespace POS.Data.Entities
{
    public class ProductCategoryEntity
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public int SortOrder { get; set; }

        public byte IsActive { get; set; }
    }

    public class ProductCategoryForCreateEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        

        public int SortOrder { get; set; }

        [Required]
        public byte IsActive { get; set; }
    }

    public class ProductCategoryForUpdateEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public int SortOrder { get; set; }
        
        [Required]
        public byte IsActive { get; set; }
    }
}
