using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace B3G.DGSN.SERVICE.INTERFACES
{
    public interface IDGSNFunc
    {
        public string GetSecret();
        public string VerifierSignature(string token, string JwksURL);
        public string GetToken(string code);
        public string SendUserToDGSN();
        public string RedirectUrl(string code,string state,string codeError,string ErrorDescription);
       // public dynamic GetIssuerKeys(string JwksURL);
        public HttpStatusCode RevokToken(string token);


    }
}
