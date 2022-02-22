﻿using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Others;
using CardCollector.Resources;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.MessageHandler.Group
{
    public class Roulette : MessageHandler
    {
        protected override string CommandText => MessageCommands.roulette;
        protected override async Task Execute()
        {
            var telegramChat = await Context.TelegramChats.FindChat(Message.Chat);
            if (Context.ChatRoulette.Any(item => !item.IsStarted && item.Group.Id == telegramChat.Id))
            {
                await telegramChat.SendMessage(Messages.roulette_start_now);
                return;
            }
            var roulette = new ChatRoulette()
            {
                Creator = User,
                Group = telegramChat
            };
            var entityResult = await Context.ChatRoulette.AddAsync(roulette);
            roulette = entityResult.Entity;
            await Context.SaveChangesAsync();
            var messageId = await telegramChat.SendMessage(string.Format(Messages.roulette_message, User.Username, ""),
                Keyboard.RouletteKeyboard(roulette.Id));
            roulette.MessageId = messageId;
            TimerController.SetupTimer(Constants.ROULETTE_INTERVAL, roulette.Start);
        }

        public override bool Match()
        {
            if (Message.Type != MessageType.Text) return false;
            if (Message.Text!.Split("@")[0] != CommandText) return false;
            return Message.Chat.Type is ChatType.Group or ChatType.Supergroup;
        }

        public Roulette(User user, BotDatabaseContext context, Message message) : base(user, context, message)
        {
        }
    }
}