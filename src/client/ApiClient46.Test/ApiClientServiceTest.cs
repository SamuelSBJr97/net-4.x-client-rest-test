using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ApiClient.Services;
using Newtonsoft.Json;
using ApiClient46.Models.Services;
using System.Data;
using System.Configuration;
using ApiClient46.Services;
using Microsoft.Testing.Platform.Extensions.Messages;

namespace ApiClient46.Test
{
    [TestClass]
    public sealed class ApiClientServiceTest
    {
        private readonly IApiClientService apiClientService;

        public ApiClientServiceTest()
        {
            string apiBaseUrl = ConfigurationManager.AppSettings["ApiBaseUrl"];
            string apiUser = ConfigurationManager.AppSettings["ApiUser"];
            string apiPassword = ConfigurationManager.AppSettings["ApiPassword"];

            if (ConfigurationManager.AppSettings["ApiTest"].Equals("HttpClient"))
            {
                apiClientService = new ApiHttpClientService(apiBaseUrl, apiUser, apiPassword);
            }
            else if (ConfigurationManager.AppSettings["ApiTest"].Equals("Restsharp"))
            {
                apiClientService = new ApiRestsharpClientService(apiBaseUrl, apiUser, apiPassword);
            }

            Assert.IsNotNull(apiClientService, "API client service is null");
        }

        [TestMethod]
        public void TestParallelRequestsAutenticar()
        {
            Parallel.For(0, 1000, x =>
            {
                var tokenResponse = apiClientService.Autenticar();

                Parallel.For(0, 1000, i =>
                {
                    if (tokenResponse.expiresIn > DateTime.Now)
                    {
                        Thread.Sleep(tokenResponse.expiresIn.Value.Subtract(DateTime.Now));
                    }

                    var result = apiClientService.Autenticar();

                    Assert.IsNotNull(result);

                    if (tokenResponse.expiresIn > DateTime.Now)
                    {
                        Assert.IsTrue(result.token.Equals(tokenResponse.token));

                        Assert.IsTrue(result.token.Equals(apiClientService.TokenAuth.token));
                    }
                    else if (tokenResponse.expiresIn < DateTime.Now)
                    {
                        Assert.IsFalse(result.token.Equals(tokenResponse.token));

                        tokenResponse = result;
                    }

                    Assert.IsNotNull(result);
                });
            });
        }

        [TestMethod]
        public void TestParallelRequestsGetAllApiDataset()
        {
            Parallel.For(0, 1000, i =>
            {
                var result = apiClientService.GetAllApiDataset();

                Assert.IsTrue(result != null && result.Count() > 0);
            });
        }

        [TestMethod]
        public void TestParallelRequestsAtualizarApiDataset()
        {
            Parallel.For(0, 1000, i =>
            {
                var dataset = apiClientService.GetAllApiDataset()?.ToArray();

                Parallel.For(0, dataset.Length, i =>
                {
                    var data = dataset[i];

                    data.Date = DateTime.UtcNow;

                    apiClientService.AtualizarApiDataset(data);

                    var result = apiClientService.GetApiDatasetByKey(data.Key)?.FirstOrDefault();

                    Assert.IsNotNull(result);
                });
            });
        }

        [TestMethod]
        public void TestParallelRequestsCriarApiDataset()
        {
            Parallel.For(0, 1000, i =>
            {
                var data = apiClientService.GetRandomApiDataset(1)?.FirstOrDefault();

                apiClientService.CriarApiDataset(data);

                var result = apiClientService.GetApiDatasetByKey(data.Key)?.FirstOrDefault();

                Assert.IsNotNull(result);
            });
        }

        [TestMethod]
        public void TestParallelRequestsGerarApiDatasetAleatoria()
        {
            Parallel.For(0, 1000, i =>
            {
                var dataset = apiClientService.GetAllApiDataset()?.ToArray();

                var result = apiClientService.GenerateRandom(10);

                var _dataset = apiClientService.GetAllApiDataset()?.ToArray();

                Assert.IsNotNull(result);
                Assert.IsTrue(_dataset.Count() > dataset.Count());
            });
        }

        [TestMethod]
        public void TestRequestsGetApiDatasetByKey()
        {
            Parallel.For(0, 1000, i =>
            {
                var dataset = apiClientService.GetAllApiDataset()?.ToArray();

                Assert.IsTrue(dataset != null && dataset.Length > 0);
            });
        }
    }
}
