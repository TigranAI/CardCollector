using System;
using System.Threading.Tasks;
using CardCollector.Attributes.Logs;
using CardCollector.Controllers;
using CardCollector.Database;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.Enums;

namespace CardCollector.Commands.MessageHandler.UrlCommands
{
    [SavedActivity]
    public class Invite : MessageUrlHandler
    {
        protected override string CommandText => MessageUrlCommands.invite_friend;

        private bool _isSuccess;

        protected override async Task Execute()
        {
            if (!(User.IsNew() || User.IsUnblocked() && User.IsNotInvited()))
            {
                await User.Messages.SendMessage(User, Messages.you_cant_be_invited);
                return;
            }
            
            var inviteInfo = await Context.InviteInfo.FindByToken(StartData[1]);
            if (inviteInfo is null)
            {
                await User.Messages.SendMessage(User, Messages.link_unavailable);
                return;
            }

            _isSuccess = true;
            
            if (!User.FirstReward)
            {
                User.FirstReward = true;
                var packInfo = await Context.Packs.FindById(1);
                User.AddPack(packInfo, 7);
                await User.Messages.SendSticker(User, packInfo.PreviewFileId!);
                await User.Messages.SendMessage(User, Messages.first_reward, Keyboard.MyPacks, ParseMode.Html);
            }
            await User.Messages.SendMessage(User, Messages.start_message, Keyboard.Menu);
            
            inviteInfo.InvitedFriends.Add(User);
            User.InviteInfo = new InviteInfo()
            {
                InvitedBy = inviteInfo.User,
                InvitedAt = DateTime.Now,
                InviteKey = await InviteInfo.GenerateKey(),
                TasksProgress = new BeginnersTasksProgress()
            };

            var pack = await Context.Packs.FindById(1);
            inviteInfo.User.AddPack(pack, 1);
            User.AddPack(pack, 2);

            await inviteInfo.User.Messages.SendMessage(inviteInfo.User,
                string.Format(Messages.invite_accepted, User.Username));
            await User.Messages.SendMessage(User, 
                string.Format(Messages.invite_welcome_message, inviteInfo.User.Username));
            
            if (inviteInfo.User.InviteInfo?.TasksProgress is { } tp && !tp.InviteFriend)
            {
                tp.InviteFriend = true;
                await inviteInfo.User.InviteInfo.CheckRewards(Context);
            }
        }
        
        protected override async Task AfterExecute()
        {
            await MessageController.DeleteMessage(User.ChatId, Message.MessageId);
            await base.AfterExecute();
        }

        protected override async Task SaveActivity(BotDatabaseContext context)
        {
            if (_isSuccess) await base.SaveActivity(context);
        }
    }
}