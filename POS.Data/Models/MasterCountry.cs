using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace POS.Data.Models
{
    [Table("MasterCountry")]
    public partial class MasterCountry
    {

        [Key]        
        public Guid Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public byte IsActive { get; set; }
        public int SortOrder { get; set; }

    }
}
