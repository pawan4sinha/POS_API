using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Models
{
    [Table("MasterRoles")]
    public partial class MasterRole
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public byte IsActive { get; set; }

        #region IgnoreDataMember

        [IgnoreDataMember]
        public Guid CreatedBy { get; set; }

        [IgnoreDataMember]
        [Column(TypeName = "datetime")]
        public DateTime CreatedDate { get; set; }

        [IgnoreDataMember]
        public Guid? ModifiedBy { get; set; }

        [IgnoreDataMember]
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }

        #endregion
    }
}
