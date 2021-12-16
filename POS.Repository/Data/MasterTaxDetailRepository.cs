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
    public class MasterTaxDetailRepository : RepositoryBase<MasterTaxDetail>, IMasterTaxDetailRepository
    {
        public RepositoryContext _repositoryContext;

        public MasterTaxDetailRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _repositoryContext.MasterTaxDetails.Include(c => c.TaxType).ToList();
        }

        public void CreateTaxDetail(MasterTaxDetail rec)
        {

            rec.Id = Guid.NewGuid();

            Create(rec);
        }

        public MasterTaxDetail GetTaxDetailById(Guid id)
        {
            return FindByCondition(x => x.Id.Equals(id)).FirstOrDefault();
        }

        public Boolean IsDuplicate(string name)
        {
            Boolean result = false;
            var rec = FindByCondition(x => x.Name.ToLower().Equals(name.ToLower())).ToList();

            if (rec.Count > 0)
                result = true;

            return result;
        }

        public Boolean IsDuplicateForUpdate(string name, Guid id)
        {
            Boolean result = false;
            var rec = FindByCondition(x => x.Name.ToLower().Equals(name.ToLower()) && x.Id != id).ToList();

            if (rec.Count > 0)
                result = true;

            return result;
        }

    }
}
