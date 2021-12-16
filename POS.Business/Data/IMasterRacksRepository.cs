using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Business.Data
{
    public interface IMasterRacksRepository : IRepositoryBase<MasterRacks>
    {
        void CreateRack(MasterRacks rec);

        MasterRacks GetRackById(Guid id);

        Boolean IsDuplicate(string name);

        Boolean IsDuplicateForUpdate(string name, Guid id);

        List<MasterRacks> GetRackByName(string searchText);
    }
}
