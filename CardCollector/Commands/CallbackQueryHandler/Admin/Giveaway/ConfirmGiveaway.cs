using System;
using System.Threading;
using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Admin.Giveaway
{
    public class ConfirmGiveaway : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.confirm_giveaway;
        protected override bool ClearMenu => true;

        protected override async Task Execute()
        {
            await User.Messages.ClearChat(User);
            var giveawayId = User.Session.GetModule<AdminModule>().SelectedChannelGiveawayId;
            var giveaway = await Context.ChannelGiveaways.FindById(giveawayId);
            try
            {
                var timer = await giveaway!.Prepare();
                await User.Messages.SendMessage(User,
                    string.Format(Messages.giveaway_successfully_created, giveaway.GetUrl()),
                    Keyboard.BackKeyboard, ParseMode.Html);
                if (timer != null)
                {
                    var resetEvent = new ManualResetEvent(false);
                    timer.Disposed += (_, _) => resetEvent.Set();
                    resetEvent.WaitOne();
                }
            }
            catch (Exception e)
            {
                await User.Messages.SendMessage(User,
                    string.Format(Messages.error_when_preparing_giveaway, e.Message), Keyboard.BackKeyboard);
                Logs.LogOutWarning(e);
                Context.ChannelGiveaways.Attach(giveaway);
                Context.ChannelGiveaways.Remove(giveaway);
            }
        }

        public ConfirmGiveaway(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context,
            callbackQuery)
        {
        }
    }
}