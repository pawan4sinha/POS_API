using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Business.Data
{
    public interface IProductSubCategoryRepository: IRepositoryBase<ProductSubCategory>
    {
        void CreateProductSubCategory(ProductSubCategory rec);

        ProductSubCategory GetProductSubCategoryById(Guid id);

        List<ProductSubCategory> GetProductSubCategoryByName(string categoryName);

        List<ProductSubCategory> GetByParentCategory(Guid parentCategoryId);

        Boolean IsDuplicate(string name);

        Boolean IsDuplicateForUpdate(string name, Guid id);

    }
}
