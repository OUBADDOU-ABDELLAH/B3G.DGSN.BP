using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace B3G.DGSN.DOMAIN
{
    public class OpenIdToken : Object
    {
        [JsonProperty("access_token")] public string _accessToken;
        [JsonProperty("expires_in")] public long _validity;
        [JsonProperty("id_token")] public string _idToken;
        [JsonProperty("scope")] public string _scope;
        [JsonProperty("token_type")] public string _tokenType;
    }
}
