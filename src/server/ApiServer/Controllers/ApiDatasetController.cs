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

            return CreatedAtAction(nameof(Post), new { guid = _memoryDb.Entity.Guid }, apiDataset);
        }
    }
}
