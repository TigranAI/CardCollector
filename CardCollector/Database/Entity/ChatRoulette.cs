using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using CardCollector.Database.EntityDao;
using CardCollector.Extensions;
using CardCollector.Extensions.Database.Entity;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
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
            await context.SaveChangesAsync();
            if (!Bets.Any(item => item.User.Id == Creator.Id))
            {
                await Group.DeleteMessage(MessageId);
                await Group.SendMessage(string.Format(Messages.creator_didnt_bet, Creator.Username));
                ReturnBets();
            }
            else if (Bets.Count < Constants.ROULETTE_MIN_PLAYERS)
            {
                await Group.DeleteMessage(MessageId);
                await Group.SendMessage(Messages.too_few_players);
                ReturnBets();
            }
            else
            {
                var stickerWinner = Bets.WeightedRandom(item => (int) Math.Pow(5, item.Sticker.Tier - 1));
                if (stickerWinner == null)
                {
                    Logs.LogOutError("Cant define bet winner. RouletteId: " + Id);
                    ReturnBets();
                }
                else
                {
                    if (stickerWinner.User.InviteInfo?.TasksProgress is { } wtp 
                        && wtp.WinRoulette < BeginnersTasksProgress.WinRouletteGoal)
                    {
                        wtp.WinRoulette++;
                        await stickerWinner.User.InviteInfo.CheckRewards(context);
                    }
                    
                    var chance = Math.Pow(5, stickerWinner.Sticker.Tier - 1) /
                        Bets.Sum(item => Math.Pow(5, item.Sticker.Tier - 1)) * 100;
                    await Group.DeleteMessage(MessageId);
                    await Group.SendDice(Emoji.SlotMachine);
                    await Group.SendMessage(string.Format(Messages.congratulation_to_roulette_winner,
                            stickerWinner.User.Username, Math.Round(chance, 2),
                            string.Join("\n", Bets
                                .Where(item => item.User.Id != stickerWinner.Id)
                                .Select(item => $"{item.Sticker.Title} {item.Sticker.TierAsStars()}"))
                        )
                    );
                    foreach (var bet in Bets)
                    {
                        bet.User.UserStats.IncreaseRouletteGames();
                        
                        await stickerWinner.User.AddSticker(bet.Sticker, 1);
                        
                        if (bet.User.InviteInfo?.TasksProgress is { } tp 
                            && tp.PlayRoulette < BeginnersTasksProgress.PlayRouletteGoal)
                        {
                            tp.PlayRoulette++;
                            await bet.User.InviteInfo.CheckRewards(context);
                        }
                    }
                    
                    await stickerWinner.User.Stickers
                        .Where(sticker => sticker.Sticker.ExclusiveTask is ExclusiveTask.WinRoulette)
                        .ApplyAsync(async sticker => await sticker.DoExclusiveTask());
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
            MessageId = await Group.EditMessage(
                string.Format(Messages.roulette_message, Creator.Username, BetsToMessage()),
                MessageId, Keyboard.RouletteKeyboard(Id));
            return true;
        }

        private string BetsToMessage()
        {
            var pool = Bets.Sum(item => Math.Pow(5, item.Sticker.Tier - 1));
            return string.Join("\n", Bets.OrderByDescending(item => item.Sticker.Tier).Select((item, i) =>
            {
                var chance = Math.Pow(5, item.Sticker.Tier - 1) / pool * 100;
                return $"{i+1}. {item.User.Username}: {item.Sticker.Title} ({Math.Round(chance, 2)}%)";
            }));
        }
    }
}