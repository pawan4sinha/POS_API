using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Entities
{
    public class MasterRoleEntity
    {
        public Guid Id { get; set; }
                
        public string Name { get; set; }
                
        public string Description { get; set; }

        public byte IsActive { get; set; }
    }

    public class MasterRoleForCreateEntity : BaseEntity
    {
        
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public byte IsActive { get; set; }
    }

    public class MasterRoleForUpdateEntity : BaseEntity
    {

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public byte IsActive { get; set; }
    }
}
