using Microsoft.EntityFrameworkCore;
using POS.Business.Data;
using POS.Common.Security;
using POS.Data.Context;
using POS.Data.Entities;
using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repository.Data
{
    public class AccountsRepository : RepositoryBase<Accounts>, IAccountsRepository
    {
        public RepositoryContext _repositoryContext;

        public AccountsRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _repositoryContext.Accounts.Include(x => x.Role).ToList();
            _repositoryContext.Accounts.Include(x => x.Country).ToList();
        }

        public Accounts ValidateUser(AccountsForLoginEntity loginEntity)
        {
            var account = FindByCondition(x => x.Username.ToLower().Equals(loginEntity.Username.ToLower())).FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(account.Password) && account.Password == Encryption.CalculateHash(loginEntity.Password, account.Salt))
            {
                return account;
            }
            else
            {
                return null;
            }

            //return FindByCondition(
            //    x => x.Username.Equals(loginEntity.Username) && 
            //    x.Password.Equals(loginEntity.Password)
            //    ).FirstOrDefault();
        }

        public void CreateAccount(Accounts accounts, Guid userId)
        {
            string salt = Encryption.GetSalt();
            
            accounts.Password = Encryption.CalculateHash(accounts.Password, salt);
            accounts.Salt = salt;

            accounts.Id = Guid.NewGuid();
            accounts.CreatedDate = DateTime.Now;
            accounts.CreatedBy = userId;

            Create(accounts);
        }

        public List<Accounts> GetAccountByRoleId(Guid roleId)
        {
            return FindByCondition(x => x.RoleId.Equals(roleId)).ToList();
        }

        public Accounts GetAccountById(Guid id)
        {
            return FindByCondition(x => x.Id.Equals(id)).FirstOrDefault();
        }

        public List<Accounts> GetAccountByFirstLastOrUserName(string searchText)
        {
            return FindByCondition(x => x.FirstName.Contains(searchText) ||
                    x.LastName.Contains(searchText) ||
                    x.Username.Contains(searchText)).ToList();
        }

        public Accounts GetAccountByUserName(string username)
        {
            return FindByCondition(x => x.Username.Equals(username)).FirstOrDefault();
        }

        public Boolean IsDuplicate(string username)
        {
            Boolean result = false;
            var account = FindByCondition(x => x.Username.ToLower().Equals(username.ToLower())).ToList();

            if (account.Count > 0)
                result = true;

            return result;
        }

        public Boolean IsDuplicateForUpdate(string username, Guid id)
        {
            Boolean result = false;
            var account = FindByCondition(x => x.Username.ToLower().Equals(username.ToLower()) && x.Id != id).ToList();

            if (account.Count > 0)
                result = true;

            return result;
        }

        public void UpdateAccount(Accounts account, Guid userId)
        {

            account.ModifyBy = userId;
            account.ModifyDate = DateTime.Now;

            Update(account);
        }

        public void ResetPassword(Accounts account, Guid userId)
        {
            string salt = Encryption.GetSalt();

            account.Password = Encryption.CalculateHash(account.Password, salt);
            account.Salt = salt;

            account.ModifyBy = userId;
            account.ModifyDate = DateTime.Now;

            Update(account);
        }
    }
}
