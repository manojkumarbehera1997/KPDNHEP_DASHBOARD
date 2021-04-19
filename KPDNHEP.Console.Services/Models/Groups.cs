using Newtonsoft.Json;

namespace KPDNHEP.Console.Services.Models
{
    public class Groups
    {
        [JsonProperty("value")]
        public string Value { get; set; }



        [JsonProperty("display")]
        public string Display { get; set; }



        [JsonProperty("type")]
        public string Type { get; set; }
    }
}
