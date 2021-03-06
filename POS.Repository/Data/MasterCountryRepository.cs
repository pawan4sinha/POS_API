using POS.Business.Data;
using POS.Data.Context;
using POS.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Repository.Data
{
    public class MasterCountryRepository : RepositoryBase<MasterCountry>, IMasterCountryRepository
    {
        public RepositoryContext _repositoryContext;

        public MasterCountryRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
            this._repositoryContext = repositoryContext;
        }


    }
}
