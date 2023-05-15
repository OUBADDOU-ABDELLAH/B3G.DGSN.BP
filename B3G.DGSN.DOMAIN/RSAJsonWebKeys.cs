using Newtonsoft.Json;

namespace B3G.DGSN.DOMAIN
{
    public class RSAJsonWebKeys
    {



        [JsonProperty("keys")] public RSAJsonWebKey[] keys;
        public static void GetIssuerKeys(string JwksURL, ref RSAJsonWebKeys keys, ref string error)
        {
            keys = null;
            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            string url = JwksURL;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                error = response.ReasonPhrase;
                return;
            }
            using (HttpContent content = response.Content)
            {
                string jsonResp = content.ReadAsStringAsync().Result;
                if (string.IsNullOrEmpty(jsonResp))
                {
                    error = "Impossible de lire la configuration OpenID";
                    return;
                }
                try
                {
                    keys = JsonConvert.DeserializeObject<RSAJsonWebKeys>(jsonResp);
                }
                catch (Exception e)
                {
                    error = e.Message;
                    return;
                }
            }
            return;
        }
    }
}
