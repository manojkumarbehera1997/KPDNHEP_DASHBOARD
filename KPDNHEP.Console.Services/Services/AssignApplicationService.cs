using KPDNHEP.Console.Services.APIs;
using KPDNHEP.Console.Services.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace KPDNHEP.Console.Services.Services
{
    public class AssignApplicationService
    {
        ConfigurationService configurationService = new ConfigurationService();
        string baseUrl;
        API api = new API();
        public AssignApplicationService()
        {
            baseUrl=configurationService.GetService();
        }

        public AssignApplication AssignApplicationToUser(AssignApplication assignApp)
        {
            return api.Post(String.Format("{0}/AssignedApplication/AssignApplicationsToUser", baseUrl), assignApp);
        }
        public AssignApplication GetUserApplicationIds(AssignApplication assignApplication)
        {
            return api.Post(String.Format("{0}/AssignedApplication/GetUserAssignedApplications", baseUrl), assignApplication);
        }
    }
}
