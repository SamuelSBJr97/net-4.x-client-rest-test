using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ApiClient.Services;

namespace ApiClient46.Test
{
    [TestClass]
    public sealed class ApiClientServiceTest
    {
        private const int NumberOfRequests = 1000;
        private const string Host = "localhost";
        private const int Port = 7216;
        private readonly IApiClientService apiClientService;

        public ApiClientServiceTest()
        {
            apiClientService = new ApiRestsharpClientService("", "", "");
        }

        [TestMethod]
        public void TestParallelRequestsGetAll()
        {
            Parallel.For(0, NumberOfRequests, i =>
            {
                try
                {
                    // Medir o tempo de execução
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    var result = ApiRestsharpClientService.API_REST_CLIENT_SERVICE.GetAllApiDataset();
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

        private bool IsPortOpen(string host, int port)
        {
            try
            {
                using (var client = new TcpClient(host, port))
                {
                    return true;
                }
            }
            catch (SocketException)
            {
                return false;
            }
        }
    }
}
