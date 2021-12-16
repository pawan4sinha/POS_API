using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Models
{
    [Table("MasterManufacturers")]
    public class MasterManufacturers
    {
        [Key]
        [StringLength(50)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public int SortOrder { get; set; }

        public byte IsActive { get; set; }

        [StringLength(100)]
        public string DisplayName { get; set; }
    }
}
