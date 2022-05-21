using System.Linq;
using System.Threading.Tasks;
using CardCollector.Database.Entity.NotMapped;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.CallbackQueryHandler.Admin.Distribution
{
    public class CreateInlineButton : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.create_inline_button;
        protected override async Task Execute()
        {
            var module = User.Session.GetModule<AdminModule>();
            var distribution = await Context.ChatDistributions.FindById(module.ChatDistributionId!.Value);
            
            distribution.Buttons.Add(new ButtonInfo());
            
            distribution.Buttons.Last().Type = ButtonType.InlineMenu;
            
            await User.Messages.SendMessage(Messages.choose_option, Keyboard.DistributionInlineOptions);
        }
    }
}