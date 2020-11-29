using RedisDemo.SharedLibrary.Data;
using System.Threading.Tasks;
using System.Web.Http;

namespace RedisDemo.WebFramework.Controllers
{

    [RoutePrefix("api/stats")]
    public class StatsController : ApiController
    {
        private readonly IDashboardDataService _dataService;

        public StatsController(IDashboardDataService dataService)
        {
            _dataService = dataService;
        }

        // GET api/values
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> Get()
        {
            var result = await _dataService.GetStatsAsync();
            return Ok(result);
        }
    }
}
