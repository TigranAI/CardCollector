using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CardCollector.Resources;
using Telegram.Bot;
using Telegram.Bot.Types;
using Sticker = CardCollector.DataBase.Entity.Sticker;

namespace CardCollector
{
    public static class Utilities
    {
        public static readonly Random rnd = new Random();
        
        public static string ToJson(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
        public static T FromJson<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public static async Task<string> DownloadFile(Document file)
        {
            /* Получаем информацию о файле */
            var fileInfo = await Bot.Client.GetFileAsync(file.FileId);
            /* Собираем ссылку на файл */
            var fileUri = $"https://api.telegram.org/file/bot{AppSettings.TOKEN}/{fileInfo.FilePath}";
            /* Загружаем файл */
            var client = new WebClient();
            client.DownloadFile(new Uri(fileUri), file.FileName ?? "file");
            return file.FileName ?? "file";
        }
        
        public static void ReplaceOldEmoji(List<Sticker> stickers)
        {
            string ToUnicode(string hex)
            {
                var sb = new StringBuilder();
                for (var i = 0; i < hex.Length; i+=4)
                {
                    var temp = hex.Substring(i, 4);
                    var character = (char)Convert.ToInt16(temp, 16);
                    sb.Append(character);
                }
                return sb.ToString();
            }
            string ToHex(string unicode)
            {
                return BitConverter.ToString(Encoding.BigEndianUnicode.GetBytes(unicode))
                    .Replace("-", "");
            }
            foreach (var sticker in stickers)
            {
                var hex = ToHex(sticker.Emoji);
                if (hex.Length < 5)
                    sticker.Emoji = ToUnicode(hex + "FE0F");
            }
        }
    }
}