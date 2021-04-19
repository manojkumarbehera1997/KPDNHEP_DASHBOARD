using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KPDNHEP.Console.Services.Models
{
    public class UserPassword
    {
        [JsonProperty("id")]
        public string Id { get; set; }



        [JsonProperty("schemas")]
        public List<string> Schemas { get; set; }



        [JsonProperty("password")]
        public string Password { get; set; }



        [JsonProperty("active")]
        public bool Active { get; set; }
    }
}
