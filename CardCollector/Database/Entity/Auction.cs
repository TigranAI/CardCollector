using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CardCollector.Commands.ChosenInlineResultHandler;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.InlineQueryResults;

namespace CardCollector.Database.Entity
{
    public class Auction
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public virtual User Trader { get; set; }
        public virtual Sticker Sticker { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }

        public InlineQueryResultArticle AsTelegramArticle(double discount)
        {
            return new InlineQueryResultArticle($"{ChosenInlineResultCommands.select_trader}={Id}",
                $"{Trader.Username} {Count}{Text.items}", new InputTextMessageContent(Text.buy))
            {
                Description = $"{(int) (discount * Price)}{Text.gem} {Text.per} 1{Text.items}"
            };
        }
    }
}