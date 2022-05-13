using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Resources;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Profile
{
    [MenuPoint]
    public class MyPacks : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.my_packs;
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            var random = User.Packs.SingleOrDefault(item => item.Pack.Id == 1);
            var authorCount = User.Packs.Sum(item => item.Count) - (random?.Count ?? 0);
            await User.Messages.EditMessage(User, 
                $"{Messages.your_packs}" +
                $"\n{Messages.random_packs}: {random?.Count ?? 0}{Text.items}" +
                $"\n{Messages.author_pack}: {authorCount}{Text.items}",
                Keyboard.PackMenu);
        }
    }
}