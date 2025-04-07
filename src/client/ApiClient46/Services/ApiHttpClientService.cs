using ApiClient.Services;
using ApiClient46.Models.Services;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Script.Serialization;

namespace ApiClient46.Services
{
    public class ApiHttpClientService : IApiClientService
    {
        public TokenResponse TokenAuth => tokenResponse;

        private TokenResponse tokenResponse;

        private readonly HttpClient restClient;

        private readonly TokenRequest tokenRequest;

        private readonly object locker = new object();

        public ApiHttpClientService(string uri, string login, string senha)
        {
            restClient = new HttpClient()
            {
                BaseAddress = new Uri(uri),
            };

            tokenRequest = new TokenRequest
            {
                username = login,
                password = senha
            };

            ServicePointManager.DefaultConnectionLimit = 1000;
        }

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
                        var response = restClient.PostAsJsonAsync("Auth/Login", tokenRequest).Result;

                        response.EnsureSuccessStatusCode();

                        tokenResponse = response.Content.ReadAsAsync<TokenResponse>().Result;

                        restClient.DefaultRequestHeaders.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tokenResponse.token);
                    }
                }
            }

            return tokenResponse;
        }
        public string AtualizarApiDataset(ApiDataset dataset)
        {
            Autenticar();

            var response = restClient.PutAsJsonAsync("ApiDataset", dataset).Result;

            return response.Content.ReadAsStringAsync().Result;
        }
        public string CriarApiDataset(ApiDataset dataset)
        {
            Autenticar();

            var response = restClient.PostAsJsonAsync("ApiDataset", dataset).Result;

            return response.Content.ReadAsStringAsync().Result;
        }

        public string GenerateRandom(int total)
        {
            Autenticar();

            var response = restClient.PostAsJsonAsync("ApiDatasetRandom/GenerateRandom", total).Result;

            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsStringAsync().Result;
        }

        public IEnumerable<ApiDataset> GetAllApiDataset()
        {
            Autenticar();

            var response = restClient.GetAsync("ApiDataset").Result;

            return response.Content.ReadAsAsync<IEnumerable<ApiDataset>>().Result;
        }

        public IEnumerable<ApiDataset> GetApiDatasetByKey(string key)
        {
            Autenticar();

            var response = restClient.GetAsync($"ApiDataset?key={SanitizeHelper.SanitizarParametro(key)}").Result;

            return response.Content.ReadAsAsync<IEnumerable<ApiDataset>>().Result;
        }

        public IEnumerable<ApiDataset> GetRandomApiDataset(int total)
        {
            Autenticar();

            var response = restClient.GetAsync($"ApiDatasetRandom/GetRandomApiDataset?total={total}").Result;

            return response.Content.ReadAsAsync<IEnumerable<ApiDataset>>().Result;
        }
    }
}