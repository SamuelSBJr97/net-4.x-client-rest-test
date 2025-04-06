using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ApiClient.Services;
using Newtonsoft.Json;
using ApiClient46.Models.Services;

namespace ApiClient46.Test
{
    [TestClass]
    public sealed class ApiClientServiceTest
    {
        private readonly IApiClientService apiClientService;

        public ApiClientServiceTest()
        {
            apiClientService = new ApiRestsharpClientService("https://localhost", "usuario", "senha");
        }

        [TestMethod]
        public void TestParallelRequestsGetAllApiDataset()
        {
            Parallel.For(0, 100, i =>
            {
                try
                {
                    // Medir o tempo de execução
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    var result = apiClientService.GetAllApiDataset();
                    stopwatch.Stop();

                    Assert.IsNotNull(result);
                    Console.WriteLine($"Request {i} completed in {stopwatch.ElapsedMilliseconds} ms");

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
                    // Medir o tempo de execução
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    var result = apiClientService.GetAllApiDataset();
                    stopwatch.Stop();

                    Assert.IsNotNull(result);
                    Console.WriteLine($"Request {i} completed in {stopwatch.ElapsedMilliseconds} ms");

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
                    // Medir o tempo de execução
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    var result = apiClientService.GetAllApiDataset();
                    stopwatch.Stop();

                    Assert.IsNotNull(result);
                    Console.WriteLine($"Request {i} completed in {stopwatch.ElapsedMilliseconds} ms");

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
                    // Medir o tempo de execução
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    var result = apiClientService.GerarApiDatasetAleatoria(10);
                    stopwatch.Stop();

                    Assert.IsNotNull(result);
                    Console.WriteLine($"Request {i} completed in {stopwatch.ElapsedMilliseconds} ms");

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

            result = apiClientService.GetAllApiDataset();

            Assert.IsNotNull(result);

            var dataset = JsonConvert.DeserializeObject<IEnumerable<ApiDataset>>(result)?.ToArray();

            Assert.IsTrue(dataset != null && dataset.Length > 0);

            Parallel.For(0, dataset.Length, i =>
            {
                try
                {
                    var data = dataset[i];

                    data.Date = DateTime.UtcNow;

                    var result = apiClientService.AtualizarApiDataset(data);

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

                    var result = JsonConvert.DeserializeObject<ApiDataset>(apiClientService.GetApiDatasetByKey(data.Key));

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
