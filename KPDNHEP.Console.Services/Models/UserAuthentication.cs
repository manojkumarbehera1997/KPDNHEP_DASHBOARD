using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace KPDNHEP.Console.Services.Models
{
public class UserAuthentication
    {
        
        public string Role { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public string MobileNumber { get; set; }

        public string Uuid { get; set; }
    }
}
