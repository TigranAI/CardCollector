using System;
using System.Threading.Tasks;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.MessageHandler.UrlCommands
{
    public class Invite : MessageUrlHandler
    {
        protected override string CommandText => MessageUrlCommands.invite_friend;

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
        }
    }
}