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
    public class MasterTaxTypeRepository : RepositoryBase<MasterTaxType>, IMasterTaxTypeRepository
    {
        public RepositoryContext _repositoryContext;

        public MasterTaxTypeRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public void CreateTaxType(MasterTaxType rec)
        {

            rec.Id = Guid.NewGuid();

            Create(rec);
        }

        public MasterTaxType GetTaxTypeById(Guid id)
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
