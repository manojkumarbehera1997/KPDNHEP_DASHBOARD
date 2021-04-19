using KPDNHEP.Console.Services.APIs;
using KPDNHEP.Console.Services.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KPDNHEP.Console.Services.Services
{
    public class SettingService
    {
        ConfigurationService configurationService = new ConfigurationService();
        API api = new API();
        string baseUrl;
        public SettingService()
        {
            baseUrl=configurationService.GetService();
        }
        public bool GetProfileSetting()
        {
            return api.Get<bool>(string.Format("{0}/Setting/GetEditProfileSettings/", baseUrl));
        }
        public EditProfileConfiguration SetProfileSetting(EditProfileConfiguration editProfileConfiguration)
        {
            return api.Post(String.Format("{0}/Setting/UpdateEditProfileSettings", baseUrl), editProfileConfiguration);
        }
    }
}
