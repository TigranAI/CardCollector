using System.Linq;
using System.Threading.Tasks;
using CardCollector.Commands.MessageHandler.Admin.Distribution;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.CallbackQueryHandler.Admin.Distribution
{
    public class SetDistributionButtonValue : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.set_distribution_button_value;
        protected override async Task Execute()
        {
            var value = CallbackQuery.Data!.Split("=")[1];
            var module = User.Session.GetModule<AdminModule>();
            var distribution = await Context.ChatDistributions.FindById(module.ChatDistributionId!.Value);

            distribution.Buttons.Last().Value = value;
            Context.ChatDistributions.Update(distribution);

            await User.Messages.SendMessage(User, Messages.enter_distribution_button_name, Keyboard.BackKeyboard);

            EnterDistributionButtonName.AddToQueue(User.Id);
        }
    }
}