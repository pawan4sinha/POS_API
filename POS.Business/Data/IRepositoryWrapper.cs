namespace POS.Business.Data
{
    public interface IRepositoryWrapper
    {
        IAccountsRepository Accounts { get; }
                
        IMasterCountryRepository MasterCountry { get; }

        IMasterCompanyRepository MasterCompany { get; }

        IMasterRoleRepository MasterRole { get; }

        IMasterTaxTypeRepository MasterTaxType { get; }

        IMasterTaxDetailRepository MasterTaxDetail { get; }

        IMasterWalletRepository MasterWallet { get; }

        IMasterUnitRepository MasterUnit { get; }

        IMasterExpenseTypeRepository MasterExpenseType { get; }

        IMasterExpenseRepository MasterExpense{ get; }

        IMasterStoreTypeRepository MasterStoreType { get; }

        IMasterStoreDetailRepository MasterStoreDetail { get; }

        IMasterRacksRepository MasterRacks { get; }

        IProductCategoryRepository ProductCategory { get; }

        IProductSubCategoryRepository ProductSubCategory { get; }

        IMasterManufacturersRepository MasterManufacturers { get; }

        IMasterProductRepository MasterProduct { get; }

        void Save();
    }
}
