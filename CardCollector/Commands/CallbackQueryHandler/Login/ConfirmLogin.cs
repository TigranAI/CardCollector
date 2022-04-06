using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CardCollector.Resources;
using CardCollector.Resources.Translations;

namespace CardCollector.Commands.CallbackQueryHandler.Login
{
    public class ConfirmLogin : CallbackQueryHandler
    {
        protected override string CommandText => CallbackQueryCommands.confirm_login;
        
        protected override async Task Execute()
        {
            HttpClient httpClient = Constants.DEBUG
                ? new HttpClient(new HttpClientHandler() {
                    ServerCertificateCustomValidationCallback = delegate { return true; }
                })
                : new HttpClient();
            
            var values = new Dictionary<string, string>
            {
                { "userId", User.Id.ToString() },
                { "key",  CallbackQuery.Data!.Split('=')[1]}
            };
            var content = new FormUrlEncodedContent(values);
            var siteUrl = $"https://{AppSettings.SITE_URL}/login/event";
            var response = await httpClient.PostAsync(siteUrl, content);
            if (response.StatusCode == HttpStatusCode.OK)
                await User.Messages.EditMessage(User, Messages.successfully_authorized, Keyboard.BackKeyboard);
            else
                await User.Messages.EditMessage(User, Messages.unexpected_exception, Keyboard.BackKeyboard);
        }
    }
}