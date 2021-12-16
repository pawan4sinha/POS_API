using Microsoft.EntityFrameworkCore;
using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POS.Data.Context
{
    public partial class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options)
            : base(options)
        {

        }

        public DbSet<Accounts> Accounts { get; set; }

        public DbSet<MasterCountry> MasterCountries { get; set; }

        public DbSet<MasterCompany> MasterCompanies { get; set; }

        public DbSet<MasterRole> MasterRoles { get; set; }

        public DbSet<MasterTaxType> MasterTaxTypes { get; set; }

        public DbSet<MasterTaxDetail> MasterTaxDetails { get; set; }

        public DbSet<MasterWallet> MasterWallets { get; set; }

        public DbSet<MasterUnit> MasterUnits { get; set; }

        public DbSet<MasterExpenseType> MasterExpenseTypes { get; set; }

        public DbSet<MasterExpense> MasterExpenses { get; set; }

        public DbSet<MasterStoreType> MasterStoreTypes { get; set; }

        public DbSet<MasterStoreDetail> MasterStoreDetails { get; set; }

        public DbSet<MasterRacks> MasterRacks { get; set; }

        public DbSet<ProductCategory> ProductCategories { get; set; }

        public DbSet<ProductSubCategory> ProductSubCategories { get; set; }

        public DbSet<MasterManufacturers> MasterManufacturers { get; set; }

        public DbSet<MasterProduct> MasterProducts { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<SupplierBank> SupplierBanks { get; set; }
    }
}
