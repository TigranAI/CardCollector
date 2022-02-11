using System;
using System.Threading.Tasks;
using CardCollector.Attributes.Menu;
using CardCollector.DataBase;
using CardCollector.DataBase.Entity;
using CardCollector.DataBase.EntityDao;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands
{
    public abstract class HandlerModel
    {
        protected abstract string CommandText { get; }
        protected virtual bool ClearMenu => false;
        protected virtual bool ClearStickers => false;
        
        protected User User;
        protected BotDatabaseContext Context;

        public async Task InitNewContext(long userId)
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
            var activity = new UserActivity() {User = User, Action = GetType()};
            await Context.UserActivities.AddAsync(activity);
            
            User.InitSession();
            if (ClearMenu) User.Session.ClearMenuStack();
            if (!Attribute.IsDefined(GetType(), typeof(DontAddToCommandStack)))
                User.Session.AddCommandToStack(this);
            if (ClearStickers) await User.Messages.ClearStickers(User);
        }
        
        protected abstract Task Execute();

        protected virtual async Task AfterExecute()
        {
            if (Context.IsDisposed()) return;
            Context.ChangeTracker.DetectChanges();
            await Context.SaveChangesAsync();
            await Context.DisposeAsync();
        }

        public abstract bool Match();

        protected HandlerModel(User user, BotDatabaseContext context)
        {
            User = user;
            Context = context;
        }
    }
}