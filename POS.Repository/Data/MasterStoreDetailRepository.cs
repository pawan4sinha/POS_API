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
    public class MasterStoreDetailRepository : RepositoryBase<MasterStoreDetail>, IMasterStoreDetailRepository
    {
        public RepositoryContext _repositoryContext;

        public MasterStoreDetailRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _repositoryContext.MasterStoreDetails.Include(c => c.StoreType).ToList();
        }

        public void CreateStoreDetail(MasterStoreDetail rec)
        {
            rec.Id = Guid.NewGuid();

            Create(rec);
        }

        public MasterStoreDetail GetStoreDetailById(Guid id)
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

        public List<MasterStoreDetail> GetStoreDetailByNameAndCity(string searchText)
        {
            return FindByCondition(x => x.Name.Contains(searchText) ||
                    x.City.Contains(searchText)).ToList();
        }
    }
}
