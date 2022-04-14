using System.Threading.Tasks;
using CardCollector.Commands.MessageHandler.Admin.Distribution;
using CardCollector.Database.Entity;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.CallbackQueryHandler.Admin.Distribution
{
    public class CreateDistribution : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.create_distribution;
        protected override async Task Execute()
        {
            await User.Messages.SendMessage(User, Messages.enter_distribution_text, Keyboard.BackKeyboard);
            
            var distribution = new ChatDistribution();
            var result = await Context.ChatDistributions.AddAsync(distribution);
            await Context.SaveChangesAsync();
            
            var module = User.Session.GetModule<AdminModule>();
            module.ChatDistributionId = result.Entity.Id;
            
            EnterDistributionText.AddToQueue(User.Id);
        }

        public override bool Match()
        {
            return base.Match() && User.PrivilegeLevel >= PrivilegeLevel.Programmer;
        }
    }
}