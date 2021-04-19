using KPDNHEP.Console.Services.Services;
using KPDNHEP.Console.Services.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;

namespace KPDNHEP.Console.UI.Controllers
{
    public class ApplicationController : Controller
    {
        private IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;
        ApplicationService applicationService;
        public ApplicationController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize]
        public IActionResult UserApplications()
        {
            string userName = "";
            userName = User.Claims.Where(p => p.Type == "user_name").Select(o => o.Value).FirstOrDefault();

            applicationService = new ApplicationService();
            ApplicationsView[] myArray;
            if (userName == "admin")
            {
                var model = applicationService.GetAllApplications();
                myArray = model.ToArray();
            }
            else
            {
                var model = applicationService.GetUserApplications(userName);
                myArray = model.ToArray();
            }
            
            ViewData["Applications"] = myArray;           
            return View();
        }
        public ActionResult AddApplication()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddApplication(ApplicationsView applicationsView)
        {
            string stringFileName = UploadFile(applicationsView.ApplicationIconFile);

            if (ModelState.IsValid)
            {
                ApplicationsView newApplication = new ApplicationsView();
                newApplication.ApplicationName = applicationsView.ApplicationName;
                newApplication.ApplicationIcon = stringFileName;
                newApplication.ApplicationUrl = applicationsView.ApplicationUrl;
                newApplication.ApplicationDescription = applicationsView.ApplicationDescription;

                applicationService = new ApplicationService();
                var model = applicationService.AddNewApplication(applicationsView);
                if (Convert.ToString(model.ApplicationId) !=null )
                {
                    TempData["message"] = "Application added Sucessfully";
                }
                
                return RedirectToAction("UserApplications");

            }
            return View();
        }
        private string UploadFile(IFormFile appIcon)
        {
            string filePath = null;
            if (appIcon != null)
            {
                string uploedDir = Path.Combine(_webHostEnvironment.WebRootPath, "Images//Dashboard");
                filePath = Guid.NewGuid().ToString() + "-" + appIcon.FileName;
                string filepath = Path.Combine(uploedDir, filePath);
                using (var fileStream = new FileStream(filepath, FileMode.Create))
                {
                    appIcon.CopyTo(fileStream);
                }
            }
            return filePath;
        }    
    }
}

