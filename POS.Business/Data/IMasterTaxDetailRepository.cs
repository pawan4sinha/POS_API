using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Business.Data
{
    public interface IMasterTaxDetailRepository : IRepositoryBase<MasterTaxDetail>
    {
        void CreateTaxDetail(MasterTaxDetail rec);

        MasterTaxDetail GetTaxDetailById(Guid id);

        Boolean IsDuplicate(string name);

        Boolean IsDuplicateForUpdate(string name, Guid id);
    }
}
