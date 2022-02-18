using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Admin.Giveaway
{
    public class ConfirmGiveaway : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.confirm_giveaway;
        protected override async Task Execute()
        {
            var giveawayId = User.Session.GetModule<AdminModule>().SelectedChannelGiveawayId;
            if (giveawayId == null) return;
            var giveaway = await Context.ChannelGiveaways.FindById(giveawayId.Value);
            /*await giveaway.Start();*/
        }

        public ConfirmGiveaway(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery)
        {
        }
    }
}