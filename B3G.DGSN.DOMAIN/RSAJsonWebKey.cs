using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3G.DGSN.DOMAIN
{
    public class RSAJsonWebKey
    {
        [JsonProperty("use")] public string _use;
        [JsonProperty("kty")] public string _kty;
        [JsonProperty("kid")] public string _kid;
        [JsonProperty("alg")] public string _alg;
        [JsonProperty("n")] public string _n;
        [JsonProperty("e")] public string _e;
     
    }
}
