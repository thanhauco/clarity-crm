using System.Collections.Generic;
using System.Threading.Tasks;
using Clarity.Core.Models;

namespace Clarity.Core.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer> GetByIdAsync(int id);
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<IEnumerable<Customer>> GetByStatusAsync(CustomerStatus status);
        Task<IEnumerable<Customer>> SearchAsync(string query);
        Task<Customer> CreateAsync(Customer customer);
        Task<Customer> UpdateAsync(Customer customer);
        Task<bool> DeleteAsync(int id);
        Task<int> GetCountAsync();
        Task<IEnumerable<Customer>> GetByAssignedUserAsync(int userId);
    }

    public interface ILeadRepository
    {
        Task<Lead> GetByIdAsync(int id);
        Task<IEnumerable<Lead>> GetAllAsync();
        Task<IEnumerable<Lead>> GetByStatusAsync(LeadStatus status);
        Task<IEnumerable<Lead>> GetBySourceAsync(LeadSource source);
        Task<Lead> CreateAsync(Lead lead);
        Task<Lead> UpdateAsync(Lead lead);
        Task<bool> DeleteAsync(int id);
        Task<int> GetCountAsync();
        Task<int> GetCountByStatusAsync(LeadStatus status);
    }

    public interface IActivityRepository
    {
        Task<Activity> GetByIdAsync(int id);
        Task<IEnumerable<Activity>> GetByCustomerIdAsync(int customerId);
        Task<IEnumerable<Activity>> GetByLeadIdAsync(int leadId);
        Task<IEnumerable<Activity>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Activity>> GetPendingAsync();
        Task<Activity> CreateAsync(Activity activity);
        Task<Activity> UpdateAsync(Activity activity);
        Task<bool> DeleteAsync(int id);
    }

    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByUsernameAsync(string username);
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
    }

    public interface IOpportunityRepository
    {
        Task<Opportunity> GetByIdAsync(int id);
        Task<IEnumerable<Opportunity>> GetByCustomerIdAsync(int customerId);
        Task<IEnumerable<Opportunity>> GetByStageAsync(OpportunityStage stage);
        Task<Opportunity> CreateAsync(Opportunity opportunity);
        Task<Opportunity> UpdateAsync(Opportunity opportunity);
        Task<decimal> GetTotalPipelineValueAsync();
    }
}
