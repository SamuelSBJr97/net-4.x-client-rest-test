using ApiClient46.Models.Services;
using ApiClient46.Services;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace ApiClient.Services
{
    public class ApiRestsharpClientService : IApiClientService
    {
        TokenResponse IApiClientService.TokenAuth => tokenResponse;

        private TokenResponse tokenResponse;

        private readonly RestClient restClient;

        private readonly TokenRequest tokenRequest;

        private readonly object locker = new object();

        private JavaScriptSerializer jsSerializer;

        public ApiRestsharpClientService(string uri, string login, string senha)
        {
            restClient = new RestClient(uri);

            tokenRequest = new TokenRequest
            {
                username = login,
                password = senha
            };

            jsSerializer = new JavaScriptSerializer();
        }

        #region Autenticação
        public TokenResponse Autenticar()
        {
            // verifica se o token expirou
            if (tokenResponse == null || DateTime.Now > tokenResponse?.expiresIn)
            {
                // faz um request por vez para obter o token
                lock (locker)
                {
                    // verifica novamente se o token expirou ou vai expirar
                    if (tokenResponse == null || DateTime.Now > tokenResponse?.expiresIn)
                    {
                        var request = new RestRequest("Auth/Login", Method.POST, DataFormat.Json)
                        .AddJsonBody(tokenRequest);

                        var response = restClient.Post(request);

                        tokenResponse = jsSerializer.Deserialize<TokenResponse>(response.Content);
                    }
                }
            }

            return tokenResponse;
        }

        private void InserirBearerToken(RestRequest request)
        {
            request.AddHeader("Authorization", "Bearer " + tokenResponse?.token);
        }
        #endregion

        #region Request helper
        private RestRequest RequestAutenticado(string endpoint, Method method)
        {
            Autenticar();

            var request = new RestRequest(endpoint, method, DataFormat.Json);

            InserirBearerToken(request);

            return request;
        }
        private RestRequest CreateGetRequestAutenticado(string endpoint)
        {
            RestRequest request = RequestAutenticado(endpoint, Method.GET);
            return request;
        }
        private RestRequest CreateBodyRequestAutenticado(string endpoint, Method method, object body)
        {
            var request = RequestAutenticado(endpoint, method);

            request.AddJsonBody(body);

            return request;
        }
        #endregion

        #region Request methods
        private T Get<T>(string endpoint)
        {
            RestRequest request = CreateGetRequestAutenticado(endpoint);

            var response = restClient.Get(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return jsSerializer.Deserialize<T>(response.Content);
            }
            else
            {
                throw new Exception("Error: " + response.StatusDescription);
            }
        }
        private T Post<T, E>(string endpoint, E body)
        {
            var request = CreateBodyRequestAutenticado(endpoint, Method.POST, body);

            var response = restClient.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return jsSerializer.Deserialize<T>(response.Content);
            }
            else
            {
                throw new Exception("Error: " + response.StatusDescription);
            }
        }
        private T Put<T, E>(string endpoint, E body)
        {
            var request = CreateBodyRequestAutenticado(endpoint, Method.PUT, body);

            var response = restClient.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return jsSerializer.Deserialize<T>(response.Content);
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

        public IEnumerable<ApiDataset> GetApiDatasetByKey(string key)
        {
            return Get<IEnumerable<ApiDataset>>("ApiDataset?key=" + SanitizeHelper.SanitizarParametro(key));
        }

        public string GenerateRandom(int total)
        {
            return Post<string, int>("ApiDatasetRandom/GenerateRandom", total);
        }

        public string CriarApiDataset(ApiDataset dataset)
        {
            return Post<string, ApiDataset>("ApiDataset", dataset);
        }

        public string AtualizarApiDataset(ApiDataset dataset)
        {
            return Put<string, ApiDataset>("ApiDataset", dataset);
        }

        public IEnumerable<ApiDataset> GetRandomApiDataset(int total)
        {
            return Get<IEnumerable<ApiDataset>>("ApiDatasetRandom/GetRandomApiDataset?total=" + total);
        }

        #endregion
    }
}