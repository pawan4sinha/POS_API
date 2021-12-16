using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Entities
{
    public class MasterManufacturersEntity
    {

        public Guid Id { get; set; }

        public string Name { get; set; }

        public int SortOrder { get; set; }

        public byte IsActive { get; set; }

        public string DisplayName { get; set; }
    }


    public class MasterManufacturersForCreateEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }


        public int SortOrder { get; set; }

        [Required]
        public byte IsActive { get; set; }

        [StringLength(100)]
        public string DisplayName { get; set; }
    }

    public class MasterManufacturersForUpdateEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public int SortOrder { get; set; }

        [Required]
        public byte IsActive { get; set; }

        [StringLength(100)]
        public string DisplayName { get; set; }
    }
}
