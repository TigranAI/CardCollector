using System.Threading.Tasks;
using CardCollector.Cache.Repository;
using CardCollector.Commands.CallbackQueryHandler;
using CardCollector.Controllers;
using CardCollector.Database.EntityDao;
using CardCollector.Extensions.Database.Entity;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Commands.MessageHandler.Group;

public class StartPuzzle : MessageHandler
{
    protected override string CommandText => MessageCommands.start_puzzle;
    protected override async Task Execute()
    {
        var chat = await Context.TelegramChats.FindByChat(Message.Chat);
        var userRepo = new UserInfoRepository();
        var userInfo = await userRepo.GetAsync(User);
        
        if (userInfo.PuzzleChatId != 0)
        {
            await chat.SendMessage(string.Format(Messages.you_are_now_playing_puzzle, User.GetMention()), parseMode: ParseMode.Html);
            return;
        }
        
        var puzzleRepo = new PuzzleInfoRepository();
        var puzzleInfo = await puzzleRepo.GetAsync(chat);
        
        if (puzzleInfo.StickerId != -1)
        {
            await chat.SendMessage(Messages.puzzle_now_started);
            return;
        }

        if (puzzleInfo.CreatorId != 0)
        {
            await chat.SendMessage(Messages.puzzle_now_created);
            return;
        }

        userInfo.PuzzleChatId = chat.Id;
        puzzleInfo.CreatorId = User.Id;

        await Games.Puzzle.SendPrepareMessage(puzzleInfo, chat, Context);
        
        await userRepo.SaveAsync(User, userInfo);
        await puzzleRepo.SaveAsync(chat, puzzleInfo);
        
        TimerController.SetupTimer(Constants.PUZZLE_INTERVAL, (_, _) => Games.Puzzle.Start(chat.Id));
    }
    
    public override bool Match()
    {
        if (Message.Type != MessageType.Text) return false;
        var data = Message.Text!.Split("@");
        if (data.Length < 2) return false;
        if (data[0] != CommandText || data[1] != AppSettings.NAME) return false;
        return Message.Chat.Type is ChatType.Group or ChatType.Supergroup;
    }

    public static InlineKeyboardMarkup Keyboard(long chatId)
    {
        return new InlineKeyboardMarkup(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.start_game, 
                $"{CallbackQueryCommands.start_puzzle}={chatId}")},
            new[] {InlineKeyboardButton.WithCallbackData(Text.join_game, 
                $"{CallbackQueryCommands.join_puzzle}={chatId}")},
            new[] {InlineKeyboardButton.WithCallbackData(Text.game_rules, 
                $"{CallbackQueryCommands.send_rules}={GamesType.Puzzle}")},
        });
    }
}