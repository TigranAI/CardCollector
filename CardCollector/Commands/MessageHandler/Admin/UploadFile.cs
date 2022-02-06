using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.DataBase;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using CardCollector.StickerEffects;
using OfficeOpenXml;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using File = System.IO.File;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands.MessageHandler.Admin
{
    public class UploadFile : MessageHandler
    {
        protected override string CommandText => "";

        protected override async Task Execute()
        {
            var module = User.Session.GetModule<AdminModule>();
            try
            {
                /* Соообщаем, что начали загрузку файла */
                await User.Messages.EditMessage(User, Messages.downloading_file);
                
                /* Загружаем файл */
                var fileName = await Utilities.DownloadFile(Message.Document!);
                
                /* Сообщаем пользователю, что читаем документ */
                await User.Messages.EditMessage(User, Messages.reading_document);
                
                /* Парсим файл */
                var stickersList = await ParseExcelFile(fileName, module.StickersList);
                Utilities.ReplaceOldEmoji(stickersList);
                
                /* Сообщаем пользователю, что удаляем файлы */
                await User.Messages.EditMessage(User, Messages.deleting_files);
                File.Delete(fileName);
                
                /* Сообщаем пользователю, что загружаем стикеры */
                await User.Messages.EditMessage(User, Messages.uploading_stickers);
                var newPack = new Pack(){Author = stickersList.First().Author};
                foreach (var sticker in stickersList)
                {
                    newPack.Stickers.Add(sticker);
                    sticker.Pack = newPack;
                }
                
                /* Сообщаем пользователю, что стикеры загружены */
                await User.Messages.EditMessage(User, Messages.stickers_succesfully_uploaded);
            }
            catch (Exception e)
            {
                /* Сообщаем пользователю, что произошла ошибка */
                await User.Messages.EditMessage(User, $"{Messages.unexpected_exception}: {e.Message}");
            }
        }
        
        private async Task<List<DataBase.Entity.Sticker>> ParseExcelFile(string fileName, List<DataBase.Entity.Sticker> stickers)
        {
            return await Task.Run(() =>
            {
                using var xlPackage = new ExcelPackage(new FileInfo(fileName));
                var myWorksheet = xlPackage.Workbook.Worksheets.First();
                for (var rowNum = 2; rowNum < stickers.Count + 2; rowNum++)
                {
                    var fields = ParseRow(myWorksheet.Cells, rowNum);
                    stickers[rowNum - 2].Title = fields["Title"];
                    stickers[rowNum - 2].Author = fields["Author"];
                    stickers[rowNum - 2].Tier = int.Parse(fields["Tier"]);
                    stickers[rowNum - 2].Emoji = fields["Emoji"];
                    stickers[rowNum - 2].Effect = (Effect) int.Parse(fields["Effect"]);
                    stickers[rowNum - 2].Description = fields["Description"];
                    stickers[rowNum - 2].IncomeTime = 60;
                    stickers[rowNum - 2].Income = (int) Math.Pow(5, stickers[rowNum - 2].Tier - 1);
                }
                return stickers;
            });
        }

        private static Dictionary<string, string> ParseRow(ExcelRange cells, int rowNum)
        {
            return new Dictionary<string, string>
            {
                {"Title", cells[rowNum, 1].Value?.ToString() ?? ""},
                {"Author", cells[rowNum, 2].Value?.ToString() ?? ""},
                {"Tier", cells[rowNum, 3].Value?.ToString() ?? ""},
                {"Emoji", cells[rowNum, 4].Value?.ToString() ?? ""},
                {"Effect", cells[rowNum, 5].Value is { } e && int.TryParse(e.ToString(), out var effect) ? effect.ToString() : "0"},
                {"Description", cells[rowNum, 6].Value is string s ? s : ""}
            };
        }

        public override bool Match()
        {
            return User.Session.State == UserState.UploadFile && Message.Type == MessageType.Document;
        }

        public UploadFile(User user, BotDatabaseContext context, Message message) : base(user, context, message)
        {
            User.Session.State = UserState.Default;
        }
    }
}