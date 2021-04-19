using Newtonsoft.Json;
using RestSharp;


namespace KPDNHEP.Console.Services.Models
{
    public class GetToken
    {
        public string GenerateToken(string url,string clientId,string clientSecret)
        {

            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            var encodedData = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(clientId + ":" + clientSecret));
            request.AddHeader("Authorization", "Basic " + encodedData);
            // request.AddHeader("Authorization", "Basic NmM0NWMzMGQtM2Q2NS00NDgzLWI1ZDctMDdhNDA3MWJlN2MwOnZ5ZW9mZDU3S0prSUl2VU1rWVBMVHFTUw==");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            //request.AddHeader("Cookie", "org.gluu.i18n.Locale=en");
            request.AddParameter("grant_type", "client_credentials");
            IRestResponse response = client.Execute(request);

            var result = JsonConvert.DeserializeObject<Token>(response.Content);
            string token = result.AccessToken;

            return token;
        }
    }
}
