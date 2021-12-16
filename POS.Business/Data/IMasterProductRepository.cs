using POS.Data.Entities;
using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Business.Data
{
    public interface IMasterProductRepository : IRepositoryBase<MasterProduct>
    {
        void CreateProduct(MasterProduct rec);

        MasterProduct GetProductById(Guid id);

        List<MasterProduct> SearchProductList(MasterProductForSearch rec);

        int GetAvailableNextCode();

        Boolean IsDuplicate(string name);

        Boolean IsDuplicateForUpdate(string name, Guid id);

        Boolean IsDuplicateCode(int code);

        Boolean IsDuplicateCodeForUpdate(int code, Guid id);

        Boolean IsDuplicateBarcode(string barcode);

        Boolean IsDuplicateBarcodeForUpdate(string barcode, Guid id);

    }
}
