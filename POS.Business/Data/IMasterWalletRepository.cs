using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Business.Data
{
    public interface IMasterWalletRepository : IRepositoryBase<MasterWallet>
    {

        void CreateWallet(MasterWallet rec);

        MasterWallet GetWalletById(Guid id);

        Boolean IsDuplicate(string name);

        Boolean IsDuplicateForUpdate(string name, Guid id);

    }
}
