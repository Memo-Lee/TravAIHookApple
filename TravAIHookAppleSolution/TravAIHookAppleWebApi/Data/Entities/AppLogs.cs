using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TravAIHookAppleWebApi.Data.Entities
{
    [Table("AppLogs")]
    public class AppLogs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public Guid? UserId { get; set; }

        [MaxLength(100)]
        public string? Endpoint { get; set; }

        [MaxLength(10)]
        public string? Method { get; set; }

        [MaxLength(200)]
        public string? Message { get; set; }

        [MaxLength(2)]
        public string? Language { get; set; }

        public int? ConversationId { get; set; }

        [MaxLength(500)]
        public string? Error { get; set; }

        [MaxLength(20)]
        public string? Status { get; set; }

        public int? InputTokens { get; set; }
        public int? OutputTokens { get; set; }
        public decimal? EstimatedCost { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
