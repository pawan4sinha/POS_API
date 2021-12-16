using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Business.Data
{
    public interface IMasterUnitRepository : IRepositoryBase<MasterUnit>
    {
        void CreateUnit(MasterUnit rec);

        MasterUnit GetUnitById(Guid id);

        Boolean IsDuplicate(string name);

        Boolean IsDuplicateForUpdate(string name, Guid id);

    }
}
