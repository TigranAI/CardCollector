using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;

namespace CardCollector.Extensions
{
    public static class Extensions
    {
        public static void Apply<T> (this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source) action.Invoke(item);
        }
        
        public static async Task ApplyAsync<T> (this IEnumerable<T> source, Func<T, Task> action)
        {
            foreach (var item in source) await action.Invoke(item);
        }

        public static IQueryable<T> OrderByRandom<T>(this IQueryable<T> query)
        {
            return from q in query orderby Guid.NewGuid() select q;
        }

        public static T And<T>(this T t, Action<T> action)
        {
            action.Invoke(t);
            return t;
        }

        public static async Task<File> UploadFileAsync(this ITelegramBotClient client, long chatId, InputFileStream fileStream)
        {
            var message = await client.SendDocumentAsync(chatId, 
                new InputOnlineFile(fileStream.Content, fileStream.FileName));
            await client.DeleteMessageAsync(chatId, message.MessageId);
            return new File()
            {
                FileId = message.Document.FileId,
                FileUniqueId = message.Document.FileUniqueId,
                FileSize = message.Document.FileSize
            };
        }
    }
}