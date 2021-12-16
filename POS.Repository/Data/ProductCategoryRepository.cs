using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POS.Business.Data;
using POS.Data.Context;
using POS.Data.Models;
using POS.Common.ExtensionMethods;

namespace POS.Repository.Data
{
    public class ProductCategoryRepository : RepositoryBase<ProductCategory>, IProductCategoryRepository
    {
        public RepositoryContext _repositoryContext;

        public ProductCategoryRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public ProductCategory GetProductCategoryById(Guid id)
        {
            return FindByCondition(x => x.Id.Equals(id)).FirstOrDefault();
        }

        public List<ProductCategory> GetProductCategoryByName(string categoryName)
        {
            return FindByCondition(x => x.Name.Contains(categoryName)).OrderBy(x => x.SortOrder).ToList();
        }

        public void CreateProductCategory(ProductCategory productCategory)
        {
            productCategory.Id = Guid.NewGuid();
            if (productCategory.SortOrder == 0)
            {
                var rec = _repositoryContext.ProductCategories.OrderByDescending(x => x.SortOrder).FirstOrDefault();
                int lastSortOrder = 0;
                if (rec != null)
                {
                    lastSortOrder = rec.SortOrder;
                }

                var nextSortOrder = Sequence.NextValue(lastSortOrder);
                productCategory.SortOrder = nextSortOrder;
            }
            Create(productCategory);
        }

        public Boolean IsDuplicate(string name)
        {
            Boolean result = false;
            var company = FindByCondition(x => x.Name.ToLower().Equals(name.ToLower())).ToList();

            if (company.Count > 0)
                result = true;

            return result;
        }

        public Boolean IsDuplicateForUpdate(string name, Guid id)
        {
            Boolean result = false;
            var company = FindByCondition(x => x.Name.ToLower().Equals(name.ToLower()) && x.Id != id).ToList();

            if (company.Count > 0)
                result = true;

            return result;
        }

    }
}
