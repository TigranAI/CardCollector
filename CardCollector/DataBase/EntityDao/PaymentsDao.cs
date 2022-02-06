using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types.Payments;

namespace CardCollector.DataBase.EntityDao
{
    public static class PaymentsDao
    {
        public static async Task Save(this DbSet<Payment> table, User user, SuccessfulPayment payment)
        {
            var entity = new Payment()
            {
                User = user,
                InvoicePayload = payment.InvoicePayload,
                ProviderPaymentChargeId = payment.ProviderPaymentChargeId,
                TelegramPaymentChargeId = payment.TelegramPaymentChargeId,
                TotalAmount = payment.TotalAmount
            };
            await table.AddAsync(entity);
        }
    }
}