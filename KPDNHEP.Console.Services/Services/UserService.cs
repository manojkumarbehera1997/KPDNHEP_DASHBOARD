using KPDNHEP.Console.Services.APIs;
using KPDNHEP.Console.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace KPDNHEP.Console.Services.Services
{
  public  class UserService
    {
        ConfigurationService configurationService = new ConfigurationService();
        string baseUrl;
        API api = new API();



        public UserService()
        {
            baseUrl = configurationService.GetService();



        }
        public UserAuthentication UserLogin(UserAuthentication userAuthentication)
        {
            return api.Post<UserAuthentication>(String.Format("{0}/User/UserLogin", baseUrl), userAuthentication);
        }

    }
}
