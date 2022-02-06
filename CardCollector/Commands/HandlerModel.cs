using System.Threading.Tasks;
using CardCollector.DataBase;
using User = CardCollector.DataBase.Entity.User;

namespace CardCollector.Commands
{
    public abstract class HandlerModel
    {
        protected abstract string CommandText { get; }
        protected virtual bool ClearMenu => false;
        protected virtual bool AddToStack => false;
        protected virtual bool ClearStickers => false;
        
        protected User User;
        protected BotDatabaseContext Context;

        public virtual async Task PrepareAndExecute()
        {
            await BeforeExecute();
            await Execute();
            await AfterExecute();
        }

        protected virtual async Task BeforeExecute()
        {
            User.Session.SetCurrentCommand(GetType());
            if (ClearMenu) User.Session.ClearMenuStack();
            if (AddToStack) User.Session.AddMenuToStack(this);
            if (ClearStickers) await User.Messages.ClearStickers(User);
        }
        
        protected abstract Task Execute();

        protected virtual async Task AfterExecute()
        {
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