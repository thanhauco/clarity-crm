using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clarity.Core.Interfaces;
using Clarity.Core.Models;
using Clarity.Core.Utilities;

namespace Clarity.Services
{
    public interface IAuditLogService
    {
        Task LogAsync(string entityName, int entityId, string action, int? userId, string changes);
        Task<IEnumerable<AuditLog>> GetLogsForEntityAsync(string entityName, int entityId);
        Task<IEnumerable<AuditLog>> GetLogsByUserAsync(int userId);
    }

    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLogRepository _repo;

        public AuditLogService(IAuditLogRepository repo)
        {
            _repo = repo;
        }

        public async Task LogAsync(string entityName, int entityId, string action, int? userId, string changes)
        {
            var log = new AuditLog
            {
                EntityName = entityName,
                EntityId = entityId,
                Action = action,
                UserId = userId,
                Changes = changes,
                Timestamp = DateTime.UtcNow
            };

            await _repo.CreateAsync(log);
            Logger.LogInfo($"Audit: {action} on {entityName} #{entityId} by User {userId}");
        }

        public async Task<IEnumerable<AuditLog>> GetLogsForEntityAsync(string entityName, int entityId)
        {
            return await _repo.GetByEntityAsync(entityName, entityId);
        }

        public async Task<IEnumerable<AuditLog>> GetLogsByUserAsync(int userId)
        {
            return await _repo.GetByUserAsync(userId);
        }
    }
}
