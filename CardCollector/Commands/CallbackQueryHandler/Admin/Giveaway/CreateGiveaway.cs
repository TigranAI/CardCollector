using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Admin.Giveaway
{
    public class CreateGiveaway : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.create_giveaway;

        protected override async Task Execute()
        {
            User.Session.State = UserState.CreateGiveaway;
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