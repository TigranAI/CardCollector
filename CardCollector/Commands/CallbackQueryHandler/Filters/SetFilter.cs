using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.Commands.CallbackQueryHandler.Others;
using CardCollector.Commands.MessageHandler.Menu;
using CardCollector.DataBase;
using CardCollector.Session.Modules;
using Telegram.Bot.Types;
using User = CardCollector.DataBase.Entity.User;

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
            var key = (FiltersModule.FilterKeys) int.Parse(data[1]);
            var value = data[2] == "" ? null : data[2];
            switch (key)
            {
                case FiltersModule.FilterKeys.Author:
                    filters.Author = value;
                    break;
                case FiltersModule.FilterKeys.Emoji:
                    filters.Emoji = value;
                    break;
                case FiltersModule.FilterKeys.Sorting:
                    filters.Sorting = (FiltersModule.SortingTypes) int.Parse(value!);
                    break;
                case FiltersModule.FilterKeys.Tier:
                    filters.Tier = value == null 
                        ? null
                        : int.Parse(value);
                    break;
                case FiltersModule.FilterKeys.PriceCoinsFrom:
                    filters.PriceCoinsFrom = value == null 
                        ? null
                        : int.Parse(value);
                    if (filters.PriceCoinsFrom > filters.PriceCoinsTo) filters.PriceCoinsTo = null;
                    break;
                case FiltersModule.FilterKeys.PriceCoinsTo:
                    filters.PriceCoinsTo = value == null 
                        ? null
                        : int.Parse(value);
                    if (filters.PriceCoinsFrom > filters.PriceCoinsTo) filters.PriceCoinsFrom = null;
                    break;
                case FiltersModule.FilterKeys.PriceGemsFrom:
                    filters.PriceGemsFrom = value == null 
                        ? null
                        : int.Parse(value);
                    if (filters.PriceGemsFrom > filters.PriceGemsTo) filters.PriceGemsTo = null;
                    break;
                case FiltersModule.FilterKeys.PriceGemsTo:
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