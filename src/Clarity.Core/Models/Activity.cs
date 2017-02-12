using System;
using System.ComponentModel.DataAnnotations;

namespace Clarity.Core.Models
{
    public class Activity
    {
        public int Id { get; set; }

        [Required]
        public ActivityType Type { get; set; }

        [Required]
        [StringLength(200)]
        public string Subject { get; set; }

        public string Description { get; set; }

        public ActivityStatus Status { get; set; }
        public ActivityPriority Priority { get; set; }

        public DateTime? DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }

        public int? DurationMinutes { get; set; }

        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }

        public int? LeadId { get; set; }
        public Lead Lead { get; set; }

        public int? OpportunityId { get; set; }
        public Opportunity Opportunity { get; set; }

        public int AssignedUserId { get; set; }
        public User AssignedUser { get; set; }

        public DateTime CreatedAt { get; set; }
        public int CreatedById { get; set; }
    }

    public enum ActivityType
    {
        Call = 1,
        Email = 2,
        Meeting = 3,
        Task = 4,
        Note = 5,
        Follow_Up = 6
    }

    public enum ActivityStatus
    {
        Pending = 1,
        InProgress = 2,
        Completed = 3,
        Cancelled = 4,
        Deferred = 5
    }

    public enum ActivityPriority
    {
        Low = 1,
        Normal = 2,
        High = 3,
        Urgent = 4
    }
}
