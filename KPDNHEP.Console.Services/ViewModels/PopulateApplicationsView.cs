using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KPDNHEP.Console.Services.ViewModels
{
    public class PopulateApplicationsView
    {
        public ApplicationsView Application { get; set; }
        public List<int> SelectedApplicationId { get; set; }
        public List<SelectListItem> SelectedApplicationList { get; set; }
    }
}
