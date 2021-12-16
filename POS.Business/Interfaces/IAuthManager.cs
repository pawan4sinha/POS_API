using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Business.Interfaces
{
    public interface IAuthManager
    {
        string Authenticate(string username);

    }
}
