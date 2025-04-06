namespace ApiServer.Services
{
    public class RandomUserService
    {
        private readonly HttpClient _httpClient;

        public RandomUserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetRandomUserAsync()
        {
            var response = await _httpClient.GetAsync("https://randomuser.me/api/");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
