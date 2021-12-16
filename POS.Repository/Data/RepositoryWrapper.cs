using Microsoft.EntityFrameworkCore;
using POS.Business.Data;
using POS.Data.Context;
using System.Linq;

namespace POS.Repository.Data
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private RepositoryContext _repositoryContext;
        private IAccountsRepository _accounts;        
        private IMasterCountryRepository _masterCountry;
        private IMasterCompanyRepository _masterCompany;
        private IMasterRoleRepository _masterRole;
        private IMasterTaxTypeRepository _masterTaxType;
        private IMasterTaxDetailRepository _masterTaxDetail;
        private IMasterWalletRepository _masterWallet;
        private IMasterUnitRepository _masterUnit;
        private IMasterExpenseTypeRepository _masterExpenseType;
        private IMasterExpenseRepository _masterExpense;
        private IMasterStoreTypeRepository _masterStoreType;
        private IMasterStoreDetailRepository _masterStoreDetail;
        private IMasterRacksRepository _masterRacks;
        private IProductCategoryRepository _productCategory;
        private IProductSubCategoryRepository _productSubCategory;
        private IMasterManufacturersRepository _masterManufacturers;
        private IMasterProductRepository _masterProduct;

        public RepositoryWrapper(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IAccountsRepository Accounts
        {
            get
            {
                if (_accounts == null)
                {
                    _accounts = new AccountsRepository(_repositoryContext);
                }
                return _accounts;
            }
        }

        public IMasterCountryRepository MasterCountry
        {
            get
            {
                if (_masterCountry == null)
                {
                    _masterCountry = new MasterCountryRepository(_repositoryContext);
                }
                return _masterCountry;
            }
        }

        public IMasterCompanyRepository MasterCompany
        {
            get
            {
                if (_masterCompany == null)
                {
                    _masterCompany = new MasterCompanyRepository(_repositoryContext);
                }
                return _masterCompany;
            }
        }

        public IMasterRoleRepository MasterRole
        {
            get
            {
                if (_masterRole == null)
                {
                    _masterRole = new MasterRoleRepository(_repositoryContext);
                }
                return _masterRole;
            }
        }

        public IMasterTaxTypeRepository MasterTaxType
        {
            get
            {
                if (_masterTaxType == null)
                {
                    _masterTaxType = new MasterTaxTypeRepository(_repositoryContext);
                }
                return _masterTaxType;
            }
        }

        public IMasterTaxDetailRepository MasterTaxDetail
        {
            get
            {
                if (_masterTaxDetail == null)
                {
                    _masterTaxDetail = new MasterTaxDetailRepository(_repositoryContext);
                }
                return _masterTaxDetail;
            }
        }

        public IMasterWalletRepository MasterWallet
        {
            get
            {
                if (_masterWallet == null)
                {
                    _masterWallet = new MasterWalletRepository(_repositoryContext);
                }
                return _masterWallet;
            }
        }

        public IMasterUnitRepository MasterUnit
        {
            get
            {
                if (_masterUnit == null)
                {
                    _masterUnit = new MasterUnitRepository(_repositoryContext);
                }
                return _masterUnit;
            }
        }

        public IMasterExpenseTypeRepository MasterExpenseType
        {
            get
            {
                if (_masterExpenseType == null)
                {
                    _masterExpenseType = new MasterExpenseTypeRepository(_repositoryContext);
                }
                return _masterExpenseType;
            }
        }

        public IMasterExpenseRepository MasterExpense
        {
            get
            {
                if (_masterExpense == null)
                {
                    _masterExpense = new MasterExpenseRepository(_repositoryContext);
                }
                return _masterExpense;
            }
        }

        public IMasterStoreTypeRepository MasterStoreType
        {
            get
            {
                if (_masterStoreType == null)
                {
                    _masterStoreType = new MasterStoreTypeRepository(_repositoryContext);
                }
                return _masterStoreType;
            }
        }

        public IMasterStoreDetailRepository MasterStoreDetail
        {
            get
            {
                if (_masterStoreDetail == null)
                {
                    _masterStoreDetail = new MasterStoreDetailRepository(_repositoryContext);
                }
                return _masterStoreDetail;
            }
        }

        public IMasterRacksRepository MasterRacks
        {
            get
            {
                if (_masterRacks == null)
                {
                    _masterRacks = new MasterRacksRepository(_repositoryContext);
                }
                return _masterRacks;
            }
        }

        public IProductCategoryRepository ProductCategory
        {
            get
            {
                if (_productCategory == null)
                {
                    _productCategory = new ProductCategoryRepository(_repositoryContext);
                }
                return _productCategory;
            }
        }

        public IProductSubCategoryRepository ProductSubCategory
        {
            get
            {
                if (_productSubCategory == null)
                {
                    _productSubCategory = new ProductSubCategoryRepository(_repositoryContext);
                }
                return _productSubCategory;
            }
        }

        public IMasterManufacturersRepository  MasterManufacturers
        {
            get
            {
                if (_masterManufacturers == null)
                {
                    _masterManufacturers = new MasterManufacturersRepository(_repositoryContext);
                }
                return _masterManufacturers;
            }
        }

        public IMasterProductRepository MasterProduct
        {
            get
            {
                if (_masterProduct == null)
                {
                    _masterProduct = new MasterProductRepository(_repositoryContext);
                }
                return _masterProduct;
            }
        }

        public void Save()
        {
            _repositoryContext.SaveChanges();
        }
    }
}
