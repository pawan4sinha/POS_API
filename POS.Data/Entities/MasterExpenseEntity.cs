using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Data.Entities
{
    public class MasterExpenseEntity
    {
        
        public Guid Id { get; set; }
        
        public string Name { get; set; }

        
        public Guid ExpenseTypeId { get; set; }

        
        public string Description { get; set; }
    }

    public class MasterExpenseForCreateEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public Guid ExpenseTypeId { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
    }

    public class MasterExpenseForUpdateEntity
    {   
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public Guid ExpenseTypeId { get; set; }

        [StringLength(200)]
        public string Description { get; set; }
    }
}
