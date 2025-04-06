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

        [HttpPost]
        public IActionResult Post([FromBody] int range = 100)
        {
            for (int i = 0; i < range; i++)
            {
                _context.ApiDataset.Add(new ApiDataset
                {
                    Key = Guid.NewGuid().ToString(),
                    Date = DateTime.Now,
                    Group = Random.Shared.Next(int.MinValue, int.MaxValue).ToString(),
                    Value = GetRandomString() ?? "",
                });
            }

            _context.SaveChanges();

            return Ok();
        }

        private string? GetRandomString()
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
