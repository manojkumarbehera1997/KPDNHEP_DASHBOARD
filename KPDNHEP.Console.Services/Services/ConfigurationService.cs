using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace KPDNHEP.Console.Services.Services
{
    public class ConfigurationService
    {
        public IConfiguration Configuration;
        public string baseUrl;

        public string GetService()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            var api_endpoint = $"{Configuration["APIEndPoint"]}";
            baseUrl = api_endpoint;
            return baseUrl;
        }
    }
}
