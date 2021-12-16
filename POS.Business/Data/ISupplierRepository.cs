using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Business.Data
{
    public interface ISupplierRepository: IRepositoryBase<Supplier>
    {
        Supplier GetSupplierById(Guid id);

        List<Supplier> GetSupplierByName(string name);

        int GetAvailableNextCode();

        void CreateSupplier(Supplier supplier, Guid userId);

        Boolean IsDuplicate(string name);

        Boolean IsDuplicateForUpdate(string name, Guid id);

        Boolean IsDuplicateCode(int code);

        Boolean IsDuplicateCodeForUpdate(int code, Guid id);
    }
}
