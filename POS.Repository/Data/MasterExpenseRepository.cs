using Microsoft.EntityFrameworkCore;
using POS.Business.Data;
using POS.Data.Context;
using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repository.Data
{
    public class MasterExpenseRepository : RepositoryBase<MasterExpense>, IMasterExpenseRepository
    {
        public RepositoryContext _repositoryContext;

        public MasterExpenseRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _repositoryContext.MasterExpenses.Include(c => c.ExpenseType).ToList();
        }

        public void CreateExpense(MasterExpense rec)
        {
            rec.Id = Guid.NewGuid();

            Create(rec);
        }

        public MasterExpense GetExpenseById(Guid id)
        {
            return FindByCondition(x => x.Id.Equals(id)).FirstOrDefault();
        }

        public Boolean IsDuplicate(string name)
        {
            Boolean result = false;
            var company = FindByCondition(x => x.Name.ToLower().Equals(name.ToLower())).ToList();

            if (company.Count > 0)
                result = true;

            return result;
        }

        public Boolean IsDuplicateForUpdate(string name, Guid id)
        {
            Boolean result = false;
            var company = FindByCondition(x => x.Name.ToLower().Equals(name.ToLower()) && x.Id != id).ToList();

            if (company.Count > 0)
                result = true;

            return result;
        }

        public List<MasterExpense> GetExpenseByName(string searchText)
        {
            return FindByCondition(x => x.Name.Contains(searchText)).ToList();
        }
    }
}
