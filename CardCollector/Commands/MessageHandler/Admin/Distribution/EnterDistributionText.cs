using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.MessageHandler.Admin.Distribution
{
    public class EnterDistributionText : MessageHandler
    {
        protected override string CommandText => "";
        
        private static readonly LinkedList<long> Queue = new();
        
        protected override async Task Execute()
        {
            RemoveFromQueue(User.Id);

            var module = User.Session.GetModule<AdminModule>();
            var distribution = await Context.ChatDistributions.FindById(module.ChatDistributionId!.Value);
            distribution.Text = Message.Text!;
            
            await User.Messages.SendMessage(User, Messages.send_distribution_image, 
                Keyboard.SkipKeyboard(typeof(SendDistributionImage).Name));
            
            SendDistributionImage.AddToQueue(User.Id);
        }

        public static void AddToQueue(long userId)
        {
            if (!Queue.Contains(userId)) Queue.AddLast(userId);
        }

        public static void RemoveFromQueue(long userId)
        {
            Queue.Remove(userId);
        }

        public override bool Match()
        {
            return Queue.Contains(User.Id) && Message.Type == MessageType.Text;
        }
    }
}