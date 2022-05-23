using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Cache.Entity;
using CardCollector.Cache.Repository;
using CardCollector.Commands.CallbackQueryHandler;
using CardCollector.Commands.InlineQueryHandler;
using CardCollector.Database;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Extensions;
using CardCollector.Extensions.Database.Entity;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Games;

public static class Puzzle
{
    public static readonly Dictionary<long, PuzzleTimer> TurnTimers = new();

    public static readonly int PUZZLE_MIN_PLAYERS = Constants.DEBUG ? 1 : 2;
    public static readonly int PUZZLE_MAX_PLAYERS = Constants.DEBUG ? 2 : 5;
    public static readonly int PUZZLE_MAX_REWARDS = Constants.DEBUG ? 2 : 5;
    public static readonly int MIN_MEMBERS_IN_CHAT = Constants.DEBUG ? 2 : 20;

    public static async void Start(long chatId)
    {
        using (var context = new BotDatabaseContext())
        {
            var chat = await context.TelegramChats.FindById(chatId);
            await Start(context, chat!);
            await context.SaveChangesAsync();
        }
    }

    public static async Task Start(BotDatabaseContext context, TelegramChat chat)
    {
        var puzzleRepo = new PuzzleInfoRepository();
        var puzzleInfo = await puzzleRepo.GetAsync(chat);
        if (puzzleInfo.StickerId != -1 || puzzleInfo.CreatorId == 0) return;

        await chat.DeleteMessage(puzzleInfo.MessageId);
        if (PUZZLE_MIN_PLAYERS > puzzleInfo.Players.Count + 1)
        {
            await chat.SendMessage(Messages.too_few_players);
            await UndoGame(context, chat, puzzleInfo, puzzleRepo);
            return;
        }

        if (PUZZLE_MAX_PLAYERS < puzzleInfo.Players.Count + 1)
        {
            await chat.SendMessage(Messages.too_many_players);
            await UndoGame(context, chat, puzzleInfo, puzzleRepo);
            return;
        }

        await StartGame(context, chat, puzzleInfo);
        await puzzleRepo.SaveAsync(chat, puzzleInfo);
    }

    private static async Task StartGame(BotDatabaseContext context, TelegramChat chat, PuzzleInfo puzzleInfo)
    {
        puzzleInfo.StickerId = 0;
        puzzleInfo.Players.Shuffle();
        if (puzzleInfo.IsSuperGame())
            await chat.SendMessage(Text.fireworks, SuperGameKeyboard());
        await chat.SendMessage(
            string.Format(Messages.puzzle_start_message, await GetMentionList(context, puzzleInfo.GetPlayers())),
            ChooseStickerKeyboard(),
            ParseMode.Html);

        TurnTimers.Add(chat.Id, PuzzleTimer.Of(delegate { BreakGame(chat.Id); }));
    }

    private static async Task UndoGame(BotDatabaseContext context, TelegramChat chat, PuzzleInfo puzzleInfo,
        PuzzleInfoRepository puzzleRepo)
    {
        var players = puzzleInfo.GetPlayers();
        await ClearPlayerList(context, players);
        puzzleInfo.Reset();
        await puzzleRepo.SaveAsync(chat, puzzleInfo);
    }

    private static async Task ClearPlayerList(BotDatabaseContext context, List<long> players, bool success = false)
    {
        var userRepo = new UserInfoRepository();
        await players.ApplyAsync(async playerId =>
        {
            var user = await context.Users.FindById(playerId);
            if (success)
                user!.UserStats.IncreasePuzzleGames();
            var info = await userRepo.GetAsync(user!);
            info.PuzzleChatId = 0;
            await userRepo.SaveAsync(user, info);
        });
    }

    private static async void BreakGame(long chatId)
    {
        using (var context = new BotDatabaseContext())
        {
            var chat = await context.TelegramChats.FindById(chatId);
            await BreakGame(context, chat!, true);
            await context.SaveChangesAsync();
        }
    }

    public static async Task BreakGame(BotDatabaseContext context, TelegramChat chat, bool endOfTurn = false)
    {
        if (TurnTimers.TryGetValue(chat.Id, out var timer)) timer.Dispose();
        TurnTimers.Remove(chat.Id);
        var puzzleRepo = new PuzzleInfoRepository();
        var puzzleInfo = await puzzleRepo.GetAsync(chat);
        var players = puzzleInfo.GetPlayers();
        var looser = await context.Users.FindById(players[puzzleInfo.Turn]);
        await chat.SendMessage(
            string.Format(endOfTurn ? Messages.puzzle_end_of_turn : Messages.puzzle_wrong_piece, looser!.GetMention()),
            parseMode: ParseMode.Html);

        await ClearPlayerList(context, players);
        puzzleInfo.EndGame();
        await puzzleRepo.SaveAsync(chat, puzzleInfo);
    }

    public static async Task SendPrepareMessage(PuzzleInfo puzzleInfo, TelegramChat chat, BotDatabaseContext context)
    {
        var creator = await context.Users.FindById(puzzleInfo.CreatorId);
        var message = string.Format(Messages.puzzle_message, creator!.GetMention(),
            await GetMentionList(context, puzzleInfo.Players));

        puzzleInfo.MessageId = puzzleInfo.MessageId == -1
            ? await chat.SendMessage(message, MenuKeyboard(chat.Id), ParseMode.Html)
            : await chat.EditMessage(message, puzzleInfo.MessageId, MenuKeyboard(chat.Id), ParseMode.Html);
    }

    private static async Task<string> GetMentionList(BotDatabaseContext context, List<long> playersIds)
    {
        var playersMention = new List<string>();
        await playersIds.ApplyAsync(async playerId =>
        {
            var player = await context.Users.FindById(playerId);
            if (player != null) playersMention.Add(player.GetMention());
        });
        return string.Join("\n", playersMention);
    }

    public static InlineKeyboardMarkup MenuKeyboard(long chatId)
    {
        return new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(Text.start_game,
                    $"{CallbackQueryCommands.start_puzzle}={chatId}")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(Text.join_game,
                    $"{CallbackQueryCommands.join_puzzle}={chatId}")
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData(Text.game_rules,
                    $"{CallbackQueryCommands.send_rules}={(int) GamesType.Puzzle}")
            },
        });
    }

    public static InlineKeyboardMarkup SuperGameKeyboard()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(Text.supergame, CallbackQueryCommands.puzzle_supergame)
            }
        });
    }

    public static InlineKeyboardMarkup ChooseStickerKeyboard()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new[]
            {
                InlineKeyboardButton.WithSwitchInlineQueryCurrentChat(Text.choose_sticker, InlineQueryCommands.puzzle)
            }
        });
    }

    public static async Task EndOfGame(BotDatabaseContext context, TelegramChat chat,
        PuzzleInfoRepository puzzleRepo, PuzzleInfo puzzleInfo)
    {
        if (TurnTimers.TryGetValue(chat.Id, out var timer)) timer.Dispose();
        TurnTimers.Remove(chat.Id);
        var players = puzzleInfo.GetPlayers();

        var message = string.Format(Messages.congratulation_you_solve_puzzle,
            chat.MembersCount > MIN_MEMBERS_IN_CHAT
                ? puzzleInfo.GamesToday < PUZZLE_MAX_REWARDS
                    ? await GetRewards(context, players)
                    : Messages.no_rewards_available
                : Messages.no_rewards_available_in_this_chat);

        await chat.SendMessage(message, parseMode: ParseMode.Html);

        await ClearPlayerList(context, players, true);
        puzzleInfo.EndGame();
        await puzzleRepo.SaveAsync(chat, puzzleInfo);
    }

    private static async Task<string> GetRewards(BotDatabaseContext context, List<long> players)
    {
        var rewardList = new List<string>();
        var userRepo = new UserInfoRepository();
        await players.Shuffle().ApplyAsync(async playerId =>
        {
            var user = await context.Users.FindById(playerId);
            var info = await userRepo.GetAsync(user!);
            info.PuzzleChatId = 0;
            await userRepo.SaveAsync(user, info);

            object reward = players.Count < 4
                ? await context.Stickers.FindAllByTier(1)
                : await context.Packs.FindById(1);

            if (rewardList.Count < players.Count - 1 || players.Count == 5)
                rewardList.Add(await GetReward(reward, user));
        });

        return string.Join("\n", rewardList);
    }

    private static async Task<string> GetReward(object reward, User user)
    {
        if (reward is Sticker[] stickers)
        {
            var sticker = stickers.Random();
            await user.AddSticker(sticker, 1);
            await user.Stickers
                .Where(us => us.Sticker.ExclusiveTask is ExclusiveTask.ClaimPuzzlePrize)
                .ApplyAsync(async us => await us.DoExclusiveTask());
            return string.Format(Messages.user_receives_reward, user.GetMention(), sticker.Title);
        }
        else if (reward is Pack basePack)
        {
            user.AddPack(basePack, 1);
            await user.Stickers
                .Where(us => us.Sticker.ExclusiveTask is ExclusiveTask.ClaimPuzzlePrize)
                .ApplyAsync(async us => await us.DoExclusiveTask());
            return string.Format(Messages.user_receives_reward, user.GetMention(), basePack.Author);
        }

        return "";
    }
}