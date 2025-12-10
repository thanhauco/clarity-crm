using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Clarity.Core.Interfaces;
using Clarity.Core.Models;

namespace Clarity.Data.Repositories
{
    public class OpportunityRepository : IOpportunityRepository
    {
        private readonly ClarityDbContext _context;

        public OpportunityRepository(ClarityDbContext context)
        {
            _context = context;
        }

        public async Task<Opportunity> GetByIdAsync(int id)
        {
            return await _context.Opportunities
                .Include(o => o.Customer)
                .Include(o => o.AssignedUser)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Opportunity>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Opportunities
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Opportunity>> GetByStageAsync(OpportunityStage stage)
        {
            return await _context.Opportunities
                .Where(o => o.Stage == stage)
                .OrderByDescending(o => o.Amount)
                .ToListAsync();
        }

        public async Task<Opportunity> CreateAsync(Opportunity opportunity)
        {
            _context.Opportunities.Add(opportunity);
            await _context.SaveChangesAsync();
            return opportunity;
        }

        public async Task<Opportunity> UpdateAsync(Opportunity opportunity)
        {
            _context.Entry(opportunity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return opportunity;
        }

        public async Task<decimal> GetTotalPipelineValueAsync()
        {
            return await _context.Opportunities
                .Where(o => o.Stage != OpportunityStage.ClosedLost && o.Stage != OpportunityStage.ClosedWon)
                .SumAsync(o => o.Amount);
        }
    }
}
