using ApiClient46.Models.Services;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
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

        private readonly RestClient restClient;

        private readonly TokenRequest tokenRequest;

        private readonly object locker = new object();

        public ApiServerClientService(string uri, string login, string senha)
        {
            restClient = new RestClient(uri);

            tokenRequest = new TokenRequest
            {
                username = login,
                password = senha
            };
        }

        #region Autenticação
        public void Autenticar()
        {

            // verifica se o token expirou
            if (DateTime.Now >= tokenResponse?.ExpiresIn)
            {
                // faz um request por vez para obter o token
                lock (locker)
                {
                    // verifica novamente se o token expirou ou vai expirar
                    if (DateTime.Now >= tokenResponse?.ExpiresIn.AddMinutes(-1))
                    {
                        var request = new RestRequest("Auth/Login", Method.POST, DataFormat.Json)
                        .AddBody(tokenRequest);

                        var response = restClient.Post(request);

                        tokenResponse = SimpleJson.DeserializeObject<TokenResponse>(response.Content);
                    }
                }
            }
        }

        private void SetAuthorizationHeader(RestRequest request)
        {
            if (tokenResponse != null)
            {
                request.AddHeader("Authorization", "Bearer " + tokenResponse.AccessToken);
            }
        }
        #endregion

        #region Request helper
        private RestRequest CreateRequest(string endpoint, Method method)
        {
            Autenticar();

            var request = new RestRequest(endpoint, method, DataFormat.Json);

            SetAuthorizationHeader(request);

            return request;
        }
        private RestRequest CreateGetRequest(string endpoint)
        {
            RestRequest request = CreateRequest(endpoint, Method.GET);
            return request;
        }
        private RestRequest CreateBodyRequest(string endpoint, Method method, ApiDataset body)
        {
            var request = CreateRequest(endpoint, method);

            request.AddJsonBody(body);

            return request;
        }
        #endregion

        #region Request methods
        public T Get<T>(string endpoint)
        {
            RestRequest request = CreateGetRequest(endpoint);

            var response = restClient.Get(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return SimpleJson.DeserializeObject<T>(response.Content);
            }
            else
            {
                throw new Exception("Error: " + response.StatusDescription);
            }
        }
        public T Post<T>(string endpoint, ApiDataset body)
        {
            var request = CreateBodyRequest(endpoint, Method.POST, body);

            var response = restClient.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return SimpleJson.DeserializeObject<T>(response.Content);
            }
            else
            {
                throw new Exception("Error: " + response.StatusDescription);
            }
        }
        public T Put<T>(string endpoint, ApiDataset body)
        {
            var request = CreateBodyRequest(endpoint, Method.PUT, body);

            var response = restClient.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return SimpleJson.DeserializeObject<T>(response.Content);
            }
            else
            {
                throw new Exception("Error: " + response.StatusDescription);
            }
        }
        #endregion

        #region Api metodos

        public IEnumerable<ApiDataset> GetAllApiDataset()
        {
            return Get<IEnumerable<ApiDataset>>("ApiDataset");
        }

        #endregion
    }
}