using System.Threading.Tasks;
using CardCollector.DataBase.Entity;
using Telegram.Bot.Types;

namespace CardCollector.Commands
{
    /* Реализация базового класса для обработки получаемых
     с сервера Телеграм обновлений.
     Поле User - пользователь, вызвавший команду 
     Поле Update - полученное обновление
     Поле Command - команда или же ключевое слово, по которому идентифицируется команда
     Метод Execute - реализация логики команды во время ее выполнения 
     Метод IsMatches - проверяет команду на сопадение по ключу */
    public abstract class UpdateModel
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
        }

        protected internal abstract bool IsMatches(UserEntity user, Update update);

        protected UpdateModel()
        {
            User = null;
        }

        protected UpdateModel(UserEntity user, Update update)
        {
            User = user;
            user.Session.UpdateLastAccess();
            Update = update;
        }
    }
}