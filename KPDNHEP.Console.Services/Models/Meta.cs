using Newtonsoft.Json;

namespace KPDNHEP.Console.Services.Models
{
    public class Meta
    {
        [JsonProperty("resourceType")]
        public string ResourceType { get; set; }



        [JsonProperty("lastModified")]
        public string LastModified { get; set; }



        [JsonProperty("location")]
        public string Location { get; set; }
    }
}
