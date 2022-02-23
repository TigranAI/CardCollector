using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Admin.Giveaway
{
    public class SelectGiveawayTier : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.select_giveaway_tier;

        protected override async Task Execute()
        {
            var module = User.Session.GetModule<AdminModule>();
            var giveaway = await Context.ChannelGiveaways.FindById(module.SelectedChannelGiveawayId!.Value);
            giveaway.SelectedStickerTier = int.Parse(CallbackQuery.Data!.Split("=")[1]);
            module.SelectedChannelGiveawayId = giveaway.Id;
            await User.Messages.EditMessage(User, Messages.select_channel, Keyboard.SelectChannel);
        }

        public SelectGiveawayTier(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user,
            context, callbackQuery)
        {
        }
    }
}