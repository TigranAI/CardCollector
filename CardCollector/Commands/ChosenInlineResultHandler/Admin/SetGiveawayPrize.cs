using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.ChosenInlineResultHandler.Admin
{
    [MenuPoint]
    public class SetGiveawayPrize : ChosenInlineResultHandler
    {
        protected override string CommandText => ChosenInlineResultCommands.set_giveaway_prize;

        protected override async Task Execute()
        {
            User.Session.State = UserState.CreateGiveaway;
            var stickerId = long.Parse(ChosenInlineResult.ResultId.Split("=")[1]);
            var sticker = await Context.Stickers.FindById(stickerId);
            var module = User.Session.GetModule<AdminModule>();
            var giveaway = await Context.ChannelGiveaways.FindById(module.SelectedChannelGiveawayId!.Value);
            giveaway.Prize = PrizeType.SelectedSticker;
            giveaway.SelectedSticker = sticker;
            module.SelectedChannelGiveawayId = giveaway.Id;
            await User.Messages.EditMessage(User, Messages.select_channel, Keyboard.SelectChannel);
        }
    }
}