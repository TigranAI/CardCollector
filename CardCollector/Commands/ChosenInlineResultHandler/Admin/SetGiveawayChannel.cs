using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Commands.MessageHandler.Admin.Giveaway;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.ChosenInlineResultHandler.Admin
{
    [MenuPoint]
    public class SetGiveawayChannel : ChosenInlineResultHandler
    {
        protected override string CommandText => ChosenInlineResultCommands.set_giveaway_channel;

        protected override async Task Execute()
        {
            var chatId = int.Parse(ChosenInlineResult.ResultId.Split("=")[1]);
            var chat = await Context.TelegramChats.FindById(chatId);
            var giveawayId = User.Session.GetModule<AdminModule>().SelectedChannelGiveawayId;
            if (chat == null || giveawayId == null) return;
            var giveaway = await Context.ChannelGiveaways.FindById(giveawayId.Value);
            giveaway.Channel = chat;
            await User.Messages.EditMessage(User, Messages.enter_number_of_prizes, Keyboard.BackKeyboard);
            EnterPrizeCount.AddToQueue(User.Id);
        }

        public override bool Match()
        {
            return base.Match() && User.PrivilegeLevel >= PrivilegeLevel.Programmer;
        }
    }
}