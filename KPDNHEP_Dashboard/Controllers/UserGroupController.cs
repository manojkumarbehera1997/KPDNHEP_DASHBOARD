using KPDNHEP.Console.Services.Models;
using KPDNHEP.Console.Services.Services;
using KPDNHEP.Console.Services.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace KPDNHEP.Console.UI.Controllers
{
    public class UserGroupController : Controller
    {
        private ApplicationService applicationService;
        private UserGroupService userGroupService;
        
        public IActionResult ApplicationGroup()
        {
 
            string userName = User.Claims.Where(p => p.Type == "user_name").Select(o => o.Value).FirstOrDefault();

            userGroupService = new UserGroupService();
            var groups = userGroupService.GetUserGroups(userName);

            //TODO: Remove later
            //applicationService = new ApplicationService();
            //var applications = applicationService.GetUserGroupApplications(userName, groupName);
            //ApplicationsView[] applicationsViews = applications.ToArray();
            //ViewData["Applications"] = applicationsViews;

            UserGroup[] userGroups = groups.ToArray();
            ViewData["Groups"] = userGroups;

            return View();
        }
        public ApplicationsView[] GetApplications()
        {
            string groupName = "ALL";
            string userName = User.Claims.Where(p => p.Type == "user_name").Select(o => o.Value).FirstOrDefault();
            applicationService = new ApplicationService();
            var applications = applicationService.GetUserGroupApplications(userName, groupName);
            return applications.ToArray();
        }

        public List<ApplicationsView> GetUserGroup(string userName, string groupName)
        {
            ApplicationService applicationService = new ApplicationService();
            var applications = applicationService.GetUserGroupApplications(userName, groupName);

            return applications;
        }
        public UserGroupApplication UpdateApplicationGroup(UserGroupApplication userGroupApplication)
        {
            UserGroupService userGroupService = new UserGroupService();
            var userapplication = userGroupService.UpdateApplicationGroups(userGroupApplication);
            return userapplication;
        }
        public UserGroup CreateGroup(UserGroup userGroup)
        {
            UserGroupService userGroupService = new UserGroupService();
            var createGroups = userGroupService.CreateGroup(userGroup);
            return createGroups;
        }
        public UserGroup DeleteGroup(UserGroup userGroup)
        {
            UserGroupService userGroupService = new UserGroupService();
            var createGroups = userGroupService.DeleteGroup(userGroup);
            return createGroups;
        }
    }
}
