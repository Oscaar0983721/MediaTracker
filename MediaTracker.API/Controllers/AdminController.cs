using System;
using System.Threading.Tasks;
using MediaTracker.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MediaTracker.API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    public class AdminController : ControllerBase
    {
        private readonly IReportService _reportService;

        public AdminController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost("reports")]
        public async Task<ActionResult> GenerateReport([FromBody] ReportRequest request)
        {
            try
            {
                var report = await _reportService.GenerateReportAsync(
                    request.ReportType,
                    DateTime.Parse(request.DateFrom),
                    DateTime.Parse(request.DateTo)
                );

                return Ok(report);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPost("reports/download")]
        public async Task<ActionResult> DownloadReport([FromBody] ReportRequest request)
        {
            try
            {
                var pdfBytes = await _reportService.GenerateReportPdfAsync(
                    request.ReportType,
                    DateTime.Parse(request.DateFrom),
                    DateTime.Parse(request.DateTo),
                    request.ReportData
                );

                return File(
                    pdfBytes,
                    "application/pdf",
                    $"{request.ReportType}-report-{DateTime.Now:yyyy-MM-dd}.pdf"
                );
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }

    public class ReportRequest
    {
        public string ReportType { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public object ReportData { get; set; }
    }
}