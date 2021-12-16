using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Models
{
    [Table("MasterExpense")]
    public partial class MasterExpense
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public Guid ExpenseTypeId { get; set; }

        [StringLength(200)]
        public string Description { get; set; }

        [ForeignKey(nameof(ExpenseTypeId))]
        public virtual MasterExpenseType ExpenseType { get; set; }
    }
}
