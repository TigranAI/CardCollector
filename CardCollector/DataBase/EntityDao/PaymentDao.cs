using System;
using System.Threading;
using CardCollector.DataBase.Entity;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types.Payments;

namespace CardCollector.DataBase.EntityDao
{
    public static class PaymentDao
    {
        public static BotDatabase Instance;
        public static DbSet<Payment> Table;

        static PaymentDao()
        {
            Instance = BotDatabase.GetClassInstance(typeof(PaymentDao));
            Table = Instance.Payments;
        }
        
        public static async void AddPayment(SuccessfulPayment payment, UserEntity user, int updateId)
        {
            try
            {
                await Table.AddAsync(new Payment()
                {
                    Id = updateId,
                    UserId = user.Id,
                    InvoicePayload = payment.InvoicePayload,
                    ProviderPaymentChargeId = payment.ProviderPaymentChargeId,
                    TelegramPaymentChargeId = payment.TelegramPaymentChargeId,
                    TotalAmount = payment.TotalAmount
                });
                await Instance.SaveChangesAsync();
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(Utilities.rnd.Next(30));
                AddPayment(payment, user, updateId);
            }
        }
    }
}