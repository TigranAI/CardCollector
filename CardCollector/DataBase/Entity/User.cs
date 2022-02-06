
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CardCollector.DailyTasks;
using CardCollector.DailyTasks.CustomTasks;
using CardCollector.Resources;
using CardCollector.Session;

namespace CardCollector.DataBase.Entity
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public long ChatId { get; set; }
        [MaxLength(256)] public string Username { get; set; }
        public bool IsBlocked { get; set; }
        public PrivilegeLevel PrivilegeLevel { get; set; }
        public bool FirstReward { get; set; } 
        public UserLevel Level { get; set; }
        public Cash Cash { get; set; }
        public UserSettings Settings { get; set; }
        public UserMessages Messages { get; set; }
        public ICollection<DailyTask> DailyTasks { get; set; }
        public ICollection<UserSticker> Stickers { get; set; }
        public ICollection<UserPacks> Packs { get; set; }
        public ICollection<SpecialOrderUser> SpecialOrdersUser { get; set; }

        [NotMapped] public UserSession Session;
        

        /*public async Task GiveExp(long count)
        {
            CurrentExp += count;
            TotalExp += count;
            var levelInfo = await LevelDao.GetLevel(CurrentLevel.Level + 1);
            if (levelInfo?.LevelExpGoal <= CurrentLevel.CurrentExp) await ClearChat();
            while (levelInfo?.LevelExpGoal <= CurrentLevel.CurrentExp)
            {
                CurrentLevel.CurrentExp -= levelInfo.LevelExpGoal;
                CurrentLevel.Level++;
                var levelReward = levelInfo.GetRewardInstance();
                var message = $"{Messages.congratulation_new_level} {CurrentLevel.Level}" +
                              $"\n{await levelReward.GetReward(this)}";
                await MessageController.SendMessage(this, message, levelReward.RandomPacks > 0 ? Keyboard.MyPacks : null);
                levelInfo = await LevelDao.GetLevel(CurrentLevel.Level + 1);
            }
        }*/

        public User()
        {
            Session = new UserSession(this);
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
            
            Level = new UserLevel();
            Cash = new Cash();
            Settings = new UserSettings();
            Messages = new UserMessages();
            DailyTasks.Add(new DailyTask(){User = this, TaskId = DailyTaskKeys.SendStickersToUsers, Progress = 5});
        }
    }
}