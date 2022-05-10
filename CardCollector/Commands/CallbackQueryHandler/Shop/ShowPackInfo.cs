using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.Database.EntityDao;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.CallbackQueryHandler.Shop
{
    public class ShowPackInfo : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.show_order_info;

        protected override async Task Execute()
        {
            var module = User.Session.GetModule<ShopModule>();
            var packId = module.SelectedPackId;
            var pack = await Context.Packs.FindById(packId);
            var message = $"{Messages.author_pack}: {pack.Author}" +
                          $"\n{Text.opened_count} {pack.OpenedCount}{Text.items}";
            if (packId != 1 && (User.PrivilegeLevel is PrivilegeLevel.Artist ||
                                User.PrivilegeLevel >= PrivilegeLevel.Programmer))
            {
                var roubles = pack.PriceGems / 5 * pack.OpenedCount;
                message += $"\n{Text.in_roubles}: {roubles}₽";
            }

            if (pack.Description != null) message += $"\n{Text.description}: {pack.Description}";
            await MessageController.AnswerCallbackQuery(User, CallbackQuery.Id, message, true);
        }

        public override bool Match()
        {
            return base.Match() && User.Session.GetModule<ShopModule>().SelectedPackId != null;
        }
    }
}