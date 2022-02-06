using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.DataBase.Entity
{
    public class Payment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public User User { get; set; }
        public int TotalAmount { get; set; }
        [MaxLength(16)] public string InvoicePayload { get; set; }
        [MaxLength(127)] public string TelegramPaymentChargeId { get; set; }
        [MaxLength(127)] public string ProviderPaymentChargeId { get; set; }
    }
}