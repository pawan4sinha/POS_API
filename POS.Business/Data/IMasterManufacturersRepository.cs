using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Business.Data
{
    public interface IMasterManufacturersRepository : IRepositoryBase<MasterManufacturers>
    {
        MasterManufacturers GetManufacturerById(Guid id);

        List<MasterManufacturers> GetManufacturerByName(string name);

        void CreateManufacturer(MasterManufacturers manufacturer);

        Boolean IsDuplicate(string name);

        Boolean IsDuplicateForUpdate(string name, Guid id);
    }
}
