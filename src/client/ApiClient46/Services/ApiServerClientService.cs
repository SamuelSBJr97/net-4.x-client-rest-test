using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace ApiClient.Services
{
    public class ApiServerClientService
    {
        public static ApiServerClientService CLIENT_SERVICE { get; private set; }

        static ApiServerClientService()
        {
            CLIENT_SERVICE = new ApiServerClientService("https://localhost:7216", "usuario", "senha");
        }

        private TokenResponse tokenResponse;

        private RestClient restClient;

        public ApiServerClientService(string uri, string login, string senha)
        {
            restClient = new RestClient(uri);
        }

        private void Autenticar()
        {
            if (DateTime.Now >= tokenResponse?.ExpiresIn.AddMinutes(10))
            {
                var response = httpClient.PostAsJsonAsync("Auth/Login", new { username = "usuario", password = "senha" }).Result;

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    tokenResponse = response.Content.ReadAsAsync<TokenResponse>().Result;
                }
            }
        }
    }
}