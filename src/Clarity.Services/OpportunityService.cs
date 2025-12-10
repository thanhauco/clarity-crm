using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clarity.Core.Interfaces;
using Clarity.Core.Models;

namespace Clarity.Services
{
    public interface IOpportunityService
    {
        Task<IEnumerable<Opportunity>> GetAllAsync();
        Task<Opportunity> GetByIdAsync(int id);
        Task<Opportunity> CreateAsync(Opportunity opportunity);
        Task<Opportunity> UpdateAsync(int id, Opportunity opportunity);
        Task<bool> DeleteAsync(int id);
        Task<Opportunity> UpdateStageAsync(int id, OpportunityStage stage);
    }

    public class OpportunityService : IOpportunityService
    {
        private readonly IOpportunityRepository _repo;

        public OpportunityService(IOpportunityRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<Opportunity>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        public async Task<Opportunity> GetByIdAsync(int id)
        {
            return await _repo.GetByIdAsync(id);
        }

        public async Task<Opportunity> CreateAsync(Opportunity opportunity)
        {
            // Initial probability based on stage
            opportunity.Probability = CalculateProbability(opportunity.Stage);
            opportunity.CreatedAt = DateTime.UtcNow;
            
            return await _repo.CreateAsync(opportunity);
        }

        public async Task<Opportunity> UpdateAsync(int id, Opportunity opportunity)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) throw new KeyNotFoundException($"Opportunity {id} not found");

            existing.Name = opportunity.Name;
            existing.Description = opportunity.Description;
            existing.Amount = opportunity.Amount;
            existing.CloseDate = opportunity.CloseDate;
            existing.AssignedUserId = opportunity.AssignedUserId;
            existing.UpdatedAt = DateTime.UtcNow;

            return await _repo.UpdateAsync(existing);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repo.DeleteAsync(id);
        }

        public async Task<Opportunity> UpdateStageAsync(int id, OpportunityStage stage)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null) throw new KeyNotFoundException($"Opportunity {id} not found");

            existing.Stage = stage;
            existing.Probability = CalculateProbability(stage);
            existing.UpdatedAt = DateTime.UtcNow;

            if (stage == OpportunityStage.ClosedWon)
            {
                existing.WonAt = DateTime.UtcNow;
            }
            else if (stage == OpportunityStage.ClosedLost)
            {
                existing.LostAt = DateTime.UtcNow;
            }

            return await _repo.UpdateAsync(existing);
        }

        private int CalculateProbability(OpportunityStage stage)
        {
            switch (stage)
            {
                case OpportunityStage.Prospecting: return 10;
                case OpportunityStage.Qualification: return 20;
                case OpportunityStage.NeedsAnalysis: return 30;
                case OpportunityStage.ValueProposition: return 50;
                case OpportunityStage.DecisionMakers: return 60;
                case OpportunityStage.Proposal: return 75;
                case OpportunityStage.Negotiation: return 90;
                case OpportunityStage.ClosedWon: return 100;
                case OpportunityStage.ClosedLost: return 0;
                default: return 0;
            }
        }
    }
}
