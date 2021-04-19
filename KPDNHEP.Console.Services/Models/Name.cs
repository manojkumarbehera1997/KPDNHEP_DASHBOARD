using Newtonsoft.Json;

namespace KPDNHEP.Console.Services.Models
{
    public class Name
    {
        [JsonProperty("familyName")]
        public string FamilyName { get; set; }



        [JsonProperty("givenName")]
        public string GivenName { get; set; }
    }
}
