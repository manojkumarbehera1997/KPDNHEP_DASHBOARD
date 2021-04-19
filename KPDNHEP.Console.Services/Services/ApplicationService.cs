using KPDNHEP.Console.Services.APIs;
using KPDNHEP.Console.Services.Models;
using KPDNHEP.Console.Services.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace KPDNHEP.Console.Services.Services
{
    public class ApplicationService
    {
        ConfigurationService configurationService = new ConfigurationService();
        API api = new API();
        string baseUrl;
        public ApplicationService()
        {
            baseUrl=configurationService.GetService();
        }
        public List<ApplicationsView> GetAllApplications()
        {
            return api.GetAll<ApplicationsView>(String.Format("{0}/Application/GetAllApplications",baseUrl));
        }
        public List<ApplicationsView> GetUserApplications(string userName)
        {
            return api.GetAll<ApplicationsView>(String.Format("{0}/Application/GetUserApplications/{1}", baseUrl,userName));
        }
        public List<ApplicationsView> GetUserGroupApplications(string userName,string groupName)
        {
            return api.GetAll<ApplicationsView>(String.Format("{0}/Application/GetApplicationsByGroup/{1},{2}",baseUrl, userName,groupName));
        }
        public ApplicationsView AddNewApplication(ApplicationsView applicationsView)
        {
            return api.Post<ApplicationsView>(String.Format("{0}/Application/AddApplication",baseUrl), applicationsView);
        }


    }
}
