using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace KPDNHEP.Console.Services.Models
{
    public class OIDCCallbackResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }

        [JsonProperty("session_id")]
        public string SessionId { get; set; }

        [JsonProperty("session_state")]
        public string SessionState { get; set; }
    }
}
