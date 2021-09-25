using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.ChosenInlineResult
{
    public class GetUnlimitedStickerAndExecuteCommand : ChosenInlineResult
    {
        protected override string CommandText => Command.unlimited_stickers;
        
        public override async Task Execute()
        {
            /* Получаем хеш стикера */
            var hash = InlineResult.Split('=')[1];
            /* Получаем объект стикера */
            var sticker = await StickerDao.GetStickerByHash(hash);
            /* Выдаем пользователю 1 стикер */
            await UserStickerRelationDao.AddNew(User, sticker, 1);
            /* Выполняем стандартный сценарий команды */
            await PrivateFactory(Update, User).Execute();
        }
        
        public GetUnlimitedStickerAndExecuteCommand() { }
        public GetUnlimitedStickerAndExecuteCommand(UserEntity user, Update update) : base(user, update) { }
        
        
        /* Список команд, аналогичный родительскому, только не включает эту команду (unlimited) */
        private static readonly List<ChosenInlineResult> PrivateList = List.GetRange(1, List.Count - 1);
        
        /* Метод, создающий объекты команд исходя из полученного обновления */
        private static UpdateModel PrivateFactory(Update update, UserEntity user)
        {
            // Текст команды
            var command = update.ChosenInlineResult!.ResultId;
            
            // Возвращаем объект, если команда совпала
            foreach (var item in PrivateList.Where(item => item.IsMatches(command)))
                if(Activator.CreateInstance(item.GetType(), user, update) is ChosenInlineResult executor)
                    if (executor.IsMatches(command)) return executor;
        
            // Возвращаем команда не найдена, если код дошел до сюда
            return new CommandNotFound(user, update, command);
        }
    }
}