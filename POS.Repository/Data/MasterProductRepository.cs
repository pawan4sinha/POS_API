using Microsoft.EntityFrameworkCore;
using POS.Business.Data;
using POS.Common.ExtensionMethods;
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
    public class MasterProductRepository : RepositoryBase<MasterProduct>, IMasterProductRepository
    {
        public RepositoryContext _repositoryContext;

        public MasterProductRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
            _repositoryContext.MasterProducts.Include(c => c.Manufacturers).ToList();
            _repositoryContext.MasterProducts.Include(c => c.SubCategory).ToList();
            _repositoryContext.MasterProducts.Include(c => c.TaxDetail).ToList();
            _repositoryContext.MasterProducts.Include(c => c.Unit).ToList();

            _repositoryContext.MasterProducts.Include(c => c.SubCategory.ProductCategory).ToList();
        }

        public void CreateProduct(MasterProduct rec)
        {

            rec.Id = Guid.NewGuid();

            Create(rec);
        }

        public MasterProduct GetProductById(Guid id)
        {
            return FindByCondition(x => x.Id.Equals(id)).FirstOrDefault();
        }

        public int GetAvailableNextCode()
        {
            var rec = _repositoryContext.MasterProducts.OrderByDescending(x => x.Code).FirstOrDefault();
            int lastCode = 0;
            if (rec != null)
            {
                lastCode = rec.Code;
            }
            var nextCode = Sequence.NextValue(lastCode, 1);

            return nextCode;
        }

        public List<MasterProduct> SearchProductList(MasterProductForSearch rec)
        {
            string procudtCode = Convert.ToString(rec.Code);

            return FindByCondition(x => (string.IsNullOrEmpty(rec.Name) || x.Name.Contains(rec.Name)) &&
                                    (string.IsNullOrEmpty(procudtCode) || x.Code.ToString().Contains(procudtCode)) &&
                                    (string.IsNullOrEmpty(rec.Barcode) || x.Barcode.Contains(rec.Barcode)) &&
                                    (rec.SubCategoryId == null || x.SubCategoryId.Equals(rec.SubCategoryId)) &&
                                    (rec.CategoryId == null || x.SubCategory.CategoryId.Equals(rec.CategoryId))
                    ).ToList();
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

        public Boolean IsDuplicateCode(int code)
        {
            Boolean result = false;
            var rec = FindByCondition(x => x.Code.Equals(code)).ToList();

            if (rec.Count > 0)
                result = true;

            return result;
        }

        public Boolean IsDuplicateCodeForUpdate(int code, Guid id)
        {
            Boolean result = false;
            var rec = FindByCondition(x => x.Code.Equals(code) && x.Id != id).ToList();

            if (rec.Count > 0)
                result = true;

            return result;
        }

        public Boolean IsDuplicateBarcode(string barcode)
        {
            Boolean result = false;
            var rec = FindByCondition(x => x.Barcode.ToLower().Equals(barcode.ToLower())).ToList();

            if (rec.Count > 0)
                result = true;

            return result;
        }

        public Boolean IsDuplicateBarcodeForUpdate(string barcode, Guid id)
        {
            Boolean result = false;
            var rec = FindByCondition(x => x.Barcode.ToLower().Equals(barcode.ToLower()) && x.Id != id).ToList();

            if (rec.Count > 0)
                result = true;

            return result;
        }
    }
}
