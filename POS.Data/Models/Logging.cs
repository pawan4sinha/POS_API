using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace POS.Data.Models
{
    public class Logging
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
