using System;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Commands.CallbackQueryHandler;
using CardCollector.Database;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Extensions;
using CardCollector.Extensions.Database.Entity;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types.ReplyMarkups;

namespace CardCollector.Commands.MessageHandler.UrlCommands;

[Statistics]
public class Invite : MessageUrlHandler
{
    protected override string CommandText => MessageUrlCommands.invite_friend;

    private bool _isSuccess;

    protected override async Task Execute()
    {
        var isFirstOrderPicked = User.SpecialOrdersUser.Any(item => item.Id == 2);
        await User.Messages.SendStartMessage(isFirstOrderPicked);
        
        if (!User.IsNew() && !(User.IsUnblocked() && User.IsNotInvited()))
        {
            await User.Messages.SendMessage(Messages.you_cant_be_invited);
            return;
        }

        var inviteInfo = await Context.InviteInfo.FindByToken(StartData[1]);
        if (inviteInfo is null)
        {
            await User.Messages.SendMessage(Messages.link_unavailable);
            return;
        }

        _isSuccess = true;

        if (!User.FirstReward)
        {
            var packInfo = await Context.Packs.FindById(1);
            User.FirstReward = true;
            User.AddPack(packInfo, 7);
            await User.Messages.SendSticker(packInfo.PreviewFileId!, OpenStartPacks());
            if (!User.FirstReward)
            {
                User.FirstReward = true;
                User.AddPack(packInfo, 7);
                await User.Messages.SendSticker(packInfo.PreviewFileId!, OpenStartPacks());
            }
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

        await inviteInfo.User.Messages.SendMessage(
            string.Format(Messages.invite_accepted, User.Username));
        await User.Messages.SendMessage(
            string.Format(Messages.invite_welcome_message, inviteInfo.User.Username));

        if (inviteInfo.User.InviteInfo?.TasksProgress is { } tp && !tp.InviteFriend)
        {
            tp.InviteFriend = true;
            await inviteInfo.User.InviteInfo.CheckRewards(Context);
        }

        await inviteInfo.User.Stickers
            .Where(sticker => sticker.Sticker.ExclusiveTask is ExclusiveTask.InviteFriends)
            .ApplyAsync(sticker => sticker.DoExclusiveTask());
        inviteInfo.User.UserStats.IncreaseFriendsInvited();
    }

    protected override async Task AfterExecute()
    {
        await DeleteMessage(User.ChatId, Message.MessageId);
        await base.AfterExecute();
    }

    private InlineKeyboardMarkup OpenStartPacks()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new[] {InlineKeyboardButton.WithCallbackData(Text.open_start_packs, CallbackQueryCommands.open_pack)}
        });
    }

    protected override async Task SaveActivity(BotDatabaseContext context)
    {
        if (_isSuccess) await base.SaveActivity(context);
    }
}