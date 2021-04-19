using KPDNHEP.Console.Services.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KPDNHEP.Console.Services.ViewModels
{
    public class UserProfileView
    {
        [JsonProperty("schemas")]
        public List<string> Schemas { get; set; }



        [JsonProperty("id")]
        public string Id { get; set; }



        [JsonProperty("userName")]
        [Display(Name = "Username")]
        public string UserName { get; set; }



        [JsonProperty("name")]
        public Name Name { get; set; }



        [JsonProperty("displayName")]
        [Display(Name = "Display Name")]
        public string DisplayName { get; set; }



        [JsonProperty("emails")]
        [Display(Name = "Email")]
        public List<Email> Emails { get; set; }



        [JsonProperty("active")]
        public bool Active { get; set; }



        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
