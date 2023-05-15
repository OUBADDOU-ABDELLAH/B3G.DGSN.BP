using B3G.DGSN.DOMAIN;
using B3G.DGSN.SERVICE.INTERFACES;
using Microsoft.AspNetCore.Mvc;

namespace B3G.DGSN.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FawriDGSNController : ControllerBase
    {
        private readonly IDGSNFunc _service;
        public FawriDGSNController(IDGSNFunc service)
        {
            _service = service;
        }



        [HttpGet("oauth2/auth")]
        public  IActionResult SendToDGSN()
        {
            var url = _service.SendUserToDGSN();
            return Redirect(url);
        }


        [HttpGet("VeriferSignature")]
        public IActionResult VeriferSignature(string token,string JwksURL)
        {
            var sign = _service.VerifierSignature(token, JwksURL);
            return Ok(sign);
        }




        [HttpGet("/authentication/logincb")]
        public async Task<IActionResult> CallBack()
        {
            string strCode = Request.Query["code"];
            string strState = Request.Query["session_state"];
            string strError = Request.Query["error"];
            string strErrDesc = Request.Query["error_description"];

           var res=_service.RedirectUrl(strCode, strState, strError, strErrDesc);

            return Ok(res);

        }

        [HttpPost("Token")]
        public async Task<IActionResult> GetToken(string code)
        {

           // string secret =  _config.GetValue<string>("secret");

            string token = _service.GetToken(code);
           
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Authentification échouée");
            }

            return Ok(token);
        }

        [HttpPost("RevokToken")]
        public IActionResult RevokToken()
        {
            return Ok();
        }













/*

        [HttpGet("oauth2/auth/getKeys")]
        public IActionResult getKeys()
        {
            RSAJsonWebKeys key = new() { };

            string error = null;

            RSAJsonWebKeys.GetIssuerKeys("https://auth-pp.dgsn.gov.ma/.well-known/jwks.json", ref key, ref error);


            return Ok(new { key.keys[0]._use, key.keys[0]._e, key.keys[0]._kid, key.keys[0]._kty, key.keys[0]._n, key.keys[0]._alg });

        }
        */

    }
}
