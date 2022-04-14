using System.Threading.Tasks;
using CardCollector.Database.EntityDao;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.CallbackQueryHandler.Admin.Distribution
{
    public class SendDistributionToPrivateAndGroups : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.send_distribution_to_private_and_groups;
        protected override async Task Execute()
        {
            var chatIds = await Context.TelegramChats.SelectGroupChatIds();
            var userChatIds = await Context.Users.SelectUserChatIds();
            chatIds.AddRange(userChatIds);

            var module = User.Session.GetModule<AdminModule>();
            var distribution = await Context.ChatDistributions.FindById(module.ChatDistributionId!.Value);

            await distribution.Send(chatIds.ToArray());
        }
    }
}