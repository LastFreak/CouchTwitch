using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CouchTwitch
{
    class ApiHelper
    {
        public static async Task<object> apiRequest<T>(string url)
        {
            Uri getUri = new Uri(url);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(getUri);
            string responseString = await response.Content.ReadAsStringAsync();
            object responseObject = JsonConvert.DeserializeObject<T>(responseString);
            return responseObject;
        }
        public static async Task<string> getJsonAsString(string url)
        {
            Uri getUri = new Uri(url);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(getUri);
            string responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }
    }
}
