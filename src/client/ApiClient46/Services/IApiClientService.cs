using ApiClient46.Models.Services;
using System.Collections;
using System.Collections.Generic;

namespace ApiClient.Services
{
    public interface IApiClientService
    {
        string AtualizarApiDataset(ApiDataset dataset);
        string CriarApiDataset(ApiDataset dataset);
        string GerarApiDatasetAleatoria(int total);
        IEnumerable<ApiDataset> GetAllApiDataset();
        IEnumerable<ApiDataset> GetApiDatasetByKey(string key);
        void Autenticar();
    }
}