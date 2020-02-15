using Microsoft.AspNetCore.Mvc;
using Clarity.Services;
namespace Clarity.Web.Controllers {
    [Route(""api/[controller]"")]
    [ApiController]
    public class DashboardController : ControllerBase {
        private readonly ReportingService _service;
        public DashboardController(ReportingService service) { _service = service; }
        
        [HttpGet(""stats"")]
        public IActionResult GetStats() {
            return Ok(_service.GetStats());
        }
    }
}
