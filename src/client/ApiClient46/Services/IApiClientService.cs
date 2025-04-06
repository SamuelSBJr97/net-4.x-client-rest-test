using ApiClient46.Models.Services;

namespace ApiClient.Services
{
    public interface IApiClientService
    {
        string AtualizarApiDataset(ApiDataset dataset);
        string CriarApiDataset(ApiDataset dataset);
        string GerarApiDatasetAleatoria(int total);
        string GetAllApiDataset();
        string GetApiDatasetByKey(string key);
    }
}