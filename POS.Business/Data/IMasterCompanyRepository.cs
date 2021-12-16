using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Business.Data
{
    public interface IMasterCompanyRepository : IRepositoryBase<MasterCompany>
    {
        void CreateCompany(MasterCompany company, Guid userId);

        void UpdateCompany(MasterCompany company, Guid userId);

        MasterCompany GetCompanyById(Guid id);

        Boolean IsDuplicate(string companyName);

        Boolean IsDuplicateForUpdate(string companyName, Guid id);

        Boolean IsDefaultExists();

        Boolean IsDefaultExistsForUpdate(Guid id);

        Boolean IsDeletingRecordIsDefault(Guid id);
    }
}
