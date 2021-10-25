using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using CardCollector.Resources;
using CardCollector.Session.Modules;
using OfficeOpenXml;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using File = System.IO.File;

namespace CardCollector.Commands.Message
{
    public class UploadFile : MessageCommand
    {
        protected override string CommandText => "";
        protected override bool ClearMenu => false;
        protected override bool AddToStack => false;

        public override async Task Execute()
        {
            User.Session.State = UserState.Default;
            var module = User.Session.GetModule<UploadedStickersModule>();
            try
            {
                /* Соообщаем, что начали загрузку файла */
                await MessageController.EditMessage(User, Messages.downloading_file);
                /* Загружаем файл */
                var fileName = await Utilities.DownloadFile(Update.Message?.Document);
                /* Сообщаем пользователю, что читаем документ */
                await MessageController.EditMessage(User, Messages.reading_document);
                /* Парсим файл */
                var stickersList = await ParseExcelFile(fileName, module.StickersList);
                /* Сообщаем пользователю, что удаляем файлы */
                await MessageController.EditMessage(User, Messages.deleting_files);
                File.Delete(fileName);
                /* Сообщаем пользователю, что загружаем стикеры */
                await MessageController.EditMessage(User, Messages.uploading_stickers);
                var packInfo = await PacksDao.AddNew(stickersList.First().Author);
                await StickerDao.AddRange(stickersList, packInfo.Id);
                /* Сообщаем пользователю, что стикеры загружены */
                await MessageController.EditMessage(User, Messages.stickers_succesfully_uploaded);
            }
            catch (Exception e)
            {
                /* Сообщаем пользователю, что произошла ошибка */
                await MessageController.EditMessage(User, $"{Messages.unexpected_exception}: {e.Message}");
            }
        }
        
        private async Task<List<StickerEntity>> ParseExcelFile(string fileName, List<StickerEntity> stickers)
        {
            return await Task.Run(() =>
            {
                using var xlPackage = new ExcelPackage(new FileInfo(fileName));
                var myWorksheet = xlPackage.Workbook.Worksheets.First(); //select sheet here
                for (var rowNum = 2; myWorksheet.Cells[rowNum, 1].Value is not null; rowNum++) //select starting row here
                {
                    var fields = ParseRow(myWorksheet.Cells, rowNum);
                    stickers[rowNum - 2].Title = fields["Title"];
                    stickers[rowNum - 2].Author = fields["Author"];
                    stickers[rowNum - 2].Tier = int.Parse(fields["Tier"]);
                    stickers[rowNum - 2].Emoji = fields["Emoji"];
                    stickers[rowNum - 2].Effect = int.Parse(fields["Effect"]);
                    stickers[rowNum - 2].Description = fields["Description"];
                    stickers[rowNum - 2].IncomeTime = 60;
                    stickers[rowNum - 2].Income = (int) Math.Pow(5, stickers[rowNum - 2].Tier - 1);
                    stickers[rowNum - 2].Md5Hash = Utilities.CreateMd5(stickers[rowNum - 2].Title);
                }
                return stickers;
            });
        }

        private static Dictionary<string, string> ParseRow(ExcelRange cells, int rowNum)
        {
            return new Dictionary<string, string>
            {
                {"Title", cells[rowNum, 1].Value.ToString()},
                {"Author", cells[rowNum, 2].Value.ToString()},
                {"Tier", cells[rowNum, 3].Value.ToString()},
                {"Emoji", cells[rowNum, 4].Value.ToString()},
                {"Effect", cells[rowNum, 5].Value is int e ? e.ToString() : "0"},
                {"Description", cells[rowNum, 6].Value is string s ? s : ""}
            };
        }

        protected internal override bool IsMatches(UserEntity user, Update update)
        {
            return user.Session.State == UserState.UploadFile && update.Message?.Type == MessageType.Document;
        }

        public UploadFile() { }
        public UploadFile(UserEntity user, Update update) : base(user, update) { }
    }
}