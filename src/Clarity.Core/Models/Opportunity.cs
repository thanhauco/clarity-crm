using System;
using System.ComponentModel.DataAnnotations;

namespace Clarity.Core.Models
{
    public class Opportunity
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }

        public OpportunityStage Stage { get; set; }

        [Range(0, 100)]
        public int Probability { get; set; }

        public decimal Amount { get; set; }
        public DateTime? CloseDate { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int? LeadId { get; set; }
        public Lead Lead { get; set; }

        public int AssignedUserId { get; set; }
        public User AssignedUser { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? WonAt { get; set; }
        public DateTime? LostAt { get; set; }

        public string LostReason { get; set; }
    }

    public enum OpportunityStage
    {
        Prospecting = 1,
        Qualification = 2,
        NeedsAnalysis = 3,
        ValueProposition = 4,
        DecisionMakers = 5,
        Proposal = 6,
        Negotiation = 7,
        ClosedWon = 8,
        ClosedLost = 9
    }
}
