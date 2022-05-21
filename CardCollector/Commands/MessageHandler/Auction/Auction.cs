using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.MessageHandler.Auction
{
    [MenuPoint]
    public class Auction : MessageHandler
    {
        protected override string CommandText => MessageCommands.auction;
        protected override bool ClearMenu => true;
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            User.Session.ResetModules();
            User.Session.State = UserState.AuctionMenu;
            var text = User.Session.GetModule<FiltersModule>().ToString(User.Session.State);
            await User.Messages.EditMessage(text, Keyboard.GetSortingMenu(User.Session.State));
        }
    }
}