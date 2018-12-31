using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FaciTApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FaciTApp.Services
{
    public class ApiServices
    {
        public async Task<bool> RegisterUser(string email, string password, string confirmPassword)
        {
            var registerModel = new RegisterModel()
            {
                Email = email,
                Password = password,
                ConfirmPassword = confirmPassword
            };
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(registerModel);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync("https://facit.azurewebsites.net/api/Account/Register", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> LoginUser(string email, string password)
        {
            var keyvalues = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("username",email),
                new KeyValuePair<string, string>("password",password),
                new KeyValuePair<string, string>("grant_type","password"),
            };
            var request = new HttpRequestMessage(HttpMethod.Post, "https://facit.azurewebsites.net/Token");
            request.Content = new FormUrlEncodedContent(keyvalues);
            var httpClient = new HttpClient();
            var response = await httpClient.SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            JObject jObject = JsonConvert.DeserializeObject<dynamic>(content);
            var accessToken = jObject.Value<string>("access_token");
            Settings.AccessToken = accessToken;
            Settings.UserName = email;
            Settings.Password = password;
            return response.IsSuccessStatusCode;
        }

        private static dynamic GetJObject(string content)
        {
            return JsonConvert.DeserializeObject<dynamic>(content);
        }

        public async Task<List<FaciTUser>> FindFacit(string region, string facitType)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", "Settings.AccessToken");
            var facitApiUrl = "https://facit.azurewebsites.net/api/FaciTUsers";
            var json = await httpClient.GetStringAsync($"{facitApiUrl}?facitType={facitType}&region={region}");
            return JsonConvert.DeserializeObject<List<FaciTUser>>(json);
        }

        public async Task<List<FaciTUser>> LatestUser()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", "Settings.AccessToken");
            var facitApiUrl = "https://facit.azurewebsites.net/api/FaciTUsers";
            var json = await httpClient.GetStringAsync(facitApiUrl);
            return JsonConvert.DeserializeObject<List<FaciTUser>>(json);
        }

        public async Task<bool> RegisterComplaints(FaciTUser facitUser)
        {
            var json = JsonConvert.SerializeObject(facitUser);
            var httpClient = new HttpClient();
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", "Settings.AccessToken");
            var facitApiUrl = "https://facit.azurewebsites.net/api/FaciTUsers";
            var response = await httpClient.PostAsync(facitApiUrl, content);
            return response.IsSuccessStatusCode;
        }


    }
}
