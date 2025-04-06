using ApiClient46.Models.Services;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace ApiClient.Services
{
    public class ApiServerClientService
    {
        public static ApiServerClientService API_CLIENT_SERVICE { get; private set; }

        static ApiServerClientService()
        {
            string uri = ConfigurationManager.AppSettings["HOST_API"];
            string login = ConfigurationManager.AppSettings["HOST_API_USER"];
            string senha = ConfigurationManager.AppSettings["HOST_API_PASS"];

            API_CLIENT_SERVICE = new ApiServerClientService(uri, login, senha);
        }

        private TokenResponse tokenResponse;

        private RestClient restClient;

        private TokenRequest tokenRequest;

        public ApiServerClientService(string uri, string login, string senha)
        {
            restClient = new RestClient(uri);

            tokenRequest = new TokenRequest
            {
                username = login,
                password = senha
            };
        }

        private void Autenticar()
        {
            // verifica se o token expirou ou vai expirar nos proximos 5 minutos
            if (DateTime.Now >= tokenResponse?.ExpiresIn.AddMinutes(-5))
            {
                var request = new RestRequest("Auth/Login", Method.POST, DataFormat.Json)
                    .AddBody(tokenRequest);

                var response = restClient.Post(request);

                tokenResponse = SimpleJson.DeserializeObject<TokenResponse>(response.Content);
            }
        }
    }
}