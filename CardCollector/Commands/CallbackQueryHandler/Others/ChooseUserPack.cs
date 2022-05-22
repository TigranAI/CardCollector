using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Database.Entity;
using CardCollector.Extensions.Database.Entity;
using CardCollector.Others;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Commands.CallbackQueryHandler.Others;

public class ChooseUserPack : CallbackQueryHandler
{
    protected override string CommandText => CallbackQueryCommands.choose_user_pack;
    protected override bool ClearStickers => true;
    protected override async Task Execute()
    {
        var data = CallbackQuery.Data!.Split('=');
        var offset = int.Parse(data[2]);
        var packs = data.Length == 4
            ? User.Packs.FindNextSkipRandom(offset, 10, data[3] == "1")
            : User.Packs.FindNextSkipRandom(offset, 10);
        if (packs.Count == 0)
            await AnswerCallbackQuery(User, CallbackQuery.Id, Messages.page_not_found);
        else
            await User.Messages.EditMessage(Messages.choose_author,
                Keyboard(packs, offset, User.Packs.Count, data));
    }
    
    private InlineKeyboardMarkup Keyboard(List<UserPacks> list, int offset,
        int totalCount, string[] data)
    {
        var keyboardList = new List<InlineKeyboardButton[]>();
        foreach (var (item, i) in list.WithIndex())
        {
            if (i % 2 == 0)
                keyboardList.Add(new[]
                {
                    InlineKeyboardButton.WithCallbackData($"{item.Pack.Author} ({item.Count}{Text.items})",
                        $"{data[1]}={item.Id}")
                });
            else
                keyboardList[keyboardList.Count - 1] = new[]
                {
                    keyboardList[keyboardList.Count - 1][0],
                    InlineKeyboardButton.WithCallbackData($"{item.Pack.Author} ({item.Count}{Text.items})",
                        $"{data[1]}={item.Id}")
                };
        }

        var arrows = new List<InlineKeyboardButton>();
        if (offset > 9)
            arrows.Add(InlineKeyboardButton
                .WithCallbackData(Text.previous, $"{CallbackQueryCommands.choose_user_pack}={data[1]}" +
                                                 $"={offset - 10}{(data.Length == 4 ? '=' + data[3] : "")}"));
        arrows.Add(InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back));
        if (totalCount > offset + list.Count)
            arrows.Add(InlineKeyboardButton
                .WithCallbackData(Text.next,$"{CallbackQueryCommands.open_author_pack_menu}{data[1]}" +
                                            $"={offset + list.Count}{(data.Length == 4 ? '=' + data[3] : "")}"));
        keyboardList.Add(arrows.ToArray());
        return new InlineKeyboardMarkup(keyboardList);
    }
}