using System.Threading.Tasks;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.CallbackQueryHandler.Admin.Giveaway
{
    public class SetGiveawayPrize : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.set_giveaway_prize;

        protected override async Task Execute()
        {
            User.Session.State = UserState.CreateGiveaway;
            var module = User.Session.GetModule<AdminModule>();
            var giveaway = await Context.ChannelGiveaways.FindById(module.SelectedChannelGiveawayId!.Value);
            giveaway!.Prize = (PrizeType) int.Parse(CallbackQuery.Data!.Split("=")[1]);
            module.SelectedChannelGiveawayId = giveaway.Id;
            if (giveaway.Prize is PrizeType.RandomSticker)
                await User.Messages.EditMessage(User, Messages.choose_tier, Keyboard.GiveawayTier);
            else
                await User.Messages.EditMessage(User, Messages.select_channel, Keyboard.SelectChannel);
        }

        public override bool Match()
        {
            return base.Match() && User.PrivilegeLevel >= PrivilegeLevel.Programmer;
        }
    }
}