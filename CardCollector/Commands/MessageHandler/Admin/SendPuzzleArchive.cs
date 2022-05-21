using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.Database.EntityDao;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using CardCollector.Session.Modules;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using File = System.IO.File;

namespace CardCollector.Commands.MessageHandler.Admin;

public class SendPuzzleArchive : MessageHandler
{
    public static readonly Dictionary<long, List<int>> StickerMessages = new ();
    
    private const string
        WEBP_EXTENSION = ".webp",
        TGS_EXTENSION = ".tgs";
    
    protected override string CommandText => "";
    protected override async Task Execute()
    {
        await User.Messages.ClearChat();
        await User.Messages.SendMessage(Messages.downloading_file);
        var fileName = await Utilities.DownloadFile(Message.Document!, User.ChatId);
        var dirName = Path.GetDirectoryName(fileName)!;

        await User.Messages.EditMessage(Messages.unzip_file);
        ZipFile.ExtractToDirectory(fileName, dirName, true);
        File.Delete(fileName);
        
        await User.Messages.EditMessage(Messages.uploading_stickers);
        await UploadStickers(dirName);
        
        await User.Messages.EditMessage(Messages.update_sticker_success, Keyboard.BackKeyboard);
        TimerController.SetupTimer(30 * 1000, delegate { ClearChat(); });
        User.Session.State = UserState.Default;
    }

    private async Task UploadStickers(string dirName)
    {
        var sticker = await Context.Stickers.FindById(User.Session.GetModule<AdminModule>().SelectedStickerId!.Value);
        var result = new List<Sticker>();
        for (var i = 0; i < 5; ++i)
        {
            var webpSticker = $"{dirName}/{i}{WEBP_EXTENSION}";
            var tgsSticker = $"{dirName}/{i}{TGS_EXTENSION}";
            if (File.Exists(webpSticker))
                result.Add(await GetSticker(webpSticker));
            else if (File.Exists(tgsSticker))
                result.Add(await GetSticker(tgsSticker));
            else break;
        }
        Directory.Delete(dirName, true);
        sticker.AddPuzzlePieces(result);
    }

    private async void ClearChat()
    {
        if (!StickerMessages.ContainsKey(User.Id)) return;
        foreach (var messageId in StickerMessages[User.Id])
            await DeleteMessage(User.ChatId, messageId);
    }
    
    private InputFileStream FileOf(string fileName)
    {
        var file = File.Open(fileName, FileMode.Open);
        var lastSlash = fileName.LastIndexOf("/", StringComparison.Ordinal);
        return new InputFileStream(file, fileName.Substring(lastSlash != -1 ? lastSlash : 0));
    }
    
    private async Task<Sticker> GetSticker(string stickerName)
    {
        var file = FileOf(stickerName);
        var message = await Bot.Client.SendStickerAsync(User.ChatId, new InputMedia(file.Content!, file.FileName!));
        
        if (!StickerMessages.ContainsKey(User.Id)) StickerMessages.Add(User.Id, new List<int>());
        StickerMessages[User.Id].Add(message.MessageId);
        
        return message.Sticker!;
    }
    
    public override bool Match()
    {
        return User.PrivilegeLevel >= PrivilegeLevel.Artist
               && User.Session.State is UserState.SendPuzzleArchive
               && Message.Type is MessageType.Document
               && Message.Document!.MimeType == "application/zip";
    }
}