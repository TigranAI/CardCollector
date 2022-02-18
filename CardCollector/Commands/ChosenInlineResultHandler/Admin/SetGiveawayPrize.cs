using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.DataBase;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.ChosenInlineResultHandler.Admin
{
    [MenuPoint]
    public class SetGiveawayPrize : ChosenInlineResultHandler
    {
        protected override string CommandText => ChosenInlineResultCommands.set_giveaway_prize;

        protected override async Task Execute()
        {
            var stickerId = long.Parse(ChosenInlineResult.ResultId.Split("=")[1]);
            var sticker = await Context.Stickers.FindById(stickerId);
            var module = User.Session.GetModule<AdminModule>();
            var giveaway = await Context.ChannelGiveaways.FindById(module.SelectedChannelGiveawayId!.Value);
            giveaway.Prize = ChannelGiveaway.PrizeType.SelectedSticker;
            giveaway.SelectedSticker = sticker;
            module.SelectedChannelGiveawayId = giveaway.Id;
            await User.Messages.EditMessage(User, Messages.select_channel, Keyboard.SelectChannel);
        }

        public SetGiveawayPrize(User user, BotDatabaseContext context, ChosenInlineResult chosenInlineResult) : base(
            user, context, chosenInlineResult)
        {
        }
    }
}