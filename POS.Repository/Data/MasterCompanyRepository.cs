using Microsoft.EntityFrameworkCore;
using POS.Business.Data;
using POS.Data.Context;
using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repository.Data
{
    public class MasterCompanyRepository: RepositoryBase<MasterCompany>, IMasterCompanyRepository
    {
        public RepositoryContext _repositoryContext;

        public MasterCompanyRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _repositoryContext.MasterCompanies.Include(c => c.Country).ToList();
            //_repositoryContext.MasterCompanies.Include(c => c.CreatedByNavigation).ToList();
            //_repositoryContext.MasterCompanies.Include(c => c.ModifyByNavigation).ToList();
        }

        public void CreateCompany(MasterCompany company, Guid userId)
        {

            company.Id = Guid.NewGuid();
            company.CreatedBy = userId;
            company.CreatedDate = DateTime.Now;

            Create(company);
        }

        public void UpdateCompany(MasterCompany company, Guid userId)
        {

            company.ModifyBy = userId;
            company.ModifyDate = DateTime.Now;

            Update(company);
        }

        public MasterCompany GetCompanyById(Guid id)
        {
            return FindByCondition(x => x.Id.Equals(id)).FirstOrDefault();
        }

        public Boolean IsDuplicate(string companyName)
        {
            Boolean result = false;
            var company = FindByCondition(x => x.Name.ToLower().Equals(companyName.ToLower())).ToList();

            if (company.Count > 0)
                result = true;

            return result;
        }

        public Boolean IsDuplicateForUpdate(string companyName, Guid id)
        {
            Boolean result = false;
            var company = FindByCondition(x => x.Name.ToLower().Equals(companyName.ToLower()) && x.Id != id).ToList();

            if (company.Count > 0)
                result = true;

            return result;
        }

        public Boolean IsDefaultExists()
        {
            Boolean result = false;
            var company = FindByCondition(x => x.SetDefault.Equals(1)).ToList();

            if (company.Count > 0)
                result = true;

            return result;
        }

        public Boolean IsDefaultExistsForUpdate(Guid id)
        {
            Boolean result = false;
            var company = FindByCondition(x => x.SetDefault.Equals(1) && x.Id != id).ToList();

            if (company.Count > 0)
                result = true;

            return result;
        }

        public Boolean IsDeletingRecordIsDefault(Guid id)
        {
            Boolean result = false;
            var company = FindByCondition(x => x.Id.Equals(id) && x.SetDefault.Equals(1));

            if (company != null)
                result = true;

            return result;
        }
    }
}
