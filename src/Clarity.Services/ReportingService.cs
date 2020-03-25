using System;
using System.Linq;
using System.Collections.Generic;
using Clarity.Core.Models;
using Clarity.Core.Interfaces;
using System.Text;

namespace Clarity.Services {
    public class ReportingService {
        private readonly ILeadRepository _leadRepo;
        private readonly IOrderRepository _orderRepo;
        
        // Constructor Injection
        public ReportingService(ILeadRepository leadRepo, IOrderRepository orderRepo) {
            _leadRepo = leadRepo;
            _orderRepo = orderRepo;
        }

        public DashboardStats GetStats() {
            // Real logic: Aggregating data from repositories
            // In a real DB scenario, these would be async calls
            var allLeads = _leadRepo.GetAllAsync().Result; 
            var allOrders = _orderRepo.GetAllAsync().Result;

            return new DashboardStats { 
                TotalLeads = allLeads.Count(), 
                MonthlyRevenue = allOrders
                    .Where(o => o.CreatedAt >= DateTime.UtcNow.AddDays(-30))
                    .Sum(o => o.TotalAmount) 
            };
        }

        public byte[] GeneratePdfReport(ReportRequest request) {
            // Generate a real CSV/Text report instead of empty byte array
            var sb = new StringBuilder();
            sb.AppendLine("CLARITY CRM REPORT");
            sb.AppendLine($"Generated: {DateTime.Now}");
            sb.AppendLine("-------------------------");
            sb.AppendLine($"Report Type: {request.Type}");
            sb.AppendLine($"Date Range: {request.StartDate} - {request.EndDate}");
            
            // Logic to fetch data based on types
            if (request.Type == "Sales") {
                var orders = _orderRepo.GetAllAsync().Result;
                foreach(var order in orders) {
                    sb.AppendLine($"Order #{order.Id}: {order.TotalAmount:C} - {order.Status}");
                }
            }
            else if (request.Type == "Leads") {
                 var leads = _leadRepo.GetAllAsync().Result;
                 foreach(var lead in leads) {
                     sb.AppendLine($"Lead: {lead.FirstName} {lead.LastName} ({lead.Company}) - {lead.Status}");
                 }
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }
    }
}
