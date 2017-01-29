using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Clarity.Core.Models
{
    public class Customer
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
        [StringLength(255)]
        public string Email { get; set; }

        [Phone]
        [StringLength(20)]
        public string Phone { get; set; }

        [StringLength(200)]
        public string Company { get; set; }

        [StringLength(100)]
        public string Title { get; set; }

        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }

        public CustomerStatus Status { get; set; }
        public CustomerType Type { get; set; }

        public decimal? AnnualRevenue { get; set; }
        public int? EmployeeCount { get; set; }

        public string Notes { get; set; }

        public int? AssignedUserId { get; set; }
        public User AssignedUser { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int CreatedById { get; set; }

        public ICollection<Activity> Activities { get; set; }
        public ICollection<Lead> Leads { get; set; }
        public ICollection<Opportunity> Opportunities { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }

    public enum CustomerStatus
    {
        Active = 1,
        Inactive = 2,
        Prospect = 3,
        Churned = 4
    }

    public enum CustomerType
    {
        Individual = 1,
        Business = 2,
        Enterprise = 3,
        Government = 4,
        NonProfit = 5
    }
}
