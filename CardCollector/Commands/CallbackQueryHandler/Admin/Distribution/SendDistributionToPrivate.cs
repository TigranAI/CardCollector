using System.Threading.Tasks;
using CardCollector.Database.EntityDao;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.CallbackQueryHandler.Admin.Distribution
{
    public class SendDistributionToPrivate : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.send_distribution_to_private;
        protected override async Task Execute()
        {
            var chatIds = await Context.Users.SelectUserChatIds();

            var module = User.Session.GetModule<AdminModule>();
            var distribution = await Context.ChatDistributions.FindById(module.ChatDistributionId!.Value);

            await distribution.Send(chatIds.ToArray());
        }
    }
}