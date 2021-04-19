using KPDNHEP.Console.Services.ViewModels;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace KPDNHEP.Console.Services.Models
{
    public class Users
    {
        [JsonProperty("schemas")]
        public List<string> Schemas { get; set; }



        [JsonProperty("totalResults")]
        public int TotalResults { get; set; }



        [JsonProperty("startIndex")]
        public int StartIndex { get; set; }



        [JsonProperty("itemsPerPage")]
        public int ItemsPerPage { get; set; }



        public List<UserProfileView> Resources { get; set; }
    }
}
