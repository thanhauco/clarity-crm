using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clarity.Core.Interfaces;
using Clarity.Core.Models;

namespace Clarity.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IAuditLogService _auditLogService;

        public CustomerService(ICustomerRepository customerRepository, IAuditLogService auditLogService)
        {
            _customerRepository = customerRepository;
            _auditLogService = auditLogService;
        }

        public async Task<Customer> GetCustomerAsync(int id)
        {
            return await _customerRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
        {
            return await _customerRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Customer>> SearchCustomersAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return await GetAllCustomersAsync();

            return await _customerRepository.SearchAsync(query);
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            ValidateCustomer(customer);
            return await _customerRepository.CreateAsync(customer);
        }

        public async Task<Customer> UpdateCustomerAsync(int id, Customer customer)
        {
            var existing = await _customerRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Customer with ID {id} not found");

            existing.FirstName = customer.FirstName;
            existing.LastName = customer.LastName;
            existing.Email = customer.Email;
            existing.Phone = customer.Phone;
            existing.Company = customer.Company;
            existing.Title = customer.Title;
            existing.Address = customer.Address;
            existing.City = customer.City;
            existing.State = customer.State;
            existing.ZipCode = customer.ZipCode;
            existing.Country = customer.Country;
            existing.Status = customer.Status;
            existing.Type = customer.Type;
            existing.Notes = customer.Notes;
            existing.AssignedUserId = customer.AssignedUserId;

            var updated = await _customerRepository.UpdateAsync(existing);
            
            await _auditLogService.LogAsync(
                "Customer", 
                updated.Id, 
                "Update", 
                customer.AssignedUserId, // Assuming the assigned user is the one making changes, or ideally we'd pass the current user ID context 
                $"Updated details for {updated.FirstName} {updated.LastName}"
            );

            return updated;
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            return await _customerRepository.DeleteAsync(id);
        }

        public async Task<CustomerStats> GetStatsAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            var customerList = customers.ToList();

            return new CustomerStats
            {
                TotalCustomers = customerList.Count,
                ActiveCustomers = customerList.Count(c => c.Status == CustomerStatus.Active),
                NewThisMonth = customerList.Count(c => c.CreatedAt >= DateTime.UtcNow.AddDays(-30)),
                TotalRevenue = customerList.Where(c => c.AnnualRevenue.HasValue).Sum(c => c.AnnualRevenue.Value)
            };
        }

        private void ValidateCustomer(Customer customer)
        {
            if (string.IsNullOrWhiteSpace(customer.FirstName))
                throw new ArgumentException("First name is required");
            if (string.IsNullOrWhiteSpace(customer.LastName))
                throw new ArgumentException("Last name is required");
            if (string.IsNullOrWhiteSpace(customer.Email))
                throw new ArgumentException("Email is required");
        }
    }

    public class LeadService : ILeadService
    {
        private readonly ILeadRepository _leadRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IAuditLogService _auditLogService;

        public LeadService(ILeadRepository leadRepository, ICustomerRepository customerRepository, IAuditLogService auditLogService)
        {
            _leadRepository = leadRepository;
            _customerRepository = customerRepository;
            _auditLogService = auditLogService;
        }

        public async Task<Lead> GetLeadAsync(int id)
        {
            return await _leadRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Lead>> GetAllLeadsAsync()
        {
            return await _leadRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Lead>> GetLeadsByStatusAsync(LeadStatus status)
        {
            return await _leadRepository.GetByStatusAsync(status);
        }

        public async Task<Lead> CreateLeadAsync(Lead lead)
        {
            lead.Status = LeadStatus.New;
            return await _leadRepository.CreateAsync(lead);
        }

        public async Task<Lead> UpdateLeadAsync(int id, Lead lead)
        {
            var existing = await _leadRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Lead with ID {id} not found");

            existing.FirstName = lead.FirstName;
            existing.LastName = lead.LastName;
            existing.Email = lead.Email;
            existing.Phone = lead.Phone;
            existing.Company = lead.Company;
            existing.Title = lead.Title;
            existing.Source = lead.Source;
            existing.Rating = lead.Rating;
            existing.EstimatedValue = lead.EstimatedValue;
            existing.Description = lead.Description;
            existing.AssignedUserId = lead.AssignedUserId;

            return await _leadRepository.UpdateAsync(existing);
        }

        public async Task<Lead> UpdateStatusAsync(int id, LeadStatus status)
        {
            var lead = await _leadRepository.GetByIdAsync(id);
            if (lead == null)
                throw new KeyNotFoundException($"Lead with ID {id} not found");

            lead.Status = status;
            if (status == LeadStatus.Won || status == LeadStatus.Lost)
                lead.ClosedAt = DateTime.UtcNow;

            return await _leadRepository.UpdateAsync(lead);
        }

        public async Task<Customer> ConvertToCustomerAsync(int leadId)
        {
            var lead = await _leadRepository.GetByIdAsync(leadId);
            if (lead == null)
                throw new KeyNotFoundException($"Lead with ID {leadId} not found");

            var customer = new Customer
            {
                FirstName = lead.FirstName,
                LastName = lead.LastName,
                Email = lead.Email,
                Phone = lead.Phone,
                Company = lead.Company,
                Title = lead.Title,
                Status = CustomerStatus.Active,
                Type = CustomerType.Business,
                AssignedUserId = lead.AssignedUserId
            };

            var createdCustomer = await _customerRepository.CreateAsync(customer);

            lead.Status = LeadStatus.Won;
            lead.ConvertedAt = DateTime.UtcNow;
            lead.CustomerId = createdCustomer.Id;
            await _leadRepository.UpdateAsync(lead);

            return createdCustomer;
        }

        public async Task<LeadStats> GetStatsAsync()
        {
            var total = await _leadRepository.GetCountAsync();
            var newLeads = await _leadRepository.GetCountByStatusAsync(LeadStatus.New);
            var qualified = await _leadRepository.GetCountByStatusAsync(LeadStatus.Qualified);
            var won = await _leadRepository.GetCountByStatusAsync(LeadStatus.Won);

            return new LeadStats
            {
                TotalLeads = total,
                NewLeads = newLeads,
                QualifiedLeads = qualified,
                ConvertedLeads = won,
                ConversionRate = total > 0 ? (decimal)won / total * 100 : 0
            };
        }
    }

    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepository;

        public ActivityService(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        public async Task<Activity> GetActivityAsync(int id)
        {
            return await _activityRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Activity>> GetActivitiesByCustomerAsync(int customerId)
        {
            return await _activityRepository.GetByCustomerIdAsync(customerId);
        }

        public async Task<IEnumerable<Activity>> GetActivitiesByUserAsync(int userId)
        {
            return await _activityRepository.GetByUserIdAsync(userId);
        }

        public async Task<IEnumerable<Activity>> GetPendingActivitiesAsync()
        {
            return await _activityRepository.GetPendingAsync();
        }

        public async Task<Activity> CreateActivityAsync(Activity activity)
        {
            activity.Status = ActivityStatus.Pending;
            return await _activityRepository.CreateAsync(activity);
        }

        public async Task<Activity> UpdateActivityAsync(int id, Activity activity)
        {
            var existing = await _activityRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Activity with ID {id} not found");

            existing.Type = activity.Type;
            existing.Subject = activity.Subject;
            existing.Description = activity.Description;
            existing.Priority = activity.Priority;
            existing.DueDate = activity.DueDate;

            return await _activityRepository.UpdateAsync(existing);
        }

        public async Task<Activity> CompleteActivityAsync(int id)
        {
            var activity = await _activityRepository.GetByIdAsync(id);
            if (activity == null)
                throw new KeyNotFoundException($"Activity with ID {id} not found");

            activity.Status = ActivityStatus.Completed;
            activity.CompletedAt = DateTime.UtcNow;

            return await _activityRepository.UpdateAsync(activity);
        }

        public async Task<bool> DeleteActivityAsync(int id)
        {
            return await _activityRepository.DeleteAsync(id);
        }
    }
}
