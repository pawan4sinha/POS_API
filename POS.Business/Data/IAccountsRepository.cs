using POS.Data.Entities;
using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Business.Data
{
    public interface IAccountsRepository: IRepositoryBase<Accounts>
    {
        Accounts ValidateUser(AccountsForLoginEntity loginEntity);

        void CreateAccount(Accounts accounts, Guid userId);

        List<Accounts> GetAccountByRoleId(Guid roleId);

        Accounts GetAccountById(Guid id);

        List<Accounts> GetAccountByFirstLastOrUserName(string searchText);

        Accounts GetAccountByUserName(string username);

        Boolean IsDuplicate(string username);

        Boolean IsDuplicateForUpdate(string username, Guid id);

        void UpdateAccount(Accounts account, Guid userId);

        void ResetPassword(Accounts account, Guid userId);
    }
}
