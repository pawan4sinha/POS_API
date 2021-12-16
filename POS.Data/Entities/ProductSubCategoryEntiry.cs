using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace POS.Data.Entities
{
    public class ProductSubCategoryEntity
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
                
        public Guid CategoryId { get; set; }

        public int SortOrder { get; set; }

        public byte IsActive { get; set; }
    }

    public class ProductSubCategoryForCreateEntity
    {

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        //[StringLength(50)]
        public Guid CategoryId { get; set; }

        public int SortOrder { get; set; }

        [Required]
        public byte IsActive { get; set; }
    }

    public class ProductSubCategoryForUpdateEntity
    {

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        //[StringLength(50)]
        public Guid CategoryId { get; set; }

        public int SortOrder { get; set; }

        [Required]
        public byte IsActive { get; set; }
    }
}
