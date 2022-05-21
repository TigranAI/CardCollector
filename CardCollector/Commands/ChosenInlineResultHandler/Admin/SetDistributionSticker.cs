using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;

namespace CardCollector.Commands.ChosenInlineResultHandler.Admin
{
    public class SetDistributionSticker : ChosenInlineResultHandler
    {
        protected override string CommandText => ChosenInlineResultCommands.set_distribution_sticker;
        protected override async Task Execute()
        {
            User.Session.State = UserState.Default;
            var module = User.Session.GetModule<AdminModule>();

            var distribution = await Context.ChatDistributions.FindById(module.ChatDistributionId!.Value);
            var stickerId = long.Parse(ChosenInlineResult.ResultId.Split("=")[1]);
            var sticker = await Context.Stickers.FindById(stickerId);
            distribution.StickerFileId = sticker.FileId;
            
            await User.Messages.SendMessage(Messages.create_distribution_buttons,
                Keyboard.DistributionButtonsKeyboard);
        }
        
        public static async Task Skip(User user, BotDatabaseContext context)
        {
            user.Session.State = UserState.Default;
            await user.Messages.SendMessage(Messages.create_distribution_buttons,
                Keyboard.DistributionButtonsKeyboard);
        }
    }
}