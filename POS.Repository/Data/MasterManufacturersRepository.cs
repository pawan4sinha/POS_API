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
    public class MasterManufacturersRepository : RepositoryBase<MasterManufacturers>, IMasterManufacturersRepository
    {
        public RepositoryContext _repositoryContext;

        public MasterManufacturersRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }


        public MasterManufacturers GetManufacturerById(Guid id)
        {
            return FindByCondition(x => x.Id.Equals(id)).FirstOrDefault();
        }

        public List<MasterManufacturers> GetManufacturerByName(string name)
        {
            return FindByCondition(x => x.Name.Contains(name)).OrderBy(x => x.SortOrder).ToList();
        }

        public void CreateManufacturer(MasterManufacturers manufacturer)
        {   
            manufacturer.Id = Guid.NewGuid();
            if (manufacturer.SortOrder == 0)
            {
                var rec = _repositoryContext.MasterManufacturers.OrderByDescending(x => x.SortOrder).FirstOrDefault();
                int lastSortOrder = 0;
                if (rec != null)
                {
                    lastSortOrder = rec.SortOrder;
                }

                var nextSortOrder = Sequence.NextValue(lastSortOrder);
                manufacturer.SortOrder = nextSortOrder;
            }
            Create(manufacturer);
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
    }
}
