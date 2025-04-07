using ApiServer.Models;
using ApiServer.Repository;
using ApiServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ApiServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ApiDatasetController(ApiDbContext apiDbContext) : ControllerBase
    {
        private readonly ApiDbContext _context = apiDbContext;

        [HttpGet]
        public IEnumerable<ApiDataset> Get([FromQuery] string? key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return [.. _context.ApiDataset];
            }

            return [.. _context.ApiDataset?.Where(_ => _.Key.Equals(key))];
        }

        [HttpPost]
        public IActionResult Post([FromBody] ApiDataset apiDataset)
        {
            var _memoryDb = _context.ApiDataset.Add(apiDataset);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPut]
        public IActionResult Put([FromBody] ApiDataset apiDataset)
        {
            if (apiDataset == null || apiDataset?.Key == null)
            {
                return BadRequest("[Key] não pode ser nulo");
            }

            PutKey(apiDataset);

            return Ok();
        }

        private void PutKey(ApiDataset apiDataset)
        {
            var existingDataset = _context.ApiDataset.Where(x => x.Key == apiDataset.Key);

            foreach (var dataset in existingDataset)
            {
                ChangeDataset(apiDataset, dataset);
            }
        }

        private void ChangeDataset(ApiDataset apiDataset, ApiDataset existingDataset)
        {
            // Update the existing dataset with the new values
            existingDataset.Date = apiDataset.Date ?? existingDataset.Date;
            existingDataset.Group = apiDataset.Group ?? existingDataset.Group;
            existingDataset.Key = apiDataset.Key ?? existingDataset.Key;
            existingDataset.Value = apiDataset.Value ?? existingDataset.Value;

            _context.SaveChanges();
        }
    }
}
