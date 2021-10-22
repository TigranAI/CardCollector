using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using CardCollector.Controllers;
using CardCollector.Resources;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using OfficeOpenXml;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using File = System.IO.File;

namespace CardCollector.Commands.Message.DocumentMessage
{
    public class UploadFileMessageCommand : MessageCommand
    {
        protected override string CommandText => "";
        public override async Task Execute()
        {
            /* Очищаем чат */
            await User.ClearChat();
            User.Session.State = UserState.Default;
            /* Соообщаем, что начали загрузку файла */
            var message = await MessageController.SendMessage(User, Messages.downloading_file);
            /* Загружаем файл */
            await Utilities.DownloadFile(Update.Message!.Document!.FileId);
            /* Сообщаем пользователю, что начали распаковку */
            await MessageController.EditMessage(User, message.MessageId, Messages.unzipping_file);
            /* Извлекаем из архива файлы */
            await Task.Run(() => ZipFile.ExtractToDirectory("pack.zip", "pack", true));
            /* Сообщаем пользователю, что читаем документ */
            await MessageController.EditMessage(User, message.MessageId, Messages.reading_document);
            /* Парсим файл */
            try
            {
                await ParseExcelFile();
                /* Сообщаем пользователю, что удаляем данные */
                await MessageController.EditMessage(User, message.MessageId, Messages.deleting_files);
                File.Delete("pack.zip");
                Directory.Delete("pack", true);
                /* Сообщаем пользователю, что список стикеров обновится через 15 минут */
                await MessageController.EditMessage(User, message.MessageId, Messages.stickers_will_be_updated);
            }
            catch (Exception)
            {
                /* Сообщаем пользователю, что произошла ошибка */
                await MessageController.EditMessage(User, message.MessageId, Messages.unexpected_exception);
            }
            var timer = new Timer
            {
                Interval = 60 * 1000,
                Enabled = true,
                AutoReset = false
            };

            async void DeleteMessage(object sender, ElapsedEventArgs args) => await MessageController.DeleteMessage(User, message.MessageId);

            timer.Elapsed += DeleteMessage;
        }
        
        private async Task ParseExcelFile()
        {
            await Task.Run(async () =>
            {
                using var xlPackage = new ExcelPackage(new FileInfo("pack/table.xlsx"));
                var myWorksheet = xlPackage.Workbook.Worksheets.First(); //select sheet here
                var dirInfo = new DirectoryInfo("pack");
                var newStickers = new List<StickerEntity>();
                var firstStickerName = myWorksheet.Cells[2, 1].Value.ToString();
                var firstStickerInfo = dirInfo.GetFiles($@"{firstStickerName}.*")[0];
                var isAnimated = firstStickerInfo.Extension == ".tgs";
                for (var rowNum = 2; myWorksheet.Cells[rowNum, 1].Value is string; rowNum++) //select starting row here
                {
                    var stickerName = myWorksheet.Cells[rowNum, 1].Value.ToString();
                    var fileInfo = dirInfo.GetFiles($@"{stickerName}.*")[0];
                    var fields = ParseRow(myWorksheet.Cells, rowNum);
                    await CreateSticker(fileInfo, fields["Emoji"]);
                    var sticker = new StickerEntity
                    {
                        Title = fields["Title"], Author = fields["Author"], Income = int.Parse(fields["IncomeCoins"]), 
                        IncomeTime = int.Parse(fields["IncomeTime"]), Tier = int.Parse(fields["Tier"]), 
                        Emoji = fields["Emoji"], Description = fields["Description"]
                    };
                    newStickers.Add(sticker);
                }

                var timer = new Timer
                {
                    Interval = 15 * 60 * 1000,
                    Enabled = true,
                    AutoReset = false,
                };

                async void SavingStickersToDatabase(object sender, ElapsedEventArgs args)
                {
                    var stickerSet = (await Bot.Client.GetStickerSetAsync(
                        $"{(isAnimated ? "a" : "s")}{User.Id}_by_{AppSettings.NAME}")).Stickers.ToList();
                    Logs.LogOut("Saving " + newStickers.Count + " stickers from " + stickerSet.Count);
                    for (var i = 0; i < newStickers.Count; i++)
                    {
                        var stickerId = stickerSet[i].FileId;
                        newStickers[i].Id = stickerId;
                        newStickers[i].Md5Hash = Utilities.CreateMd5(stickerId);
                        await StickerDao.AddNew(newStickers[i]);
                    }
                    foreach (var sticker in newStickers)
                    {
                        try
                        {
                            await Bot.Client.DeleteStickerFromSetAsync(sticker.Id);
                        }
                        catch (Exception)
                        {
                            Logs.LogOut("Cant delete sticker " + sticker.Title);
                        }
                    }
                }
                
                timer.Elapsed += SavingStickersToDatabase;
            });
        }

        private static Dictionary<string, string> ParseRow(ExcelRange cells, int rowNum)
        {
            return new Dictionary<string, string>
            {
                {"Title", cells[rowNum, 2].Value.ToString()},
                {"Author", cells[rowNum, 3].Value.ToString()},
                {"IncomeCoins", cells[rowNum, 4].Value.ToString()},
                {"IncomeGems", cells[rowNum, 5].Value.ToString()},
                {"IncomeTime", cells[rowNum, 6].Value.ToString()},
                {"PriceCoins", cells[rowNum, 7].Value.ToString()},
                {"PriceGems", cells[rowNum, 8].Value.ToString()},
                {"Tier", cells[rowNum, 9].Value.ToString()},
                {"Emoji", cells[rowNum, 10].Value.ToString()},
                {"Description", cells[rowNum, 11].Value is string s ? s : ""},
            };
        }

        private async Task CreateSticker(FileInfo fileInfo, string emoji)
        {
            var isAnimated = fileInfo.Extension == ".tgs";
            var fileStream = fileInfo.OpenRead();
            try
            {
                if (isAnimated)
                    await Bot.Client.AddAnimatedStickerToSetAsync(User.Id, $"a{User.Id}_by_{AppSettings.NAME}",
                    new InputFileStream(fileStream), emoji);
                else
                {
                    var onlineFile = await Bot.Client.UploadStickerFileAsync(User.Id, fileInfo.OpenRead());
                    await Bot.Client.AddStickerToSetAsync(User.Id, $"s{User.Id}_by_{AppSettings.NAME}",
                        new InputMedia(onlineFile.FileId), emoji);
                }
            }
            catch (Exception)
            {
                fileStream.Close();
                fileStream = fileInfo.OpenRead();
                if (isAnimated) 
                    await Bot.Client.CreateNewAnimatedStickerSetAsync(User.Id, $"a{User.Id}_by_{AppSettings.NAME}",
                    "animated", new InputFileStream(fileStream), emoji);
                else
                {
                    var onlineFile = await Bot.Client.UploadStickerFileAsync(User.Id, fileInfo.OpenRead());
                    await Bot.Client.CreateNewStickerSetAsync(User.Id, $"s{User.Id}_by_{AppSettings.NAME}",
                        "static", new InputMedia(onlineFile.FileId), emoji);
                }
            }
            fileStream.Close();
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return user.Session.State == UserState.UploadFile;
        }

        public UploadFileMessageCommand() { }
        public UploadFileMessageCommand(UserEntity user, Update update) : base(user, update) { }
    }
}