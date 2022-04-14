using System.Threading.Tasks;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.CallbackQueryHandler.Admin.Distribution
{
    public class CheckDistribution : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.check_distribution;
        protected override async Task Execute()
        {
            var module = User.Session.GetModule<AdminModule>();
            var distribution = await Context.ChatDistributions.FindById(module.ChatDistributionId!.Value);
            await distribution.Send(User.ChatId);
            await User.Messages.SendMessage(User, Messages.send_distribution, Keyboard.SendDistribution);
        }
    }
}