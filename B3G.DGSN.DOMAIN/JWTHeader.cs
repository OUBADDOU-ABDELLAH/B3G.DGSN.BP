using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3G.DGSN.DOMAIN
{
    public class JWTHeader
    {
        public string _typ { get; set; }
        public string _key { get; set; }
        public string _alg { get; set; }
        public string _kid { get; set; }
    }
}
