using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Business.Data
{
    public interface IProductCategoryRepository: IRepositoryBase<ProductCategory>
    {
        ProductCategory GetProductCategoryById(Guid id);

        List<ProductCategory> GetProductCategoryByName(string categoryName);

        void CreateProductCategory(ProductCategory productCategory);

        Boolean IsDuplicate(string name);

        Boolean IsDuplicateForUpdate(string name, Guid id);
    }
}
