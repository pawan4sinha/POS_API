using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS.API.Extensions
{
    public class AppSettings
    {        
        public Jwt Jwt { get; set; }

        public string MyNewValue { get; set; }

    }

    public class Jwt
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Expiry { get; set; }
    }
}

