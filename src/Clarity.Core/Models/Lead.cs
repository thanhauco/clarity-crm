using System;
using System.ComponentModel.DataAnnotations;

namespace Clarity.Core.Models
{
    public class Lead
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Phone]
        public string Phone { get; set; }

        [StringLength(200)]
        public string Company { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        public LeadStatus Status { get; set; }
        public LeadSource Source { get; set; }
        public int? Rating { get; set; }

        public decimal? EstimatedValue { get; set; }
        public string Description { get; set; }

        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int? AssignedUserId { get; set; }
        public User AssignedUser { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ConvertedAt { get; set; }
        public DateTime? ClosedAt { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }

    public enum LeadStatus
    {
        New = 1,
        Contacted = 2,
        Qualified = 3,
        Proposal = 4,
        Negotiation = 5,
        Won = 6,
        Lost = 7,
        Disqualified = 8
    }

    public enum LeadSource
    {
        Website = 1,
        Referral = 2,
        ColdCall = 3,
        Email = 4,
        TradeShow = 5,
        SocialMedia = 6,
        Advertisement = 7,
        Partner = 8,
        Other = 9
    }
}
