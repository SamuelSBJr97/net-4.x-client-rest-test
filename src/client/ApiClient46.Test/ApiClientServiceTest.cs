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

            apiClientService = new ApiRestsharpClientService(apiBaseUrl, apiUser, apiPassword);
        }

        [TestMethod]
        public void TestParallelRequestsAutenticar()
        {
            var tokenResponse = apiClientService.Autenticar();

            Parallel.For(0, 10, i =>
            {
                try
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

                    Assert.IsTrue(result.expiresIn > DateTime.Now);
                }
                catch (Exception ex)
                {
                    Assert.Fail($"Request {i} failed: {ex.Message}");
                }
            });
        }

        [TestMethod]
        public void TestParallelRequestsGetAllApiDataset()
        {
            Parallel.For(0, 100, i =>
            {
                try
                {
                    var result = apiClientService.GetAllApiDataset();

                    Assert.IsTrue(result != null && result.Count() > 0);
                }
                catch (Exception ex)
                {
                    Assert.Fail($"Request {i} failed: {ex.Message}");
                }
            });
        }

        [TestMethod]
        public void TestParallelRequestsAtualizarApiDataset()
        {
            Parallel.For(0, 100, i =>
            {
                try
                {
                    var result = apiClientService.GetAllApiDataset();

                    Assert.IsNotNull(result);

                }
                catch (Exception ex)
                {
                    Assert.Fail($"Request {i} failed: {ex.Message}");
                }
            });
        }

        [TestMethod]
        public void TestParallelRequestsCriarApiDataset()
        {
            Parallel.For(0, 100, i =>
            {
                try
                {
                    var result = apiClientService.GetAllApiDataset();

                    Assert.IsNotNull(result);

                }
                catch (Exception ex)
                {
                    Assert.Fail($"Request {i} failed: {ex.Message}");
                }
            });
        }

        [TestMethod]
        public void TestParallelRequestsGerarApiDatasetAleatoria()
        {
            Parallel.For(0, 100, i =>
            {
                try
                {
                    var result = apiClientService.GerarApiDatasetAleatoria(10);

                    Assert.IsNotNull(result);

                }
                catch (Exception ex)
                {
                    Assert.Fail($"Request {i} failed: {ex.Message}");
                }
            });
        }

        [TestMethod]
        public void TestParallelRequestsGetApiDatasetByKey()
        {
            var result = apiClientService.GerarApiDatasetAleatoria(10);

            var dataset = apiClientService.GetAllApiDataset()?.ToArray();

            Assert.IsTrue(dataset != null && dataset.Length > 0);

            Parallel.For(0, dataset.Length, i =>
            {
                try
                {
                    var data = dataset[i];

                    data.Date = DateTime.UtcNow;

                    apiClientService.AtualizarApiDataset(data);

                    var result = apiClientService.GetApiDatasetByKey(data.Key)?.FirstOrDefault();

                    Assert.IsNotNull(result);

                    Assert.Equals(data, result);
                }
                catch (Exception ex)
                {
                    Assert.Fail($"Request {i} failed: {ex.Message}");
                }
            });

            Parallel.For(0, dataset.Length, i =>
            {
                try
                {
                    var data = dataset[i];

                    var result = apiClientService.GetApiDatasetByKey(data.Key);

                    Assert.IsNotNull(result);

                    Assert.Equals(data, result);

                }
                catch (Exception ex)
                {
                    Assert.Fail($"Request {i} failed: {ex.Message}");
                }
            });
        }
    }
}
