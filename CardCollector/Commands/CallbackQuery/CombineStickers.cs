using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class CombineStickers : CallbackQuery
    {
        protected override string CommandText => Command.combine_stickers;
        public override async Task Execute()
        {
            
        }

        public CombineStickers() { }
        public CombineStickers(UserEntity user, Update update) : base(user, update) { }
    }
}