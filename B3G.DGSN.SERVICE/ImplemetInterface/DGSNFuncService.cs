using B3G.DGSN.DOMAIN;
using B3G.DGSN.SERVICE.INTERFACES;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Azure;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System;

namespace B3G.DGSN.SERVICE.ImplemetInterface
{
    public class DGSNFuncService : IDGSNFunc
    {
        ISessionService _service;
        private readonly IConfiguration _config;
        public DGSNFuncService(ISessionService service, IConfiguration config)
        {
            _service = service;
            _config = config;
        }
        public string RedirectUrl(string code, string state, string codeError, string ErrorDescription)
        {


            string erreur = "";
            if (state != null)
            {
                if (!_service.IsSessionExist(state))
                {
                    return erreur = "Le serveur OpenID a retourné un 'state' inconnu !";
                }
            }

            if (!string.IsNullOrEmpty(codeError) || !string.IsNullOrEmpty(ErrorDescription))
            {
                return erreur = ErrorDescription;
            }

            if (!string.IsNullOrEmpty(code))
            {
                // return Ok(new { strCode, strState });//getToken(code);
                return "code : " + code + " state : " + state;
            }
            else
            {
                erreur = "Le serveur OpenID n'a pas fourni le code d'autorisation!";
                return erreur;
            }
            //return erreur
        }
        public string GetToken(string code)
        {
            OpenIdToken token = null;
            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);

            string secret = GetSecret();// _config.GetConnectionString("secret");  //GetValue<string>("secret");
            // string secret = "b0efdc68-5723-41f2-acd1-8786d7ed7c29";
            string idClient = "c7ab42c2-446e-4881-89b1-6162e50ea204";

            string basicToken = idClient + ":" + secret;
            var basicBytes = Encoding.UTF8.GetBytes(basicToken);
            string basic = Convert.ToBase64String(basicBytes);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", basic);

            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));

            string exchangeParameters = "";
            exchangeParameters += HttpUtility.UrlEncode("grant_type") + "=" + HttpUtility.UrlEncode("authorization_code");
            exchangeParameters += "&";
            exchangeParameters += HttpUtility.UrlEncode("code") + "=" + HttpUtility.UrlEncode(code);
            exchangeParameters += "&";
            exchangeParameters += HttpUtility.UrlEncode("client_secret") + "=" + HttpUtility.UrlEncode(secret);
            exchangeParameters += "&";
            exchangeParameters += HttpUtility.UrlEncode("client_id") + "=" + HttpUtility.UrlEncode(idClient);
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "https://auth-pp.dgsn.gov.ma/oauth2/token");
            request.Content = new StringContent(exchangeParameters, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");

            HttpResponseMessage response = client.SendAsync(request).Result;
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }

            using (HttpContent content = response.Content)
            {
                var jsonResp = content.ReadAsStringAsync().Result;
                if (string.IsNullOrEmpty(jsonResp))
                {
                    return null;
                }
                else
                {
                    return jsonResp + "le secret est :" + secret;
                }
                try
                {
                    token = JsonConvert.DeserializeObject<OpenIdToken>(jsonResp);
                }
                catch (Exception e5)
                {
                    // Gérer l'erreur de désérialisation ici
                }

            }
            //  return null;
        }



        public static byte[] Base64UrlDecode(string input)
        {
            string padded = input + new string('=', (4 - input.Length % 4) % 4);
            string base64 = padded.Replace('-', '+').Replace('_', '/');
            return Base64UrlDecode(base64);  
            //return Convert.FromBase64String(base64);
        }






        public string VerifierSignature(string token,string JwksURL)
        {
            string jwksURL = JwksURL;
            RSAJsonWebKeys key1 = new() { };
            string error = null;
            RSAJsonWebKeys.GetIssuerKeys(jwksURL, ref key1, ref error);
            string res = "";
            //JWTHeader header = null;
             string[] parts = token.Split('.');
            // Décoder le header
            JwtSecurityTokenHandler paparsedJwt1 = null;
            var jwtHandler = new JwtSecurityTokenHandler();
            var parsedJwt = jwtHandler.ReadJwtToken(token);
         // Get the JWT header
            var headerTest = parsedJwt.Header;

            JWTHeader jwtHeader = new JWTHeader
            {
                _typ = headerTest.Typ,
                _alg = headerTest.Alg,
                _kid = headerTest.Kid
            };
            /*
            try
            {
                var decoded = Base64UrlDecode(parts[0]);
                if (decoded != null)
                {
                    var rawJson = System.Text.Encoding.UTF8.GetString(decoded);
                    header = JsonConvert.DeserializeObject<JWTHeader>(rawJson);
                }
            }
            catch (Exception e2)
            {
                // lbl_description.Text = e2.Message;
                // break;
            }
            */
            String Signature = parts[2];

            // Extraire la signature du jeton d'identité
            var signature = Base64UrlDecode(Signature);
            if (jwtHeader == null)
            {
                res = res+ "jwtHeader == null";
               // return res;
            }
            if (jwtHeader._typ != "JWT")
            {
                res = res + "jwtHeader._typ != JWT";
               // return res;
            }
            if (!jwtHeader._alg.StartsWith("RS"))
            {
                res = res + "!jwtHeader._alg.StartsWith(RS)";
              //  return res;
            }
         
            //var cle = key1.keys[0];

            var keySet=key1;
            bool foundKey = false;
            bool algoOK = false;
            bool useOK = false;
           // string Varkey;
            RSAJsonWebKey sigKey = null;


            
            foreach (RSAJsonWebKey key in keySet.keys)
            {
                
               
                if (key._kid == jwtHeader._kid)
                {
                    
                    foundKey = true;
                    algoOK = (key._alg == jwtHeader._alg);
                    useOK = key._use == "sig";
                    sigKey = key;
                    
                   // break;
                }
            }





            // RSAJsonWebKey key;
            var cle = keySet.keys[0];
            if (cle._kid == jwtHeader._kid)
            {
                foundKey = true;
                algoOK = (cle._alg == jwtHeader._alg);
                useOK = cle._use == "sig";
                if (algoOK && useOK && foundKey)
                {

                    res ="algoOK && useOK && foundKey";
                   // return res;

                }
            }
            // Abandonner la vérification si la clé est inconnue
            if (!foundKey)
            {
                res = "Key not found";
                //return res;
            }
            // Abandonner la vérification si l'algorithme de signature n'est pas supporté.
            if (!algoOK)
            {
                res = "algo not OK";
                //return res;
            }
            // Abandonner la vérification s'il ne s'agit pas d'une clé de signature
            if (!useOK)
            {
                res = "useOK!=sig";
               // return res;
            }




            // Identifier l'algorithme SHA utilisé par le serveur lors de la signature.
            HashAlgorithm hashAlgo = null;
            switch (jwtHeader._alg.Substring(1))
            {
                case "S256":
                    hashAlgo = System.Security.Cryptography.SHA256.Create();
                    break;
                case "S384":
                    hashAlgo = System.Security.Cryptography.SHA384.Create(); break;
                case "S512":
                    hashAlgo = System.Security.Cryptography.SHA512.Create(); break;
                default: break;
            }
            // Abandonner la vérification si l'algorithme de hash n'est pas supporté
            if (hashAlgo == null)
            {
                //lbl_description.Text = "L'algorithme de signature est incorrect !";
                // break;
                res = res + "L'algorithme de signature est incorrect !";
                return res;
            }

            // Reconstituer la donnée signée par le serveur (header.payload)
            var signed = parts[0] + "." + parts[1];
            var dataToVerify = System.Text.Encoding.UTF8.GetBytes(signed);

            bool signatureOK = false;
            switch (sigKey._kty)
            {
                case "RSA":
                    {
                        // Obtenir la clé publique RSA du serveur OpenID
                        var modulus = Base64UrlDecode(sigKey._n);
                        var exponent = Base64UrlDecode(sigKey._e);
                        RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
                        RSAParameters rsaParams = new RSAParameters
                        {
                            Exponent = exponent,
                            Modulus = modulus
                        };
                        csp.ImportParameters(rsaParams);
                        // Vérifier que la signature reçue s'applique effectivement à la donnée signée
                        signatureOK = csp.VerifyData(dataToVerify, hashAlgo, signature);
                        res =  "la signature reçue s'applique effectivement à la donnée signée";

                    }
                    break;
                default:
                    res = res + "L'utilisation de la clé n'est pas correcte !";
                    // lbl_description.Text = "L'utilisation de la clé n'est pas correcte !";
                    break;
            }
            return res ;

        } 
 
            
        





        /*
                public string VerifierSignature(string token)
                {
                    string[] parts = token.Split('.');


                    String Signature = parts[2];

                    // Extraire la signature du jeton d'identité
                    var signature = Base64UrlDecode(Signature);

                    string res = "";




                    var jwtHandler = new JwtSecurityTokenHandler();
                    if (jwtHandler.CanReadToken(token))
                    {
                        var parsedJwt = jwtHandler.ReadJwtToken(token);
                         var header = parsedJwt.Header;

                        JWTHeader jwtHeader = new JWTHeader
                        {
                            _typ = header.Typ,
                            _alg = header.Alg,
                            _kid = header.Kid
                        };

                        if (jwtHeader == null)
                        {
                            res = "jwtHeader == null";
                            return res;
                        }
                        if (jwtHeader._typ != "JWT")
                        {
                            res= "jwtHeader._typ != JWT" ;
                            return res;
                        }
                        if (!jwtHeader._alg.StartsWith("RS"))
                        {
                            res = "!jwtHeader._alg.StartsWith(RS)";
                            return res;
                        }
                        string jwksURL = "https://auth-pp.dgsn.gov.ma/.well-known/jwks.json";
                        RSAJsonWebKeys key = new() { };
                        string error = null;
                        RSAJsonWebKeys.GetIssuerKeys(jwksURL, ref key, ref error);
                        var cle = key.keys[0];
                        bool foundKey = false;
                        bool algoOK = false;
                        bool useOK = false;
                        string Varkey;
                        RSAJsonWebKey sigKey = null;
                        // RSAJsonWebKey key;

                        if (cle._kid == jwtHeader._kid)
                        {
                            foundKey = true;
                            algoOK = (cle._alg == jwtHeader._alg);
                            useOK = cle._use == "sig";
                            if (algoOK && useOK && foundKey)
                            {

                                res = "algoOK && useOK && foundKey";
                                return res;

                            }
                        }
                        // Abandonner la vérification si la clé est inconnue
                        if (!foundKey)
                        {
                            res = "!foundKey";
                            return res;
                        }
                        // Abandonner la vérification si l'algorithme de signature n'est pas supporté.
                        if (!algoOK)
                        {
                            return res;
                        }
                        // Abandonner la vérification s'il ne s'agit pas d'une clé de signature
                        if (!useOK)
                        {
                            return res;
                        }




                        // Identifier l'algorithme SHA utilisé par le serveur lors de la signature.
                        HashAlgorithm hashAlgo = null;
                        switch (header.Alg.Substring(1))
                        {
                            case "S256":
                                hashAlgo = System.Security.Cryptography.SHA256.Create(); 
                                break;
                            case "S384":
                                hashAlgo = System.Security.Cryptography.SHA384.Create(); break;
                            case "S512":
                                hashAlgo = System.Security.Cryptography.SHA512.Create(); break;
                            default: break;
                        }
                        // Abandonner la vérification si l'algorithme de hash n'est pas supporté
                        if (hashAlgo == null)
                        {
                            //lbl_description.Text = "L'algorithme de signature est incorrect !";
                            // break;
                            res= "L'algorithme de signature est incorrect !";
                            return res ; 
                        }

                        // Reconstituer la donnée signée par le serveur (header.payload)
                        var signed = parts[0] + "." + parts[1];
                        var dataToVerify = System.Text.Encoding.UTF8.GetBytes(signed);

                        bool signatureOK = false;
                        switch (sigKey._kty)
                        {
                            case "RSA":
                                {
                                    // Obtenir la clé publique RSA du serveur OpenID
                                    var modulus = Base64UrlDecode(sigKey._n);
                                    var exponent = Base64UrlDecode(sigKey._e);
                                    RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
                                    RSAParameters rsaParams = new RSAParameters
                                    {
                                        Exponent = exponent,
                                        Modulus = modulus
                                    };
                                    csp.ImportParameters(rsaParams);
                                    // Vérifier que la signature reçue s'applique effectivement à la donnée signée
                                     signatureOK = csp.VerifyData(dataToVerify, hashAlgo, signature);
                                    res = "Vérifier que la signature reçue s'applique effectivement à la donnée signée =>"+ signatureOK;

                                }
                                break;
                            default:
                                res = "L'utilisation de la clé n'est pas correcte !";
                                // lbl_description.Text = "L'utilisation de la clé n'est pas correcte !";
                                break;
                        }





                        return res;
                    }
                    else
                    {
                        res = "Return an error if the token can't be read";
                        // Return an error if the token can't be read
                        return res;
                    }
           }


        */



        public string SendUserToDGSN()
        {
            Dictionary<string, Session> m_Sessions = new Dictionary<string, Session>();
            string serverURL = "https://auth-pp.dgsn.gov.ma";
            string locales = "fr";
            string scope = "b135885a-abb7-4e33-8be1-4923b212ced6";
            string client = "c7ab42c2-446e-4881-89b1-6162e50ea204";
            string redirectURL = "https://dgsntest-b3gmaroc.msappproxy.net/authentication/logincb";
            string state = Guid.NewGuid().ToString().Replace("-", "");
            //on va utiliser le DP FACTORY
            Session session = new() {};
            session.state = state;
            m_Sessions[state] = session;


            //   HttpContext.Session.Set(state, session);
            _service.AddSession(session);
            //string nonce = Guid.NewGuid().ToString().Replace("-", "");
            string openIDPerformAuthenticationURL = serverURL;
            openIDPerformAuthenticationURL.Trim('/');
            openIDPerformAuthenticationURL += "/oauth2/auth?";
            openIDPerformAuthenticationURL += "client_id=" + client;
            openIDPerformAuthenticationURL += "&scope=openid+offline+" + scope;
            openIDPerformAuthenticationURL += "&redirect_uri=" + redirectURL;
            openIDPerformAuthenticationURL += "&response_type=code";
            openIDPerformAuthenticationURL += "&ui_locales=" + locales;
            openIDPerformAuthenticationURL += "&state=" + state;
            //openIDPerformAuthenticationURL += "&nonce=" + nonce;
            return openIDPerformAuthenticationURL;
        }

        public HttpStatusCode RevokToken(string token)
        {
            // WellKnownConfig config = new WellKnownConfig() { _revocation_endpoint = "http://localhost:8080/realms/master/protocol/openid-connect/revoke" };
            string revocation_endpoint = "https://auth-pp.dgsn.gov.ma/oauth2/revoke";
            HttpClientHandler handler = new HttpClientHandler();
            HttpClient client = new HttpClient(handler);
            // Header: Authorization basic: fournir l'identifiant et le secret du Fournisseur de Services
            string client_id = "c7ab42c2-446e-4881-89b1-6162e50ea204";
            string client_secret = GetSecret();
            //userSession._secret
            string basicToken = client_id + ":" + client_secret ;
            var basicBytes = System.Text.Encoding.UTF8.GetBytes(basicToken);
            string basic = Convert.ToBase64String(basicBytes);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", basic);
            // Header: Accept */*
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));
            // La requête de révocation doit contenir:
            // - le type du jeton (token_type_hint : bearer)
            // - le jeton d'accès
            string exchangeParameters = "";
            exchangeParameters += HttpUtility.UrlEncode("token_type_hint") + "=" + HttpUtility.UrlEncode("bearer");
            exchangeParameters += "&";
            //recuperer le token d'apres la base de données
            exchangeParameters += HttpUtility.UrlEncode("token") + "=" + HttpUtility.UrlEncode(token);
            // Contenu de la requête : URL encoded parameters
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, revocation_endpoint);
            request.Content = new StringContent(exchangeParameters, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
            // Envoyer la requête POST
            HttpResponseMessage response = client.SendAsync(request).Result;
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                // La révocation a échoué
                // lbl_description.Text = "Impossible de révoquer le jeton OpenID";
                // lbl_description.ForeColor = Color.DarkOrange;
               // return BadRequest("Impossible de révoquer le jeton OpenID");
            }
            //  return null;
             //res = response.StatusCode;
            return response.StatusCode;
        }

        public string GetSecret()
        {
          return  _config.GetConnectionString("secret");
        }
    }
}
