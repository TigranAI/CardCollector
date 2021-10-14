using System.Threading.Tasks;
using CardCollector.Resources;

namespace CardCollector.Commands.CallbackQuery
{
    public class OpenPackCallback : CallbackQuery
    {
        protected override string CommandText => Command.open_pack;
        
        public override async Task Execute()
        {
            await User.ClearChat();
            var packId = int.Parse(CallbackData.Split("=")[1]);
            
        }
    }
}