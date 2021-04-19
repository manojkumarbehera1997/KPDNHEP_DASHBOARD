using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KPDNHEP.Console.Services.ViewModels;
using KPDNHEP.Console.Services.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using KPDNHEP.Console.Services.Services;

namespace KPDNHEP.Console.UI.Controllers
{
    public class UserController : Controller
    {

        
        string url = "";
        string clientid = "";
        string clientsecret = "";
        private IConfiguration _configuration;
        GetToken getToken = new GetToken();
        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
            url = _configuration["ApiUrl:SCIM"];
            clientid = _configuration["SCIM:ClientId"];
            clientsecret = _configuration["SCIM:ClientSecret"];

        }
        
        public ActionResult List()
        
        {         
            ScimService scimService = new ScimService();
            var result = scimService.UsersLIst();
            List<Users> userList = new List<Users>();
            userList.Add(result);
            return View(userList);
        }

        public ActionResult AssignToUser(string username)
        {
            TempData["username"] = username;
            TempData.Keep();
            return RedirectToAction("AssignApplication", "AssignApplication");
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePassword(PasswordView passwordView)
        {
        string inum = User.Claims.Where(p => p.Type == "inum").Select(o => o.Value).FirstOrDefault();
            if (ModelState.IsValid)
            {
                UserPassword password = new UserPassword();
                password.Active = true;
                password.Schemas = new List<string>();
                password.Schemas.Add("urn:ietf:params:scim:schemas:core:2.0:User");
                password.Password = passwordView.Password;                 
                ScimService scimService = new ScimService();
                var result = scimService.ChangePassword(inum, password);
                TempData["message"] = "Your Password Changed Sucessfully";
                return RedirectToAction("UserApplications", "Application");
            }
            return View();
        }

        public ActionResult UpdateProfile()
        {          
            UserProfileView userProfileView = new UserProfileView();
            string inum = User.Claims.Where(p => p.Type == "inum").Select(o => o.Value).FirstOrDefault();          
            userProfileView.Active = true;
            userProfileView.Schemas = new List<string>();
            userProfileView.Schemas.Add("urn:ietf:params:scim:schemas:core:2.0:User");
            ScimService scimService = new ScimService();
            var result = scimService.UsersProfile(inum, userProfileView);
            return View(result);
        }

        [HttpPost]
        public IActionResult UpdateProfile(UserProfileView userProfileView)
        {
          
            if (ModelState.IsValid)
            {
                var password = User.Claims.Where(p => p.Type == "password").Select(o => o.Value).FirstOrDefault();
                string inum = User.Claims.Where(p => p.Type == "inum").Select(o => o.Value).FirstOrDefault();
                userProfileView.Active = true;
                userProfileView.Schemas = new List<string>();
                userProfileView.Schemas.Add("urn:ietf:params:scim:schemas:core:2.0:User");
                userProfileView.Password = password;              
                ScimService scimService = new ScimService();
                var result = scimService.UpdateProfile(inum,userProfileView);
                TempData["message"] = "Updated Sucessfully";
                return RedirectToAction("UserApplications", "Application");
            }
            return View();
        }

        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }

        private string GetTokenUrl()
        {
            string tokenClientUrl = $"https://" + _configuration["OIDC:Domain"] + "/" + _configuration["OIDC:CustomPath"] + "/restv1/token";
            return tokenClientUrl;
        }
    }
}