using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Common.ExtensionMethods
{
    public static class Sequence
    {
        public static int NextValue(int lastValue, int increament = 10)
        {
            // Smaller multiple
            int a = (lastValue / increament) * increament;

            // Larger multiple
            int b = a + increament;

            // Return 
            return b;
        }
    }
}
