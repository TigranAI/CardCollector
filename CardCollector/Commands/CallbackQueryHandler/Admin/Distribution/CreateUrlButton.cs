using System.Linq;
using System.Threading.Tasks;
using CardCollector.Commands.MessageHandler.Admin.Distribution;
using CardCollector.Database.Entity.NotMapped;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.CallbackQueryHandler.Admin.Distribution
{
    public class CreateUrlButton : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.create_url_button;
        protected override async Task Execute()
        {
            var module = User.Session.GetModule<AdminModule>();
            var distribution = await Context.ChatDistributions.FindById(module.ChatDistributionId!.Value);
            
            distribution.Buttons.Add(new ButtonInfo());
            
            distribution.Buttons.Last().Type = ButtonType.Url;
            
            await User.Messages.SendMessage(Messages.enter_button_url, Keyboard.BackKeyboard);

            EnterButtonUrl.AddToQueue(User.Id);
        }
    }
}