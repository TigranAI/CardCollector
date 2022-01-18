using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CardCollector.Migrations.Database_old.Entity
{
    [Table("payments")]
    public class Payment
    {
        [Key] [Column("id"), MaxLength(32)] public int Id { get; set; }
        [Column("user_id"), MaxLength(127)] public long UserId { get; set; }
        [Column("total_amount"), MaxLength(32)] public int TotalAmount { get; set; }
        [Column("invoice_payload"), MaxLength(16)] public string InvoicePayload { get; set; }
        [Column("telegram_payment_charge_id"), MaxLength(127)] public string TelegramPaymentChargeId { get; set; }
        [Column("provider_payment_charge_id"), MaxLength(127)] public string ProviderPaymentChargeId { get; set; }
    }
}