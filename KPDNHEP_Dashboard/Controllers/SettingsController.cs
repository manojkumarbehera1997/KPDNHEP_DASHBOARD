using KPDNHEP.Console.Services.Models;
using KPDNHEP.Console.Services.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KPDNHEP.Console.UI.Controllers
{
    public class SettingsController : Controller
    {
        private IConfiguration _configuration;
        SettingService settingService;
        public SettingsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// Edit Profile settings to user
        /// </summary>
        /// <returns></returns>
        public IActionResult EditProfileSettings()
        {
            return View();
        }
        public bool GetEditProfileSettings()
        {
            settingService = new SettingService();
            return settingService.GetProfileSetting();
        }
        
        public EditProfileConfiguration SetEditProfileSettings(EditProfileConfiguration editProfileConfiguration)
        {
            settingService = new SettingService();
            return settingService.SetProfileSetting(editProfileConfiguration);
        }
    }
}
