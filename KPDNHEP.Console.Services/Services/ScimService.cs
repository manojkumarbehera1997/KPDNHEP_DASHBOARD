using KPDNHEP.Console.Services.APIs;
using KPDNHEP.Console.Services.Models;
using KPDNHEP.Console.Services.ViewModels;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace KPDNHEP.Console.Services.Services
{
    public  class ScimService
    {
        ScimAPI scimApi;
        private readonly IConfiguration Configuration;
        private readonly string url;
       
        private readonly string token;
        GetToken getToken = new GetToken();
        public ScimService()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            string tokenClientUrl = $"https://" + Configuration["OIDC:Domain"] + "/" + Configuration["OIDC:CustomPath"] + "/restv1/token";
            var scim_api_endpoint = $"{Configuration["ApiUrl:SCIM"]}";
            string clientId = $"{Configuration["SCIM:ClientId"]}";
            string clientSecret = $"{Configuration["SCIM:ClientSecret"]}";
            token = getToken.GenerateToken(tokenClientUrl,clientId, clientSecret);
            scimApi = new ScimAPI();
            url = scim_api_endpoint;
        }

        public Users UsersLIst()
        {
            return scimApi.GetAll<Users>(url,token);
        }
        public UserProfileView UsersProfile(string inum, UserProfileView userProfileView)
        {
            return scimApi.Get<UserProfileView>(url, token, inum, userProfileView);
        }
        public UserPassword ChangePassword(string inum, UserPassword password)
        {
            return scimApi.Post<UserPassword>(url, token, inum, password);
        
        }
        public UserProfileView UpdateProfile(string inum,UserProfileView userProfileView)
        {
            return scimApi.Post<UserProfileView>(url, token, inum, userProfileView);

        }


    }
}
