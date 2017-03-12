using System.Collections.Generic;
using System.Threading.Tasks;
using Clarity.Core.Models;

namespace Clarity.Core.Interfaces
{
    public interface ICustomerService
    {
        Task<Customer> GetCustomerAsync(int id);
        Task<IEnumerable<Customer>> GetAllCustomersAsync();
        Task<IEnumerable<Customer>> SearchCustomersAsync(string query);
        Task<Customer> CreateCustomerAsync(Customer customer);
        Task<Customer> UpdateCustomerAsync(int id, Customer customer);
        Task<bool> DeleteCustomerAsync(int id);
        Task<CustomerStats> GetStatsAsync();
    }

    public interface ILeadService
    {
        Task<Lead> GetLeadAsync(int id);
        Task<IEnumerable<Lead>> GetAllLeadsAsync();
        Task<IEnumerable<Lead>> GetLeadsByStatusAsync(LeadStatus status);
        Task<Lead> CreateLeadAsync(Lead lead);
        Task<Lead> UpdateLeadAsync(int id, Lead lead);
        Task<Lead> UpdateStatusAsync(int id, LeadStatus status);
        Task<Customer> ConvertToCustomerAsync(int leadId);
        Task<LeadStats> GetStatsAsync();
    }

    public interface IActivityService
    {
        Task<Activity> GetActivityAsync(int id);
        Task<IEnumerable<Activity>> GetActivitiesByCustomerAsync(int customerId);
        Task<IEnumerable<Activity>> GetActivitiesByUserAsync(int userId);
        Task<IEnumerable<Activity>> GetPendingActivitiesAsync();
        Task<Activity> CreateActivityAsync(Activity activity);
        Task<Activity> UpdateActivityAsync(int id, Activity activity);
        Task<Activity> CompleteActivityAsync(int id);
        Task<bool> DeleteActivityAsync(int id);
    }

    public interface IAuthService
    {
        Task<AuthResult> LoginAsync(string username, string password);
        Task<AuthResult> RegisterAsync(User user, string password);
        Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword);
        Task<User> GetCurrentUserAsync(string token);
        Task<bool> ValidateTokenAsync(string token);
    }

    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string to, string subject, string body);
        Task<bool> SendTemplateEmailAsync(string to, string templateName, object data);
        Task<bool> SendBulkEmailAsync(IEnumerable<string> recipients, string subject, string body);
    }

    public interface IReportService
    {
        Task<DashboardData> GetDashboardDataAsync();
        Task<SalesReportData> GetSalesReportAsync(DateTime startDate, DateTime endDate);
        Task<PipelineReportData> GetPipelineReportAsync();
        Task<ActivityReportData> GetActivityReportAsync(int userId, DateTime startDate, DateTime endDate);
    }

    // DTOs
    public class CustomerStats
    {
        public int TotalCustomers { get; set; }
        public int ActiveCustomers { get; set; }
        public int NewThisMonth { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class LeadStats
    {
        public int TotalLeads { get; set; }
        public int NewLeads { get; set; }
        public int QualifiedLeads { get; set; }
        public int ConvertedLeads { get; set; }
        public decimal ConversionRate { get; set; }
    }

    public class AuthResult
    {
        public bool Success { get; set; }
        public string Token { get; set; }
        public User User { get; set; }
        public string Error { get; set; }
    }

    public class DashboardData
    {
        public int TotalCustomers { get; set; }
        public int TotalLeads { get; set; }
        public int OpenOpportunities { get; set; }
        public decimal PipelineValue { get; set; }
        public int PendingActivities { get; set; }
        public decimal RevenueThisMonth { get; set; }
        public List<ChartDataPoint> SalesTrend { get; set; }
        public List<ChartDataPoint> LeadsBySource { get; set; }
    }

    public class ChartDataPoint
    {
        public string Label { get; set; }
        public decimal Value { get; set; }
    }

    public class SalesReportData
    {
        public decimal TotalRevenue { get; set; }
        public int DealsWon { get; set; }
        public int DealsLost { get; set; }
        public decimal WinRate { get; set; }
        public decimal AverageDealSize { get; set; }
        public List<SalesRepPerformance> TopPerformers { get; set; }
    }

    public class SalesRepPerformance
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public decimal Revenue { get; set; }
        public int Deals { get; set; }
    }

    public class PipelineReportData
    {
        public decimal TotalValue { get; set; }
        public List<StageData> ByStage { get; set; }
    }

    public class StageData
    {
        public string Stage { get; set; }
        public int Count { get; set; }
        public decimal Value { get; set; }
    }

    public class ActivityReportData
    {
        public int TotalActivities { get; set; }
        public int Completed { get; set; }
        public int Pending { get; set; }
        public List<ChartDataPoint> ByType { get; set; }
    }
}
