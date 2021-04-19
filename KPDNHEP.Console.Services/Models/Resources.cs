using Newtonsoft.Json;
using System.Collections.Generic;

namespace KPDNHEP.Console.Services.Models
{
    public class Resources
    {
        [JsonProperty("schemas")]
        public List<string> Schemas { get; set; }



        [JsonProperty("id")]
        public string Id { get; set; }



        [JsonProperty("meta")]
        public Meta Meta { get; set; }



        [JsonProperty("userName")]
        public string UserName { get; set; }



        [JsonProperty("name")]
        public Name Name { get; set; }



        [JsonProperty("displayName")]
        public string DisplayName { get; set; }



        [JsonProperty("nickName")]
        public string NickName { get; set; }



        [JsonProperty("timezone")]
        public string TimeZone { get; set; }



        [JsonProperty("active")]
        public bool Active { get; set; }



        [JsonProperty("email")]
        public List<Email> Email { get; set; }



        [JsonProperty("groups")]
        public List<Groups> Groups { get; set; }
    }
}
