using System;
using System.Threading.Tasks;

namespace MediaTracker.API.Services.Interfaces
{
    public interface IReportService
    {
        Task<object> GenerateReportAsync(string reportType, DateTime dateFrom, DateTime dateTo);
        Task<byte[]> GenerateReportPdfAsync(string reportType, DateTime dateFrom, DateTime dateTo, object reportData);
    }
}