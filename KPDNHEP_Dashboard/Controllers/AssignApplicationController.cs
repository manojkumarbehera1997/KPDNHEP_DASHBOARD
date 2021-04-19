using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KPDNHEP.Console.Services.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using KPDNHEP.Console.Services.Models;
using KPDNHEP.Console.Services.Services;
using RestSharp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace KPDNHEP.Console.UI.Controllers
{
    public class AssignApplicationController : Controller
    {
        private IConfiguration configuration;
        AssignApplicationService assignApplicationService;
        ApplicationService applicationService;
        public AssignApplicationController(IConfiguration iConfiguration)
        {
            configuration = iConfiguration;
        }
        public IActionResult AssignApplication()
        {
            string userName = (string)TempData["username"];
            TempData["User"] = userName;

            PopulateApplicationsView allApplications = new PopulateApplicationsView();
            allApplications.SelectedApplicationList = GetAllApplications();

            PopulateApplicationsView userApplications = new PopulateApplicationsView();
            userApplications.SelectedApplicationList = PopulateApplications(userName);

            foreach (SelectListItem app in allApplications.SelectedApplicationList)
            {
                foreach (var item in userApplications.SelectedApplicationList)
                {
                    if (app.Value == item.Value)
                    {
                        app.Selected = true;
                    }
                }
            }
            return View(allApplications);
        }

        [HttpPost]
        public IActionResult AssignApplication(PopulateApplicationsView populateApplicationsView)
        {
            AssignApplication assignApplication = new AssignApplication();

            string name = (string)TempData["User"];
            populateApplicationsView.SelectedApplicationList = GetAllApplications();
            if (populateApplicationsView.SelectedApplicationId != null && name != "")
            {
                var selectedItems = populateApplicationsView.SelectedApplicationList.Where(p => populateApplicationsView.SelectedApplicationId.Contains(int.Parse(p.Value))).ToList();
                string applist = "";
                ViewBag.Message = "Selected Apps:";
                foreach (var selectedItem in selectedItems)
                {
                    applist += selectedItem.Value + ",";
                    ViewBag.Message += "\\n" + selectedItem.Text;
                }
                applist = applist.TrimEnd(',');
                assignApplication.AssignedApplications = applist;
                assignApplication.UserName = name;

            }
            else if (populateApplicationsView.SelectedApplicationId == null && name != "")
            {
                assignApplication.AssignedApplications = null;
                assignApplication.UserName = name;
            }

            assignApplicationService = new AssignApplicationService();
            var result = assignApplicationService.AssignApplicationToUser(assignApplication);
            return RedirectToAction("list", "User");

            //return View();
        }
        private List<SelectListItem> PopulateApplications(string userName)
        {
            AssignApplication assignApplication = new AssignApplication();
            assignApplication.UserName = userName;
            List<SelectListItem> items = new List<SelectListItem>();

            assignApplicationService = new AssignApplicationService();
            var applications = assignApplicationService.GetUserApplicationIds(assignApplication);

            if (applications.AssignedApplications == null)
            {
                applications.AssignedApplications = "0";
            }
            string[] values = applications.AssignedApplications.ToString().Split(',');
            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim();
            }
            foreach (var item in values)
            {
                items.Add(new SelectListItem
                {
                    Value = item
                });
            }
            return items;
        }

        private List<SelectListItem> GetAllApplications()
        {
            applicationService = new ApplicationService();
            var applications = applicationService.GetAllApplications();

            List<SelectListItem> items = new List<SelectListItem>();
            foreach (var item in applications)
            {
                items.Add(new SelectListItem
                {
                    Text = Convert.ToString(item.ApplicationName),
                    Value = Convert.ToString(item.ApplicationId)
                });
            }
            return items;
        }
    }
}
