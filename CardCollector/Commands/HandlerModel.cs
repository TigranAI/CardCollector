using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardCollector.Attributes.Logs;
using CardCollector.Attributes.Menu;
using CardCollector.Database;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
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
            User = await Context.Users.FindById(userId);
            User.InitSession();
        }
        
        public virtual async Task PrepareAndExecute()
        {
            await BeforeExecute();
            await Execute();
            await AfterExecute();
        }

        protected virtual async Task BeforeExecute()
        {
            User.InitSession();
            if (ClearMenu) User.Session.ClearMenuStack();
            if (!Attribute.IsDefined(GetType(), typeof(DontAddToCommandStack)))
                User.Session.AddCommandToStack(this);
            if (Attribute.IsDefined(GetType(), typeof(ResetModuleAttribute)))
                User.Session.ResetModules();
            if (ClearStickers) await User.Messages.ClearStickers(User);
        }
        
        protected abstract Task Execute();

        protected virtual async Task AfterExecute()
        {
            Logs.LogOut(this);
            if (!Context.IsDisposed())
            {
                if (Attribute.IsDefined(GetType(), typeof(SavedActivityAttribute)))
                    await SaveActivity(Context);
                await Context.SaveChangesAsync();
                await Context.DisposeAsync();
            }
            else if (Attribute.IsDefined(GetType(), typeof(SavedActivityAttribute)))
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
            string userId = $"user id: {User?.Id.ToString() ?? "null"}";
            string commandFullname = GetType().FullName ?? GetType().Name;
            string traceData = Utilities.ToJson(TRACE_DATA, Formatting.Indented);
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