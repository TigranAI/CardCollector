using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Session;
using CardCollector.UserDailyTask;

namespace CardCollector.Database.Entity
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public long ChatId { get; set; }
        [MaxLength(256)] public string Username { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime BlockedAt { get; set; }
        public DateTime UnblockedAt { get; set; }
        public PrivilegeLevel PrivilegeLevel { get; set; }
        public bool FirstReward { get; set; }
        public virtual UserLevel Level { get; set; }
        public virtual Cash Cash { get; set; }
        public virtual UserSettings Settings { get; set; }
        public virtual UserMessages Messages { get; set; }
        public virtual InviteInfo? InviteInfo { get; set; }
        public virtual ICollection<UserSticker> Stickers { get; set; }
        public virtual ICollection<UserPacks> Packs { get; set; }
        public virtual ICollection<DailyTask> DailyTasks { get; set; }
        public virtual ICollection<SpecialOrderUser> SpecialOrdersUser { get; set; }
        public virtual ICollection<ChannelGiveaway> UsedGiveaways { get; set; }
        public virtual ICollection<TelegramChat> AvailableChats { get; set; }

        [NotMapped] public UserSession Session;
        [NotMapped] private bool _isNew = false;

        public bool HasAuctionDiscount()
        {
            return Stickers.Any(item => item.Sticker.Effect == Effect.AuctionDiscount5);
        }

        public async Task AddSticker(Sticker sticker, int count, bool sendAlert = false)
        {
            if (Stickers.SingleOrDefault(item => item.Sticker.Id == sticker.Id) is { } userSticker)
            {
                userSticker.Count += count;
            }
            else
            {
                var newUserSticker = new UserSticker()
                {
                    User = this,
                    Sticker = sticker,
                    Count = count
                };
                Stickers.Add(newUserSticker);
                await sticker.ApplyEffect(this, newUserSticker);
            }

            if (sendAlert)
            {
                await Messages.SendSticker(this, sticker.FileId);
                await Messages.SendMessage(this,
                    string.Format(Resources.Translations.Messages.you_got_sticker, sticker.ToString(count)));
            }
        }

        public void AddPack(Pack pack, int count)
        {
            if (Packs.FirstOrDefault(item => item.Pack.Id == pack.Id) is { } userPack)
                userPack.Count += count;
            else
                Packs.Add(new UserPacks(this, pack, count));
        }

        public User()
        {
            DailyTasks = new LinkedList<DailyTask>();
            Stickers = new LinkedList<UserSticker>();
            Packs = new LinkedList<UserPacks>();
            SpecialOrdersUser = new LinkedList<SpecialOrderUser>();
        }

        public User(Telegram.Bot.Types.User telegramUser) : this()
        {
            Username = telegramUser.Username ?? $"user{telegramUser.Id}";
            ChatId = telegramUser.Id;
            PrivilegeLevel = PrivilegeLevel.User;
            IsBlocked = telegramUser.IsBot;

            Level = new UserLevel();
            Cash = new Cash();
            Settings = new UserSettings();
            Messages = new UserMessages();
            DailyTasks.Add(new DailyTask()
            {
                User = this,
                TaskId = TaskKeys.SendStickersToUsers,
                Progress = TaskGoals.Goals[TaskKeys.SendStickersToUsers]
            });
        }

        public void InitSession()
        {
            var session = SessionController.FindSession(this);
            if (session != null) Session = session;
            else
            {
                Session = new UserSession();
                SessionController.AddSession(this);
            }
        }

        public bool IsNew()
        {
            return _isNew;
        }

        public User SetNew()
        {
            _isNew = true;
            return this;
        }

        public bool IsUnblocked()
        {
            return BlockedAt - UnblockedAt >= Constants.UNBLOCK_INVITE_INTERVAL;
        }

        public bool IsNotInvited()
        {
            return InviteInfo == null || InviteInfo.InvitedBy == null;
        }
    }
}