using System;
using System.ComponentModel.DataAnnotations;

namespace Clarity.Core.Models
{
    public class AuditLog
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string EntityName { get; set; }

        public int EntityId { get; set; }

        [Required]
        [StringLength(50)]
        public string Action { get; set; } // Create, Update, Delete

        public int? UserId { get; set; }
        public User User { get; set; }

        public DateTime Timestamp { get; set; }

        public string Changes { get; set; } // JSON or description of changes
    }
}
