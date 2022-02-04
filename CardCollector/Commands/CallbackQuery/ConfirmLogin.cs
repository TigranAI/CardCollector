using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using Telegram.Bot.Types;

namespace CardCollector.Commands.CallbackQuery
{
    public class ConfirmLogin : CallbackQueryHandler
    {
        protected override string CommandText => Command.confirm_login;
        
        public override async Task Execute()
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
                { "secretKey",  CallbackData.Split('=')[1]}
            };
            var content = new FormUrlEncodedContent(values);
            var siteUrl = $"https://{AppSettings.SITE_URL}/events/login";
            var response = await httpClient.PostAsync(siteUrl, content);
            if (response.StatusCode == HttpStatusCode.OK)
                await MessageController.EditMessage(User, Messages.successfully_authorized, Keyboard.BackKeyboard);
            else
                await MessageController.EditMessage(User, Messages.unexpected_exception, Keyboard.BackKeyboard);
        }

        public ConfirmLogin(UserEntity user, Update update) : base(user, update) { }
    }
}