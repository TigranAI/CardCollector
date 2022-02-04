using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Telegram.Bot.Types;

namespace CardCollector.Commands
{
    public abstract class HandlerModel
    {
        protected abstract string CommandText { get; }
        protected virtual bool ClearMenu => false;
        protected virtual bool AddToStack => false;
        protected virtual bool ClearStickers => false;
        protected UserEntity User;
        protected Update Update;

        public abstract Task Execute();

        public virtual async Task PrepareAndExecute()
        {
            User.Session.SetCurrentCommand(GetType());
            if (ClearMenu) User.Session.ClearMenuStack();
            if (AddToStack) User.Session.AddMenuToStack(this);
            if (ClearStickers) await User.Session.ClearStickers();
            await Execute();
            await AfterExecute();
        }

        public virtual Task AfterExecute() => Task.CompletedTask;

        protected internal abstract bool Match(UserEntity user, Update update);

        protected HandlerModel(UserEntity user, Update update)
        {
            if (user == null) return;
            if (update == null) return;
            User = user;
            user.Session.UpdateLastAccess();
            Update = update;
        }
    }
}