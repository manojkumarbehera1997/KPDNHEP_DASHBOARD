using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KPDNHEP.Console.Services.ViewModels
{
    public class ApplicationsView
    {
        public int ApplicationId { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        public string ApplicationName { get; set; }

        public string ApplicationIcon { get; set; }
        [Required(ErrorMessage = "Icon is required")]
        [Display(Name = "Icon")]
        public IFormFile ApplicationIconFile { get; set; }

        [Required(ErrorMessage = "Url is required")]
        [Display(Name = "Url")]
        public string ApplicationUrl { get; set; }
        [Display(Name = "Description")]
        public string ApplicationDescription { get; set; }
    }
}
