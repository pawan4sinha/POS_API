using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Business.Data
{
    public interface IMasterRoleRepository : IRepositoryBase<MasterRole>
    {
        void CreateRole(MasterRole role, Guid userId);

        void UpdateRole(MasterRole role, Guid userId);

        MasterRole GetRoleById(Guid id);

        List<MasterRole> GetRoleByName(string roleName);

        Boolean IsDuplicate(string roleName);

        Boolean IsDuplicateForUpdate(string roleName, Guid id);
    }
}
