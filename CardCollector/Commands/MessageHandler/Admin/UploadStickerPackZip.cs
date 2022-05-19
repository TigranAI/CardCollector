using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.Database.Entity;
using CardCollector.Extensions;
using CardCollector.Resources.Enums;
using CardCollector.Resources.Translations;
using OfficeOpenXml;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using File = Telegram.Bot.Types.File;
using Sticker = CardCollector.Database.Entity.Sticker;

namespace CardCollector.Commands.MessageHandler.Admin;

public class UploadStickerPackZip : MessageHandler
{
    public static readonly Dictionary<long, List<int>> StickerMessages = new ();

    protected override string CommandText => "";

    private const string
        EXCEL_TABLE_NAME = "/description.xlsx",
        STICKER_PACK_THUMB_NAME = "/thumb",
        STICKER_PACK_THUMB_GIF_NAME = "/thumb.gif",
        STICKER_FOLDER = "/stickers/",
        STICKER_WATERMARK_FOLDER = "/watermark_stickers/",
        STICKER_MONOCHROME_FOLDER = "/monochrome_stickers/",
        STICKER_EXCLUSIVE_KEY = "Exclusive",
        WEBP_EXTENSION = ".webp",
        TGS_EXTENSION = ".tgs";

    private const string
        FILE_NAME_COLUMN = "file",
        TITLE_COLUMN = "title",
        TIER_COLUMN = "tier",
        EFFECT_COLUMN = "effect",
        EXCLUSIVE_TASK_COLUMN = "exclusive_task",
        EXCLUSIVE_TASK_GOAL_COLUMN = "exclusive_task_goal",
        EMOJI_COLUMN = "emoji",
        DESCRIPTION_COLUMN = "description";


    protected override async Task Execute()
    {
        await User.Messages.ClearChat(User);
        await User.Messages.SendMessage(User, Messages.downloading_file);
        var fileName = await Utilities.DownloadFile(Message.Document!, User.ChatId);
        var dirName = Path.GetDirectoryName(fileName)!;

        await User.Messages.EditMessage(User, Messages.unzip_file);
        ZipFile.ExtractToDirectory(fileName, dirName, true);
        System.IO.File.Delete(fileName);

        await User.Messages.EditMessage(User, Messages.reading_document);
        var table = System.IO.File.Open(dirName + EXCEL_TABLE_NAME, FileMode.Open);
        var pack = ParseExcelFile(table);
        table.Close();

        await User.Messages.EditMessage(User, Messages.uploading_stickers);
        await UploadPackPreviews(dirName, pack);
        await UploadStickers(dirName, pack);

        await Context.Packs.AddAsync(pack);
        await User.Messages.EditMessage(User, Messages.stickers_succesfully_uploaded);
        
        TimerController.SetupTimer(60 * 1000, delegate { ClearChat(); });
    }

    private async void ClearChat()
    {
        if (!StickerMessages.ContainsKey(User.Id)) return;
        foreach (var messageId in StickerMessages[User.Id])
            await DeleteMessage(User.ChatId, messageId);
    }

    private async Task UploadStickers(string dirName, Pack pack)
    {
        foreach (var sticker in pack.Stickers)
        {
            sticker.Pack = pack;
            sticker.Author = pack.Author;
            var fileName = dirName + STICKER_FOLDER + sticker.Filename;
            sticker.SetSticker(await GetSticker(fileName));
            var watermarkFileName = dirName + STICKER_WATERMARK_FOLDER + sticker.Filename;
            sticker.SetWatermark(await GetSticker(watermarkFileName));
            if (pack.IsExclusive)
            {
                var monochromeFileName = dirName + STICKER_MONOCHROME_FOLDER + sticker.Filename;
                sticker.SetMonochrome(await GetSticker(monochromeFileName));
            }
        }
    }

    private async Task UploadPackPreviews(string dirName, Pack pack)
    {
        var thumbName = dirName + STICKER_PACK_THUMB_NAME;
        pack.SetPreview(await GetSticker(thumbName));

        var thumbGifName = dirName + STICKER_PACK_THUMB_GIF_NAME;
        if (System.IO.File.Exists(thumbGifName))
            pack.SetGifPreview(await GetFile(thumbGifName));
    }

    private Pack ParseExcelFile(FileStream table)
    {
        using (var xlpkg = new ExcelPackage(table))
        {
            var xl = xlpkg.Workbook.Worksheets.First().Cells!;
            var pack = ParsePackDescription(xl, 1);
            var columns = ParseColumnNames(xl, 2, pack.IsExclusive);
            pack.Stickers = ParseStickers(xl, columns, 3, pack.IsExclusive);
            return pack;
        }
    }

    private ICollection<Sticker> ParseStickers(ExcelRange xl, Dictionary<string, int> columns, int start,
        bool isExclusive)
    {
        var stickers = new List<Sticker>();
        for (var i = start; xl[i, 1].Value != null; ++i)
        {
            var sticker = new Sticker();
            sticker.Filename =
                xl[i, columns[FILE_NAME_COLUMN]].Value.ToString() ??
                throw new Exception(string.Format(ExceptionMessages.column_not_initialized, i, FILE_NAME_COLUMN));
            sticker.Title =
                xl[i, columns[TITLE_COLUMN]].Value.ToString() ??
                throw new Exception(string.Format(ExceptionMessages.column_not_initialized, i, TITLE_COLUMN));
            sticker.Tier =
                int.Parse(xl[i, columns[TIER_COLUMN]].Value.ToString() ??
                          throw new Exception(string.Format(ExceptionMessages.column_not_initialized, i, TIER_COLUMN)));
            sticker.Emoji =
                xl[i, columns[EMOJI_COLUMN]].Value.ToString() ??
                throw new Exception(string.Format(ExceptionMessages.column_not_initialized, i, EMOJI_COLUMN));

            if (columns[DESCRIPTION_COLUMN] > 0)
                sticker.Description = xl[i, columns[DESCRIPTION_COLUMN]].Value.ToString();

            if (isExclusive)
            {
                sticker.ExclusiveTask =
                    (ExclusiveTask) int.Parse(xl[i, columns[EXCLUSIVE_TASK_COLUMN]].Value.ToString()
                                              ?? throw new Exception(string.Format(
                                                  ExceptionMessages.column_not_initialized, i, EXCLUSIVE_TASK_COLUMN)));
                sticker.ExclusiveTaskGoal =
                    int.Parse(xl[i, columns[EXCLUSIVE_TASK_GOAL_COLUMN]].Value.ToString()
                              ?? throw new Exception(string.Format(ExceptionMessages.column_not_initialized, i,
                                  EXCLUSIVE_TASK_GOAL_COLUMN)));

                sticker.Income = 1;
                sticker.IncomeTime = 24 * 60;
                sticker.IncomeType = IncomeType.Candies;
            }
            else
            {
                sticker.Effect =
                    (Effect) int.Parse(xl[i, columns[EFFECT_COLUMN]].Value.ToString()
                                       ?? throw new Exception(string.Format(ExceptionMessages.column_not_initialized, i,
                                           EFFECT_COLUMN)));

                sticker.Income = (int) Math.Pow(5, sticker.Tier - 1);
                sticker.IncomeTime = 60;
                sticker.IncomeType = IncomeType.Coins;
            }
            
            stickers.Add(sticker);
        }

        return stickers;
    }

    private Dictionary<string, int> ParseColumnNames(ExcelRange xl, int row, bool isExclusive)
    {
        Dictionary<string, int> columns = new()
        {
            {FILE_NAME_COLUMN, -1},
            {TITLE_COLUMN, -1},
            {TIER_COLUMN, -1},
            {EFFECT_COLUMN, -1},
            {EXCLUSIVE_TASK_COLUMN, -1},
            {EMOJI_COLUMN, -1},
            {DESCRIPTION_COLUMN, -1},
            {EXCLUSIVE_TASK_GOAL_COLUMN, -1},
        };

        for (var i = 1; xl[row, i].Value is string s; ++i)
        {
            if (columns.ContainsKey(s)) columns[s] = i;
            else throw new Exception(string.Format(ExceptionMessages.incorrect_column, s));
        }

        if (columns[FILE_NAME_COLUMN] == -1)
            throw new Exception(string.Format(ExceptionMessages.column_not_found, FILE_NAME_COLUMN));
        if (columns[TITLE_COLUMN] == -1)
            throw new Exception(string.Format(ExceptionMessages.column_not_found, TITLE_COLUMN));
        if (columns[TIER_COLUMN] == -1)
            throw new Exception(string.Format(ExceptionMessages.column_not_found, TIER_COLUMN));
        if (columns[EMOJI_COLUMN] == -1)
            throw new Exception(string.Format(ExceptionMessages.column_not_found, EMOJI_COLUMN));
        if (columns[EFFECT_COLUMN] == -1 && !isExclusive)
            throw new Exception(string.Format(ExceptionMessages.column_not_found, EFFECT_COLUMN));
        if (columns[EXCLUSIVE_TASK_COLUMN] == -1 && isExclusive)
            throw new Exception(string.Format(ExceptionMessages.column_not_found, EXCLUSIVE_TASK_COLUMN));
        if (columns[EXCLUSIVE_TASK_GOAL_COLUMN] == -1 && isExclusive)
            throw new Exception(string.Format(ExceptionMessages.column_not_found, EXCLUSIVE_TASK_GOAL_COLUMN));

        return columns;
    }

    private InputFileStream FileOf(string fileName)
    {
        var file = System.IO.File.Open(fileName, FileMode.Open);
        var lastSlash = fileName.LastIndexOf("/", StringComparison.Ordinal);
        return new InputFileStream(file, fileName.Substring(lastSlash != -1 ? lastSlash : 0));
    }

    private async Task<File> GetFile(string fileName)
    {
        return await Bot.Client.UploadFileAsync(User.ChatId, FileOf(fileName));
    }

    private async Task<Telegram.Bot.Types.Sticker> GetSticker(string stickerName)
    {
        var file = FileOf(stickerName +
                          (System.IO.File.Exists(stickerName + WEBP_EXTENSION) ? WEBP_EXTENSION : TGS_EXTENSION));
        var message = await Bot.Client.SendStickerAsync(User.ChatId, new InputMedia(file.Content!, file.FileName));
        
        if (!StickerMessages.ContainsKey(User.Id)) StickerMessages.Add(User.Id, new List<int>());
        StickerMessages[User.Id].Add(message.MessageId);
        
        return message.Sticker!;
    }

    private Pack ParsePackDescription(ExcelRange xl, int row)
    {
        var pack = new Pack();
        pack.Author = xl[row, 1].Value.ToString() ?? throw new Exception(ExceptionMessages.author_not_found);
        pack.Description = xl[row, 2].Value.ToString() ?? "";
        pack.IsExclusive = xl[row, 3].Value.ToString() == STICKER_EXCLUSIVE_KEY;
        pack.PriceGems = pack.IsExclusive ? 500 : 100;
        pack.PriceCoins = -1;
        pack.IsPreviewAnimated = false;
        return pack;
    }

    public override bool Match()
    {
        return User.PrivilegeLevel >= PrivilegeLevel.Artist
               && User.Session.State is UserState.UploadStickerPackZip
               && Message.Type is MessageType.Document
               && Message.Document!.MimeType == "application/zip";
    }
}