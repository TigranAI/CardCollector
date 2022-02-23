using System;
using System.Threading.Tasks;
using CardCollector.Attributes.Logs;
using CardCollector.Attributes.Menu;
using CardCollector.Database;
using CardCollector.Database.Entity;
using CardCollector.Database.EntityDao;
using User = CardCollector.Database.Entity.User;

namespace CardCollector.Commands
{
    public abstract class HandlerModel
    {
        protected abstract string CommandText { get; }
        protected virtual bool ClearMenu => false;
        protected virtual bool ClearStickers => false;
        
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
            if (!Context.IsDisposed())
            {
                if (Attribute.IsDefined(GetType(), typeof(SavedActivityAttribute)))
                {
                    var activity = new UserActivity() {User = User, Action = GetType().FullName};
                    await Context.UserActivities.AddAsync(activity);
                }
                Context.ChangeTracker.DetectChanges();
                await Context.SaveChangesAsync();
                await Context.DisposeAsync();
            }
            else if (Attribute.IsDefined(GetType(), typeof(SavedActivityAttribute)))
            {
                using (var context = new BotDatabaseContext())
                {
                    var user = await context.Users.FindById(User.Id);
                    var activity = new UserActivity() {User = user, Action = GetType().FullName};
                    await Context.UserActivities.AddAsync(activity);
                    await context.SaveChangesAsync();
                }
            }
        }

        public abstract bool Match();

        protected HandlerModel(User user, BotDatabaseContext context)
        {
            User = user;
            Context = context;
        }
    }
}