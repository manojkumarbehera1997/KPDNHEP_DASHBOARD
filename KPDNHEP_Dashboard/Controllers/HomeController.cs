using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Linq;
using KPDNHEP.Console.Services.Services;
using System.Web;
using System.IdentityModel.Tokens.Jwt;
using KPDNHEP.Console.Services.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;

namespace KPDNHEP.Console.UI.Controllers
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        private readonly IWebHostEnvironment _webHostEnvironment;
        private IConfiguration _configuration;
        public HomeController(IWebHostEnvironment webHostEnvironment, IConfiguration configuration)
        {
            _webHostEnvironment = webHostEnvironment;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            string userType = "";
            userType = User.Claims.Where(p => p.Type == "roles").Select(o => o.Value).FirstOrDefault();
            
            if (userType == "admin")
            {
                return RedirectToAction("list", "user");
            }
            else
            {
                return View();
            }      
        }

        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return RedirectToAction("Index");
        }

        public IActionResult Error()
        {
            return View();
        }
        public IActionResult DemoLogin()
        {
            return View();
        }

        #region Authentication
        public async Task Login(string returnUrl = "/Home/Index", string ui_locales = "")
        {
            HttpContext.Request.Headers.Add("ui_locales", ui_locales);
            await HttpContext.ChallengeAsync("oidc", new AuthenticationProperties() { RedirectUri = returnUrl });
        }
        public IActionResult CustomAuthentication()
        {
            var location = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}");
            var url = location.AbsoluteUri;
            string auth_token = HttpUtility.ParseQueryString(location.Query).Get("auth_token");
            DecodeJwt(auth_token);
            return View();
        }
        public void DecodeJwt(string authToken)
        {
            var stream = authToken;
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;
            string redirectUri = tokenS.Claims.Where(p => p.Type == "redirect_Url").Select(o => o.Value).FirstOrDefault();
        }

        [HttpPost]
        public IActionResult CustomAuthentication(UserAuthentication userAuthentication)
        {
            string redirectUrl = "";
            var issuer = new Uri($"{Request.Scheme}://{Request.Host}");
            UserService userService = new UserService();
            var result = userService.UserLogin(userAuthentication);
            if (result != null)
            {
                string auth_response = GenerateJSONWebToken(result, issuer.ToString());
                redirectUrl = "https://" + _configuration["OIDC:Domain"] + $"/" + _configuration["OIDC:CustomPath"] + $"/" + "landingpage.htm?auth_res=" + auth_response;
            }
           return Redirect(redirectUrl);
        }
        public string GenerateJSONWebToken(UserAuthentication userAuthentication, string issuer)
        {
            string filepath = Path.Combine(_webHostEnvironment.WebRootPath, "iam_private.pem");
            string cert = System.IO.File.ReadAllText(filepath, Encoding.UTF8);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cert));

            var claims = new List<Claim>();
            claims.Add(new Claim("role", userAuthentication.Role));
            claims.Add(new Claim("uuid", userAuthentication.Uuid));
            claims.Add(new Claim("user_name", userAuthentication.UserName));
            claims.Add(new Claim("first_name", userAuthentication.FirstName));
            claims.Add(new Claim("last_name", userAuthentication.LastName));
            claims.Add(new Claim("displayname", userAuthentication.DisplayName));
            claims.Add(new Claim("email", userAuthentication.Email));
            claims.Add(new Claim("mobile", userAuthentication.MobileNumber));

            string token = CreateToken(claims, cert);
            return token;
        }
        public static string CreateToken(List<Claim> claims, string privateRsaKey)
        {
            RSAParameters rsaParams;
            using (var tr = new StringReader(privateRsaKey))
            {
                var pemReader = new PemReader(tr);
                var keyPair = pemReader.ReadObject() as AsymmetricCipherKeyPair;
                if (keyPair == null)
                {
                    throw new Exception("Could not read RSA private key");
                }
                var privateRsaParams = keyPair.Private as RsaPrivateCrtKeyParameters;
                rsaParams = DotNetUtilities.ToRSAParameters(privateRsaParams);
            }
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportParameters(rsaParams);
                Dictionary<string, object> payload = claims.ToDictionary(k => k.Type, v => (object)v.Value);
                return Jose.JWT.Encode(payload, rsa, Jose.JwsAlgorithm.RS256);
            }
        }

        public async Task SessionOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        }

        [Authorize]
        public async Task Logout()
        {
            await HttpContext.SignOutAsync("oidc", new AuthenticationProperties
            {
                RedirectUri = Url.Action("Index", "Home")
            });
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
        #endregion

    }
}
