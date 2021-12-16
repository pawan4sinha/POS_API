using POS.Business.Data;
using POS.Common.ExtensionMethods;
using POS.Data.Context;
using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repository.Data
{
    public class SupplierRepository : RepositoryBase<Supplier>, ISupplierRepository
    {
        public RepositoryContext _repositoryContext;

        public SupplierRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public Supplier GetSupplierById(Guid id)
        {
            return FindByCondition(x => x.Id.Equals(id)).FirstOrDefault();
        }

        public List<Supplier> GetSupplierByName(string name)
        {
            return FindByCondition(x => x.Name.Contains(name)).OrderBy(x => x.Name).ToList();
        }

        public int GetAvailableNextCode()
        {
            var rec = _repositoryContext.Suppliers.OrderByDescending(x => x.Code).FirstOrDefault();
            int lastCode = 0;
            if (rec != null)
            {
                lastCode = rec.Code;
            }
            var nextCode = Sequence.NextValue(lastCode, 1);

            return nextCode;
        }

        public void CreateSupplier(Supplier supplier, Guid userId)
        {
            supplier.Id = Guid.NewGuid();
            supplier.CreatedBy = userId;
            supplier.CreatedDate = DateTime.Now;

            
            Create(supplier);
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

        public Boolean IsDuplicateCode(int code)
        {
            Boolean result = false;
            var rec = FindByCondition(x => x.Code.Equals(code)).ToList();

            if (rec.Count > 0)
                result = true;

            return result;
        }

        public Boolean IsDuplicateCodeForUpdate(int code, Guid id)
        {
            Boolean result = false;
            var rec = FindByCondition(x => x.Code.Equals(code) && x.Id != id).ToList();

            if (rec.Count > 0)
                result = true;

            return result;
        }


    }
}
