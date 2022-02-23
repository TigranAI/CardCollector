using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CardCollector.Database;
using CardCollector.Resources;
using CardCollector.Resources.Translations;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands.CallbackQueryHandler.Login
{
    public class ConfirmLogin : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.confirm_login;
        
        protected override async Task Execute()
        {
            HttpClient httpClient;
            if (Constants.DEBUG)
            {
                var clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = delegate { return true; };
                httpClient = new HttpClient(clientHandler);
            }
            else httpClient = new HttpClient();
            var values = new Dictionary<string, string>
            {
                { "userId", User.Id.ToString() },
                { "secretKey",  CallbackQuery.Data!.Split('=')[1]}
            };
            var content = new FormUrlEncodedContent(values);
            var siteUrl = $"https://{AppSettings.SITE_URL}/events/login";
            var response = await httpClient.PostAsync(siteUrl, content);
            if (response.StatusCode == HttpStatusCode.OK)
                await User.Messages.EditMessage(User, Messages.successfully_authorized, Keyboard.BackKeyboard);
            else
                await User.Messages.EditMessage(User, Messages.unexpected_exception, Keyboard.BackKeyboard);
        }

        public ConfirmLogin(User user, BotDatabaseContext context, CallbackQuery callbackQuery) : base(user, context, callbackQuery) { }
    }
}