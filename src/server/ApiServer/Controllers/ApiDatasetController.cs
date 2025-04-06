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
        public IEnumerable<ApiDataset> Get()
        {
            return [.. _context.ApiDataset];
        }

        [HttpPost]
        public IActionResult Post([FromBody] ApiDataset apiDataset)
        {
            var _memoryDb = _context.ApiDataset.Add(apiDataset);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Post), new { key = _memoryDb.Entity.Key }, apiDataset);
        }

        [HttpPut]
        public IActionResult Put([FromBody] ApiDataset apiDataset)
        {
            if (apiDataset == null || apiDataset?.Key == null)
            {
                return BadRequest("[Key] não pode ser nulo");
            }

            PutKey(apiDataset);

            return NoContent();
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
            existingDataset.Date = apiDataset.Date;
            existingDataset.Group = apiDataset.Group;
            existingDataset.Key = apiDataset.Key;
            existingDataset.Value = apiDataset.Value;

            _context.SaveChanges();
        }
    }
}
