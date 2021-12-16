using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Business.Data
{
    public interface IMasterExpenseRepository : IRepositoryBase<MasterExpense>
    {

        void CreateExpense(MasterExpense rec);

        MasterExpense GetExpenseById(Guid id);

        Boolean IsDuplicate(string name);

        Boolean IsDuplicateForUpdate(string name, Guid id);

        List<MasterExpense> GetExpenseByName(string searchText);
    }
}
