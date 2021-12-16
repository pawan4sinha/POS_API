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
    [Table("Supplier")]
    public class Supplier
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [Required]
        public int Code { get; set; }

        [Required]
        [StringLength(500)]
        public string Address { get; set; }

        [StringLength(200)]
        public string ContactNo { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(200)]
        public string GST { get; set; }

        [StringLength(500)]
        public string Remarks { get; set; }

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
