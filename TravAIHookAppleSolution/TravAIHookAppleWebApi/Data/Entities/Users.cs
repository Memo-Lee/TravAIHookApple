using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TravAIHookAppleWebApi.Data.Entities
{
    public class Users
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReferanceId { get; set; }
        public Guid? UserId { get; set; } // Flutter UUID
        public string? ReceiptToken { get; set; }//NULL (abonelik yok)
        public string? Language { get; set; } //Flutter’dan gelen dil (örneğin "tr" veya "en")
        public string? Location { get; set; } //Flutter’dan gelen lokasyon (örn. "Turkey")
        public DateTime? SubscriptionExpireDate { get; set; } //NULL (çünkü premium değil)
        public int? RemainingMessages { get; set; } = 5; //(başlangıç hakkı)
        public DateTime? LastFreeMessageEarnedAt { get; set; } = DateTime.UtcNow; //şu anki zaman (GETUTCDATE())
        public string? SessionToken { get; set; }
        public DateTime? SessionExpiresAt { get; set; }
    }
}
