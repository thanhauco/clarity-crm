using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Clarity.Core.Interfaces;
using Clarity.Core.Models;

namespace Clarity.Data.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ClarityDbContext _context;

        public CustomerRepository(ClarityDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _context.Customers
                .Include(c => c.AssignedUser)
                .Include(c => c.Activities)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _context.Customers
                .Include(c => c.AssignedUser)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetByStatusAsync(CustomerStatus status)
        {
            return await _context.Customers
                .Where(c => c.Status == status)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Customer>> SearchAsync(string query)
        {
            var lowerQuery = query.ToLower();
            return await _context.Customers
                .Where(c => c.FirstName.ToLower().Contains(lowerQuery) ||
                           c.LastName.ToLower().Contains(lowerQuery) ||
                           c.Email.ToLower().Contains(lowerQuery) ||
                           c.Company.ToLower().Contains(lowerQuery))
                .OrderByDescending(c => c.CreatedAt)
                .Take(50)
                .ToListAsync();
        }

        public async Task<Customer> CreateAsync(Customer customer)
        {
            customer.CreatedAt = DateTime.UtcNow;
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            customer.UpdatedAt = DateTime.UtcNow;
            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return false;

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Customers.CountAsync();
        }

        public async Task<IEnumerable<Customer>> GetByAssignedUserAsync(int userId)
        {
            return await _context.Customers
                .Where(c => c.AssignedUserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }
    }

    public class LeadRepository : ILeadRepository
    {
        private readonly ClarityDbContext _context;

        public LeadRepository(ClarityDbContext context)
        {
            _context = context;
        }

        public async Task<Lead> GetByIdAsync(int id)
        {
            return await _context.Leads
                .Include(l => l.AssignedUser)
                .Include(l => l.Customer)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<IEnumerable<Lead>> GetAllAsync()
        {
            return await _context.Leads
                .Include(l => l.AssignedUser)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Lead>> GetByStatusAsync(LeadStatus status)
        {
            return await _context.Leads
                .Where(l => l.Status == status)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Lead>> GetBySourceAsync(LeadSource source)
        {
            return await _context.Leads
                .Where(l => l.Source == source)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync();
        }

        public async Task<Lead> CreateAsync(Lead lead)
        {
            lead.CreatedAt = DateTime.UtcNow;
            _context.Leads.Add(lead);
            await _context.SaveChangesAsync();
            return lead;
        }

        public async Task<Lead> UpdateAsync(Lead lead)
        {
            lead.UpdatedAt = DateTime.UtcNow;
            _context.Entry(lead).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return lead;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var lead = await _context.Leads.FindAsync(id);
            if (lead == null) return false;

            _context.Leads.Remove(lead);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Leads.CountAsync();
        }

        public async Task<int> GetCountByStatusAsync(LeadStatus status)
        {
            return await _context.Leads.CountAsync(l => l.Status == status);
        }
    }

    public class ActivityRepository : IActivityRepository
    {
        private readonly ClarityDbContext _context;

        public ActivityRepository(ClarityDbContext context)
        {
            _context = context;
        }

        public async Task<Activity> GetByIdAsync(int id)
        {
            return await _context.Activities
                .Include(a => a.Customer)
                .Include(a => a.AssignedUser)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Activity>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Activities
                .Where(a => a.CustomerId == customerId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Activity>> GetByLeadIdAsync(int leadId)
        {
            return await _context.Activities
                .Where(a => a.LeadId == leadId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Activity>> GetByUserIdAsync(int userId)
        {
            return await _context.Activities
                .Where(a => a.AssignedUserId == userId)
                .OrderByDescending(a => a.DueDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<Activity>> GetPendingAsync()
        {
            return await _context.Activities
                .Where(a => a.Status == ActivityStatus.Pending || a.Status == ActivityStatus.InProgress)
                .OrderBy(a => a.DueDate)
                .ToListAsync();
        }

        public async Task<Activity> CreateAsync(Activity activity)
        {
            activity.CreatedAt = DateTime.UtcNow;
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
            return activity;
        }

        public async Task<Activity> UpdateAsync(Activity activity)
        {
            _context.Entry(activity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return activity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null) return false;

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            return true;
        }
    }

    public class UserRepository : IUserRepository
    {
        private readonly ClarityDbContext _context;

        public UserRepository(ClarityDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username.ToLower() == username.ToLower());
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users
                .Where(u => u.IsActive)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToListAsync();
        }

        public async Task<User> CreateAsync(User user)
        {
            user.CreatedAt = DateTime.UtcNow;
            user.IsActive = true;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            user.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
