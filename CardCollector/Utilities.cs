using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CardCollector.Resources;
using Telegram.Bot;

namespace CardCollector
{
    public static class Utilities
    {
        public static string ToJson(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
        
        public static string CreateMd5(string input)
        {
            // Use input string to calculate MD5 hash
            using var md5 = System.Security.Cryptography.MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(input);
            var hashBytes = md5.ComputeHash(inputBytes);
            // Convert the byte array to hexadecimal string
            var sb = new StringBuilder();
            foreach (var t in hashBytes)
                sb.Append(t.ToString("X2"));
            return sb.ToString();
        }

        public static async Task DownloadFile(string fileId)
        {
            await Task.Run(async () =>
            {
                /* Получаем информацию о файле */
                var fileInfo = await Bot.Client.GetFileAsync(fileId);
                /* Собираем ссылку на файл */
                var fileUri = $"https://api.telegram.org/file/bot{AppSettings.TOKEN}/{fileInfo.FilePath}";
                /* Загружаем файл */
                var client = new WebClient();
                client.DownloadFile(new Uri(fileUri), "pack.zip");
            });
        }
    }
}