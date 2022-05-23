using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Attributes;
using CardCollector.Database;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using CardCollector.Resources.Translations;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands
{
    public abstract class HandlerModel
    {
        protected abstract string CommandText { get; }
        protected virtual bool ClearMenu => false;
        protected virtual bool ClearStickers => false;
        protected readonly Dictionary<string, object> TRACE_DATA = new ();

        protected User User;
        protected BotDatabaseContext Context;

        public virtual async Task InitNewContext(long userId)
        {
            Context = new BotDatabaseContext();
            User = (await Context.Users.FindById(userId))!;
            User.InitSession();
        }
        
        public virtual async Task PrepareAndExecute()
        {
            try
            {
                await InitFields();
                await BeforeExecute();
                await Execute();
                await AfterExecute();
            }
            catch (Exception e)
            {
                await OnFallback(e);
            }
        }

        private async Task InitFields()
        {
            if (User.UserStats is null)
            {
                var stats = await Context.UsersStats.AddAsync(new UserStats() {User = User});
                User.UserStats = stats.Entity;
            }
        }

        private async Task OnFallback(Exception e)
        {
            try
            {
                using (var context = new BotDatabaseContext())
                {
                    User = (await context.Users.FindById(User.Id))!;
                    await User.Messages.ClearChat();
                    await User.Messages.SendMessage($"{Messages.unexpected_exception} {e.Message}");
                    Logs.LogOutError(e);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Logs.LogOutError(e);
                Logs.LogOutError(ex);
            }
        }

        protected virtual async Task BeforeExecute()
        {
            User.InitSession();
            if (ClearMenu) User.Session.ClearMenuStack();
            if (!Attribute.IsDefined(GetType(), typeof(SkipCommandAttribute)))
                User.Session.AddCommandToStack(this);
            if (Attribute.IsDefined(GetType(), typeof(ResetModuleAttribute)))
                User.Session.ResetModules();
            if (ClearStickers) await User.Messages.ClearStickers();
        }
        
        protected abstract Task Execute();

        protected virtual async Task AfterExecute()
        {
            Logs.LogOut(this);
            if (!Context.IsDisposed())
            {
                if (Attribute.IsDefined(GetType(), typeof(StatisticsAttribute)))
                    await SaveActivity(Context);
                await Context.SaveChangesAsync();
                await Context.DisposeAsync();
            }
            else if (Attribute.IsDefined(GetType(), typeof(StatisticsAttribute)))
            {
                using (var context = new BotDatabaseContext())
                {
                    await SaveActivity(context);
                    await context.SaveChangesAsync();
                }
            }
        }

        protected virtual async Task SaveActivity(BotDatabaseContext context)
        {
            var activity = new UserActivity() {User = User, Action = GetType().FullName};
            await context.UserActivities.AddAsync(activity);
        }

        public override string ToString()
        {
            var userId = $"user id: {User?.Id.ToString() ?? "null"}";
            var commandFullname = GetType().FullName ?? GetType().Name;
            var traceData = Utilities.ToJson(TRACE_DATA, Formatting.Indented);
            return $"[{commandFullname}] triggered by [{userId}]. Trace data: {traceData}";
        }

        public abstract bool Match();

        public virtual HandlerModel Init(User user, BotDatabaseContext context, Update update)
        {
            User = user;
            Context = context;
            return this;
        }
    }
}