using ApiServer.Models;
using ApiServer.Repository;
using ApiServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ApiDatasetRandomController(ApiDbContext apiDbContext, RandomUserService randomUser) : Controller
    {
        private readonly ApiDbContext _context = apiDbContext;
        private readonly RandomUserService _randomUser = randomUser;

        [HttpPost("GenerateRandom")]
        public IActionResult GenerateRandom([FromBody] int total = 100)
        {
            foreach (var item in GetRandomApiDataset(total))
            {
                _context.ApiDataset.Add(item);
            }

            _context.SaveChanges();

            return Ok();
        }

        [HttpGet("GetRandomApiDataset")]
        public IEnumerable<ApiDataset> GetRandomApiDataset([FromQuery] int total = 100)
        {
            ApiDataset[] apiDatasets = new ApiDataset[total];

            for (int i = 0; i < total; i++)
            {
                apiDatasets[i] = new ApiDataset
                {
                    Key = GetRandomKey(),
                    Date = GetRandomDate(),
                    Group = GetRandomGroup(),
                    Value = GetRandomString(),
                };
            }

            return apiDatasets;
        }

        [HttpGet("GetRandomDate")]
        public DateTime GetRandomDate()
        {
            return DateTime.UtcNow.AddMinutes(Random.Shared.Next(-1000, 1000));
        }

        [HttpGet("GetRandomKey")]
        public string GetRandomKey()
        {
            return Guid.NewGuid().ToString();
        }

        [HttpGet("GetRandomGroup")]
        public string? GetRandomGroup()
        {
            return Random.Shared.Next(int.MinValue, int.MaxValue).ToString();
        }

        [HttpGet("GetRandomString")]
        public string? GetRandomString()
        {
            try
            {
                return _randomUser.GetRandomUserAsync().Result;
            }
            catch
            {
                return null;
            }
        }
    }
}
