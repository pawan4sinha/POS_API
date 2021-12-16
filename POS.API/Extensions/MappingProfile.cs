using AutoMapper;
using POS.Data.Entities;
using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.API.Extensions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Accounts, AccountsEntity>();
            CreateMap<Accounts, AccountsForUpdateEntity>();
            CreateMap<AccountsForCreateEntity, Accounts>();
            CreateMap<AccountsForLoginEntity, Accounts>();
            CreateMap<AccountsForUpdateEntity, Accounts>();
            
            CreateMap<MasterCompany, MasterCompanyEntity>();
            CreateMap<MasterCompany, CompanyForCreateEntity>();
            CreateMap<CompanyForCreateEntity, MasterCompany>();
            CreateMap<MasterCompany, CompanyForUpdateEntity>();
            CreateMap<CompanyForUpdateEntity, MasterCompany>();

            CreateMap<MasterRole, MasterRoleEntity>();
            CreateMap<MasterRole, MasterRoleForCreateEntity>();
            CreateMap<MasterRoleForCreateEntity, MasterRole>();
            CreateMap<MasterRole, MasterRoleForUpdateEntity>();
            CreateMap<MasterRoleForUpdateEntity, MasterRole>();

            CreateMap<MasterTaxType, MasterTaxTypeEntity>();
            CreateMap<MasterTaxType, MasterTaxTypeForCreateEntity>();
            CreateMap<MasterTaxTypeForCreateEntity, MasterTaxType>();
            CreateMap<MasterTaxType, MasterTaxTypeForUpdateEntity>();
            CreateMap<MasterTaxTypeForUpdateEntity, MasterTaxType>();

            CreateMap<MasterTaxDetail, MasterTaxDetailEntity>();
            CreateMap<MasterTaxDetail, MasterTaxDetailForCreateEntity>();
            CreateMap<MasterTaxDetailForCreateEntity, MasterTaxDetail>();
            CreateMap<MasterTaxDetail, MasterTaxDetailForUpdateEntity>();
            CreateMap<MasterTaxDetailForUpdateEntity, MasterTaxDetail>();

            CreateMap<MasterWallet, MasterWalletEntity>();
            CreateMap<MasterWallet, MasterWalletForCreateEntity>();
            CreateMap<MasterWalletForCreateEntity, MasterWallet>();
            CreateMap<MasterWallet, MasterWalletForUpdateEntity>();
            CreateMap<MasterWalletForUpdateEntity, MasterWallet>();

            CreateMap<MasterUnit, MasterUnitEntity>();
            CreateMap<MasterUnit, MasterUnitForCreateEntity>();
            CreateMap<MasterUnitForCreateEntity, MasterUnit>();
            CreateMap<MasterUnit, MasterUnitForUpdateEntity>();
            CreateMap<MasterUnitForUpdateEntity, MasterUnit>();

            CreateMap<MasterExpense, MasterExpenseEntity>();
            CreateMap<MasterExpense, MasterExpenseForCreateEntity>();
            CreateMap<MasterExpenseForCreateEntity, MasterExpense>();
            CreateMap<MasterExpense, MasterExpenseForUpdateEntity>();
            CreateMap<MasterExpenseForUpdateEntity, MasterExpense>();

            CreateMap<MasterStoreDetail, MasterStoreDetailEntity>();
            CreateMap<MasterStoreDetail, MasterStoreDetailForCreateEntity>();
            CreateMap<MasterStoreDetailForCreateEntity, MasterStoreDetail>();
            CreateMap<MasterStoreDetail, MasterStoreDetailForUpdateEntity>();
            CreateMap<MasterStoreDetailForUpdateEntity, MasterStoreDetail>();

            CreateMap<MasterRacks, MasterRacksEntity>();
            CreateMap<MasterRacks, MasterRacksForCreateEntity>();
            CreateMap<MasterRacksForCreateEntity, MasterRacks>();
            CreateMap<MasterRacks, MasterRacksForUpdateEntity>();
            CreateMap<MasterRacksForUpdateEntity, MasterRacks>();

            CreateMap<ProductCategory, ProductCategoryEntity>();
            CreateMap<ProductCategory, ProductCategoryForCreateEntity>();
            CreateMap<ProductCategoryForCreateEntity, ProductCategory>();
            CreateMap<ProductCategory, ProductCategoryForUpdateEntity>();
            CreateMap<ProductCategoryForUpdateEntity, ProductCategory>();

            CreateMap<ProductSubCategory, ProductSubCategoryEntity>();
            CreateMap<ProductSubCategory, ProductSubCategoryForCreateEntity>();
            CreateMap<ProductSubCategoryForCreateEntity, ProductSubCategory>();
            CreateMap<ProductSubCategory, ProductSubCategoryForUpdateEntity>();
            CreateMap<ProductSubCategoryForUpdateEntity, ProductSubCategory>();

            CreateMap<MasterManufacturers, MasterManufacturersEntity>();
            CreateMap<MasterManufacturers, MasterManufacturersForCreateEntity>();
            CreateMap<MasterManufacturersForCreateEntity, MasterManufacturers>();
            CreateMap<MasterManufacturers, MasterManufacturersForUpdateEntity>();
            CreateMap<MasterManufacturersForUpdateEntity, MasterManufacturers>();

            CreateMap<MasterProduct, MasterProductEntity>();
            CreateMap<MasterProduct, MasterProductForCreateEntity>();
            CreateMap<MasterProductForCreateEntity, MasterProduct>();
            CreateMap<MasterProduct, MasterProductForUpdateEntity>();
            CreateMap<MasterProductForUpdateEntity, MasterProduct>();
        }
    }
}
