using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Others;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Commands.CallbackQueryHandler.Others
{
    [MenuPoint]
    public class ChoosePack : CallbackQueryHandler
    {
        /* Command syntax select_pack=<target command>=<offset>=<optional exclusive or not> */
        protected override string CommandText => CallbackQueryCommands.choose_pack;
        protected override bool ClearStickers => true;

        protected override async Task Execute()
        {
            var data = CallbackQuery.Data!.Split('=');
            var offset = int.Parse(data[2]);
            var packs = data.Length == 4
                ? await Context.Packs.FindNextSkipRandom(offset, 10, data[3] == "1")
                : await Context.Packs.FindNextSkipRandom(offset, 10);
            if (packs.Count == 0)
                await AnswerCallbackQuery(User, CallbackQuery.Id, Messages.page_not_found);
            else
                await User.Messages.EditMessage(Messages.choose_author,
                    Keyboard(packs, offset, await Context.Packs.GetCount(), data));
        }

        private InlineKeyboardMarkup Keyboard(List<Pack> list, int offset, int totalCount, string[] data)
        {
            var keyboardList = new List<InlineKeyboardButton[]>();
            foreach (var (item, i) in list.WithIndex())
            {
                if (i % 2 == 0)
                    keyboardList.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData(item.Author, $"{data[1]}={item.Id}")
                    });
                else
                    keyboardList[keyboardList.Count - 1] = new[]
                    {
                        keyboardList[keyboardList.Count - 1][0],
                        InlineKeyboardButton.WithCallbackData(item.Author, $"{data[1]}={item.Id}")
                    };
            }

            var arrows = new List<InlineKeyboardButton>();
            if (offset > 9)
                arrows.Add(InlineKeyboardButton
                    .WithCallbackData(Text.previous,$"{CallbackQueryCommands.choose_pack}={data[1]}" +
                                                    $"={offset - 10}{(data.Length == 4 ? '=' + data[3] : "")}"));
            arrows.Add(InlineKeyboardButton.WithCallbackData(Text.back, CallbackQueryCommands.back));
            if (totalCount > offset + list.Count)
                arrows.Add(InlineKeyboardButton
                    .WithCallbackData(Text.next,$"{CallbackQueryCommands.choose_pack}={data[1]}" +
                                                $"={offset + list.Count}{(data.Length == 4 ? '=' + data[3] : "")}"));
            keyboardList.Add(arrows.ToArray());
            return new InlineKeyboardMarkup(keyboardList);
        }
    }
}