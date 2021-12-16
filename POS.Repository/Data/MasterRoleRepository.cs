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
    public class MasterRoleRepository : RepositoryBase<MasterRole>, IMasterRoleRepository
    {
        public RepositoryContext _repositoryContext;

        public MasterRoleRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public void CreateRole(MasterRole role, Guid userId)
        {

            role.Id = Guid.NewGuid();
            role.CreatedBy = userId;
            role.CreatedDate = DateTime.Now;

            Create(role);
        }

        public void UpdateRole(MasterRole role, Guid userId)
        {

            role.ModifiedBy = userId;
            role.ModifiedDate = DateTime.Now;

            Update(role);
        }

        public MasterRole GetRoleById(Guid id)
        {
            return FindByCondition(x => x.Id.Equals(id)).FirstOrDefault();
        }

        public List<MasterRole> GetRoleByName(string roleName)
        {
            return FindByCondition(x => x.Name.Contains(roleName)).OrderBy(x => x.Name).ToList();
        }

        public Boolean IsDuplicate(string roleName)
        {
            Boolean result = false;
            var company = FindByCondition(x => x.Name.ToLower().Equals(roleName.ToLower())).ToList();

            if (company.Count > 0)
                result = true;

            return result;
        }

        public Boolean IsDuplicateForUpdate(string roleName, Guid id)
        {
            Boolean result = false;
            var company = FindByCondition(x => x.Name.ToLower().Equals(roleName.ToLower()) && x.Id != id).ToList();

            if (company.Count > 0)
                result = true;

            return result;
        }

    }
}
