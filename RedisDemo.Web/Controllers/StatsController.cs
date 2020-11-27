using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using RedisDemo.Web.Data;

namespace RedisDemo.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private readonly IDashboardDataService _service;

        public StatsController(IDashboardDataService dataService)
        {
            _service = dataService;
        }

        // GET api/stats
        [HttpGet]
        public async Task<ActionResult<SummaryToday>> GetAsync()
        {
            var result = await _service.GetStatsAsync();

            return Ok(result);
        }
    }
}
