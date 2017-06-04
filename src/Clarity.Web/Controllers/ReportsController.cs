using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Clarity.Core.Interfaces;

namespace Clarity.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("dashboard")]
        public async Task<ActionResult<DashboardData>> GetDashboard()
        {
            var data = await _reportService.GetDashboardDataAsync();
            return Ok(data);
        }

        [HttpGet("sales")]
        public async Task<ActionResult<SalesReportData>> GetSalesReport(
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var start = startDate ?? DateTime.UtcNow.AddMonths(-1);
            var end = endDate ?? DateTime.UtcNow;

            var data = await _reportService.GetSalesReportAsync(start, end);
            return Ok(data);
        }

        [HttpGet("pipeline")]
        public async Task<ActionResult<PipelineReportData>> GetPipelineReport()
        {
            var data = await _reportService.GetPipelineReportAsync();
            return Ok(data);
        }

        [HttpGet("activities/{userId}")]
        public async Task<ActionResult<ActivityReportData>> GetActivityReport(
            int userId,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null)
        {
            var start = startDate ?? DateTime.UtcNow.AddMonths(-1);
            var end = endDate ?? DateTime.UtcNow;

            var data = await _reportService.GetActivityReportAsync(userId, start, end);
            return Ok(data);
        }
    }

    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public ActionResult<object> Get()
        {
            return Ok(new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Version = "1.0.0"
            });
        }
    }
}
