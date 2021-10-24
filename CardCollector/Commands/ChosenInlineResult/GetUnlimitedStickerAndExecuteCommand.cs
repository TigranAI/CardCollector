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
    public class GetUnlimitedStickerAndExecuteCommand : ChosenInlineResultCommand
    {
        protected override string CommandText => Command.unlimited_stickers;
        protected override bool ClearMenu => false;
        protected override bool AddToStack => false;

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
        private static readonly List<ChosenInlineResultCommand> PrivateList = List.GetRange(1, List.Count - 1);
        
        /* Метод, создающий объекты команд исходя из полученного обновления */
        private static UpdateModel PrivateFactory(Update update, UserEntity user)
        {
            // Возвращаем объект, если команда совпала
            return PrivateList.FirstOrDefault(item => item.IsMatches(user, update)) is { } executor
                ? (UpdateModel) Activator.CreateInstance(executor.GetType(), user, update)
                : new CommandNotFound(user, update, update.ChosenInlineResult!.ResultId);
        }
    }
}