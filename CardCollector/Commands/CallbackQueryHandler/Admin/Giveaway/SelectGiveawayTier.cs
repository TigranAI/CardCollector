using System.Threading.Tasks;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.CallbackQueryHandler.Admin.Giveaway
{
    public class SelectGiveawayTier : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.select_giveaway_tier;

        protected override async Task Execute()
        {
            var module = User.Session.GetModule<AdminModule>();
            var giveaway = await Context.ChannelGiveaways.FindById(module.SelectedChannelGiveawayId!.Value);
            giveaway!.SelectedStickerTier = int.Parse(CallbackQuery.Data!.Split("=")[1]);
            module.SelectedChannelGiveawayId = giveaway.Id;
            await User.Messages.EditMessage(User, Messages.select_channel, Keyboard.SelectChannel);
        }
    }
}