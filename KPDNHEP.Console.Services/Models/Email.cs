using Newtonsoft.Json;

namespace KPDNHEP.Console.Services.Models
{
    public class Email
    {
        
            [JsonProperty("value")]
            public string Value { get; set; }



            [JsonProperty("type")]
            public string Type { get; set; }



            [JsonProperty("primary")]
            public bool Primary { get; set; }
        
    }
}
