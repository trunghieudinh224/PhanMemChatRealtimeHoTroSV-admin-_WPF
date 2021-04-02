using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C65.method
{
    class mt_sudungchung
    {
        public string format_N2T(int x)
        {
            return x.ToString("#,##,##,##,##,##,##,##,##,##,##,##,##,###");
        }

        public int format_T2N(string x)
        {
            if (x == "")
            {
                x = "0";
            }
            return Convert.ToInt32(x.Trim().Replace(",", "").Replace(".", "").Replace("- ",""));
        }
    }
}
