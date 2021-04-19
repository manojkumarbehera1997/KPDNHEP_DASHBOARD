using Newtonsoft.Json;
using RestSharp;
using System.Net.Http;

namespace KPDNHEP.Console.Services.APIs
{
    public class ScimAPI
    {
        HttpClient httpClient { get; set; }
     
        public ScimAPI()
        {
            this.httpClient = new HttpClient();
           
        }
        public T GetAll<T>(string url,string token)
        {

            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "04d7c49a-6341-5636-ed32-33005edd3afc");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("authorization", "Bearer " + token);
            IRestResponse response = client.Execute(request);
            T result = JsonConvert.DeserializeObject<T>(response.Content);
          
            return result;
        }

       

        public T Post<T>(string url,string token,string inum, T data)
        {
      
            var client = new RestClient(url + inum);
            var request = new RestRequest(Method.PUT);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + token);
            request.AddParameter("application/json", JsonConvert.SerializeObject(data), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
           var result = JsonConvert.DeserializeObject<T>(response.Content);

            return result;
        }
        public T Get<T>(string url, string token, string inum, T data)
        {

            var client = new RestClient(url + inum);
            var request = new RestRequest(Method.GET);
            request.AddHeader("content-type", "application/json");
            request.AddHeader("authorization", "Bearer " + token);
            request.AddHeader("cache-control", "no-cache");
            //  request.AddParameter("application/json", JsonConvert.SerializeObject(data), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var result = JsonConvert.DeserializeObject<T>(response.Content);
            

            return result;
        }

    }
}
