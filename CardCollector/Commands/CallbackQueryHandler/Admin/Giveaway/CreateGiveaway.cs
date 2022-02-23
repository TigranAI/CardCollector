using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Admin.Giveaway
{
    public class CreateGiveaway : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.create_giveaway;

        protected override async Task Execute()
        {
            User.Session.State = UserState.SelectGiveawayPrize;
            var giveaway = await Context.ChannelGiveaways.CreateNew();
            await Context.SaveChangesAsync();
            User.Session.GetModule<AdminModule>().SelectedChannelGiveawayId = giveaway.Id;
            await User.Messages.EditMessage(User, Messages.choose_option, Keyboard.GiveawayKeyboard);
        }

        public override bool Match()
        {
            return base.Match() && User.PrivilegeLevel >= PrivilegeLevel.Programmer;
        }

        public CreateGiveaway(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context,
            callbackQuery)
        {
        }
    }
}