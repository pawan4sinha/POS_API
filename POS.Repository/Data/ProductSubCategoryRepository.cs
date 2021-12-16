using Microsoft.EntityFrameworkCore;
using POS.Business.Data;
using POS.Data.Context;
using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using POS.Common.ExtensionMethods;

namespace POS.Repository.Data
{
    public class ProductSubCategoryRepository:RepositoryBase<ProductSubCategory>, IProductSubCategoryRepository
    {
        public RepositoryContext _repositoryContext;
        public ProductSubCategoryRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _repositoryContext.ProductSubCategories.Include(c => c.ProductCategory).ToList();
        }

        public void CreateProductSubCategory(ProductSubCategory productSubCategory)    
        {

            productSubCategory.Id = Guid.NewGuid();

            if (productSubCategory.SortOrder == 0)
            {
                var rec = _repositoryContext.ProductSubCategories.OrderByDescending(x => x.SortOrder).FirstOrDefault();
                int lastSortOrder = 0;
                if (rec != null)
                {
                    lastSortOrder = rec.SortOrder;
                }

                var nextSortOrder = Sequence.NextValue(lastSortOrder);
                productSubCategory.SortOrder = nextSortOrder;
            }

            Create(productSubCategory);
        }

        public ProductSubCategory GetProductSubCategoryById(Guid id)
        {
            return FindByCondition(x => x.Id.Equals(id)).FirstOrDefault();
        }

        public List<ProductSubCategory> GetProductSubCategoryByName(string categoryName)
        {
            return FindByCondition(x => x.Name.Contains(categoryName)).OrderBy(x => x.SortOrder).ToList();
        }

        public List<ProductSubCategory> GetByParentCategory(Guid parentCategoryId)
        {
            return FindByCondition(x => x.CategoryId == parentCategoryId).ToList();
        }

        public Boolean IsDuplicate(string name)
        {
            Boolean result = false;
            var rec = FindByCondition(x => x.Name.ToLower().Equals(name.ToLower())).ToList();

            if (rec.Count > 0)
                result = true;

            return result;
        }

        public Boolean IsDuplicateForUpdate(string name, Guid id)
        {
            Boolean result = false;
            var rec = FindByCondition(x => x.Name.ToLower().Equals(name.ToLower()) && x.Id != id).ToList();

            if (rec.Count > 0)
                result = true;

            return result;
        }
    }
}
