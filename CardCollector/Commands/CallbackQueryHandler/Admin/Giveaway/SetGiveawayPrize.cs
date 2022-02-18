using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Admin.Giveaway
{
    public class SetGiveawayPrize : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.set_giveaway_prize;

        protected override async Task Execute()
        {
            var module = User.Session.GetModule<AdminModule>();
            var giveaway = await Context.ChannelGiveaways.FindById(module.SelectedChannelGiveawayId!.Value);
            giveaway.Prize = (ChannelGiveaway.PrizeType) int.Parse(CallbackQuery.Data!.Split("=")[1]);
            module.SelectedChannelGiveawayId = giveaway.Id;
            await User.Messages.EditMessage(User, Messages.select_channel, Keyboard.SelectChannel);
        }

        public override bool Match()
        {
            return base.Match() && User.PrivilegeLevel >= PrivilegeLevel.Programmer;
        }

        public SetGiveawayPrize(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user,
            context, callbackQuery)
        {
        }
    }
}