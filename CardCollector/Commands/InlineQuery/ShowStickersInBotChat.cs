﻿using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.InlineQuery
{
    /* Отображение стикеров в личной беседt с ботом */
    public class ShowStickersInBotChat : InlineQuery
    {
        /* Команда - пустая строка, поскольку пользователь может вводить любые слова
         после @имя_бота, введенная фраза будет использоваться для фильтрации стикеров */
        protected override string CommandText => "";
        
        public override async Task Execute()
        {
            // Фильтр - введенная пользователем фраза
            var filter = Update.InlineQuery!.Query;
            // Получаем список стикеров
            var stickersList = await User.GetStickersList(Command.select_sticker, filter, true);
            // Посылаем пользователю ответ на его запрос
            await MessageController.AnswerInlineQuery(InlineQueryId, stickersList);
        }

        /* Команда пользователя удовлетворяет условию, если она вызвана
         в личных сообщениях с ботом */
        protected internal override bool IsMatches(string command)
        {
            return command.Contains("Sender");
        }

        public ShowStickersInBotChat() { }
        public ShowStickersInBotChat(UserEntity user, Update update, string inlineQueryId) : base(user, update, inlineQueryId) { }
    }
}