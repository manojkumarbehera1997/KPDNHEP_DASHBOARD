using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace KPDNHEP.Console.Services.APIs
{
    public class API
    {
        HttpClient httpClient { get; set; }

        public API()
        {
            this.httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Add("Token", token);
        }

        public List<T> GetAll<T>(string url)
        {
            var response = httpClient.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var apiresponse = response.Content.ReadAsStringAsync().Result;
                List<T> result = JsonConvert.DeserializeObject<List<T>>(apiresponse);
                return result;
            }
            return null;
        }

        public T Get<T>(string url)
        {
            var response = httpClient.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var apiresponse = response.Content.ReadAsStringAsync().Result;
                T result = JsonConvert.DeserializeObject<T>(apiresponse);
                return result;

            }
            return default(T);
        }

        public T Post<T>(string url, T data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                var apiresponse = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<T>(apiresponse);
                return result;
            }

            return default(T);
        }

        public int Delete(string url)
        {
            var response = httpClient.DeleteAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var apiresponse = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<int>(apiresponse);
                return result;
            }

            return 0;
        }


    }
}
