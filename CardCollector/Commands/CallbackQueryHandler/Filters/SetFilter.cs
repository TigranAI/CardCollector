using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.Commands.CallbackQueryHandler.Others;
using CardCollector.Commands.MessageHandler.Menu;
using CardCollector.Database;
using CardCollector.Resources.Enums;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Filters
{
    [DontAddToCommandStack]
    public class SetFilter : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.set;

        protected override async Task Execute()
        {
            EnterEmoji.RemoveFromQueue(User.Id);
            var data = CallbackQuery.Data!.Split('=');
            var filters = User.Session.GetModule<FiltersModule>();
            var key = (FilterKeys) int.Parse(data[1]);
            var value = data[2] == "" ? null : data[2];
            switch (key)
            {
                case FilterKeys.Author:
                    filters.Author = value;
                    break;
                case FilterKeys.Emoji:
                    filters.Emoji = value;
                    break;
                case FilterKeys.Sorting:
                    filters.Sorting = (SortingTypes) int.Parse(value!);
                    break;
                case FilterKeys.Tier:
                    filters.Tier = value == null 
                        ? null
                        : int.Parse(value);
                    break;
                case FilterKeys.PriceCoinsFrom:
                    filters.PriceCoinsFrom = value == null 
                        ? null
                        : int.Parse(value);
                    if (filters.PriceCoinsFrom > filters.PriceCoinsTo) filters.PriceCoinsTo = null;
                    break;
                case FilterKeys.PriceCoinsTo:
                    filters.PriceCoinsTo = value == null 
                        ? null
                        : int.Parse(value);
                    if (filters.PriceCoinsFrom > filters.PriceCoinsTo) filters.PriceCoinsFrom = null;
                    break;
                case FilterKeys.PriceGemsFrom:
                    filters.PriceGemsFrom = value == null 
                        ? null
                        : int.Parse(value);
                    if (filters.PriceGemsFrom > filters.PriceGemsTo) filters.PriceGemsTo = null;
                    break;
                case FilterKeys.PriceGemsTo:
                    filters.PriceGemsTo = value == null 
                        ? null
                        : int.Parse(value);
                    if (filters.PriceGemsFrom > filters.PriceGemsTo) filters.PriceGemsFrom = null;
                    break;
            }
            await new Back(User, Context, CallbackQuery).PrepareAndExecute();
        }

        public SetFilter(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery)
        {
        }
    }
}