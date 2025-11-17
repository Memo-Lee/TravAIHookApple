using System;
using TravAIHookAppleWebApi.Data;
using TravAIHookAppleWebApi.Data.Entities;

namespace TravAIHookAppleWebApi.Middlewares
{
    public class AppLogger
    {
        private readonly AppDbContext _db;
        public AppLogger(AppDbContext db)
        {
            _db = db;
        }

        public async Task LogAsync(
            string endpoint,
            string method,
            string message,
            string? language,
            int conversationId,
            string status = "Success",
            Guid? userId = null,
            string? error = null,
            int? inputTokens = null,
            int? outputTokens = null,
            decimal? estimatedCost = null)
        {
            try
            {
                var log = new AppLogs
                {
                    UserId = userId,
                    Endpoint = endpoint,
                    Method = method,
                    Message = message,
                    Language = language,
                    ConversationId = conversationId,
                    Error = error,
                    Status = status,
                    InputTokens = inputTokens,
                    OutputTokens = outputTokens,
                    EstimatedCost = estimatedCost,
                    CreatedAt = DateTime.UtcNow
                };

                await _db.AppLogs.AddAsync(log);
                await _db.SaveChangesAsync();
            }
            catch
            {
                // Sessiz hata
            }
        }
    }
}
