using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Business.Data
{
    public interface IMasterStoreDetailRepository : IRepositoryBase<MasterStoreDetail>
    {
        void CreateStoreDetail(MasterStoreDetail rec);

        MasterStoreDetail GetStoreDetailById(Guid id);

        Boolean IsDuplicate(string name);

        Boolean IsDuplicateForUpdate(string name, Guid id);

        List<MasterStoreDetail> GetStoreDetailByNameAndCity(string searchText);
    }
}
