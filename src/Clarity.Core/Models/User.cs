using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Clarity.Core.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        public UserRole Role { get; set; }
        public bool IsActive { get; set; }

        public string Avatar { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? LastLoginAt { get; set; }

        public ICollection<Customer> AssignedCustomers { get; set; }
        public ICollection<Lead> AssignedLeads { get; set; }
        public ICollection<Activity> AssignedActivities { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }

    public enum UserRole
    {
        Admin = 1,
        Manager = 2,
        SalesRep = 3,
        Support = 4,
        ReadOnly = 5
    }
}
