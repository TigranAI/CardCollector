using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using CardCollector.Database.EntityDao;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Database.Entity
{
    public class ChatRoulette
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;
        public virtual User Creator { get; set; }
        public virtual TelegramChat Group { get; set; }
        public int MessageId { get; set; }
        public virtual ICollection<UserSticker> Bets { get; set; }
        public bool IsStarted { get; set; }

        public async void Start(object? o, ElapsedEventArgs eventArgs)
        {
            using (var context = new BotDatabaseContext())
            {
                var roulette = await context.ChatRoulette.FindById(Id);
                if (roulette == null) return;
                await roulette.Start(context);
                await context.SaveChangesAsync();
            }
        }

        public async Task Start(BotDatabaseContext context)
        {
            if (IsStarted) return;
            IsStarted = true;
            Logs.LogOut($"roulette {Id} is started");
            await context.SaveChangesAsync();
            if (!Bets.Any(item => item.User.Id == Creator.Id))
            {
                await Group.EditMessage(string.Format(Messages.creator_didnt_bet, Creator.Username), MessageId);
                ReturnBets();
            }
            else if (Bets.Count < Constants.ROULETTE_MIN_PLAYERS)
            {
                await Group.EditMessage(Messages.too_few_players, MessageId);
                ReturnBets();
            }
            else
            {
                var winner = Bets.WeightedRandom(item => (int) Math.Pow(5, item.Sticker.Tier - 1));
                if (winner == null)
                {
                    Logs.LogOutError("Cant define bet winner. RouletteId: " + Id);
                    ReturnBets();
                }
                else
                {
                    var chance = Math.Pow(5, winner.Sticker.Tier - 1) /
                        Bets.Sum(item => Math.Pow(5, item.Sticker.Tier - 1)) * 100;
                    await Group.DeleteMessage(MessageId);
                    await Group.SendDice(Emoji.SlotMachine);
                    await Group.SendMessage(string.Format(Messages.congratulation_to_roulette_winner,
                            winner.User.Username, Math.Round(chance, 2),
                            string.Join("\n", Bets.Select(item => $"{item.Sticker.Title} {item.Sticker.TierAsStars()}"))
                        )
                    );
                    foreach (var bet in Bets)
                        await winner.User.AddSticker(bet.Sticker, 1);
                }
            }
        }

        private void ReturnBets()
        {
            foreach (var bet in Bets)
            {
                bet.Count++;
            }
        }

        public async Task<bool> MadeABet(UserSticker userSticker)
        {
            if (IsStarted) return false;
            if (Bets.SingleOrDefault(item => item.User.Id == userSticker.User.Id) is { } previousBet)
            {
                previousBet.Count++;
                Bets.Remove(previousBet);
            }
            userSticker.Count--;
            Bets.Add(userSticker);
            await Group.EditMessage(string.Format(Messages.roulette_message, Creator.Username, BetsToMessage()),
                MessageId, Keyboard.RouletteKeyboard(Id));
            return true;
        }

        private string BetsToMessage()
        {
            var pool = Bets.Sum(item => Math.Pow(5, item.Sticker.Tier - 1));
            return string.Join("\n", Bets.Select((item, i) =>
            {
                var chance = Math.Pow(5, item.Sticker.Tier - 1) / pool * 100;
                return $"{i+1}. {item.User.Username}: {item.Sticker.Title} ({Math.Round(chance, 2)}%)";
            }));
        }
    }
}