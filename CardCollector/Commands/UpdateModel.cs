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
        protected abstract string Command { get; }
        protected UserEntity User;
        protected Update Update;

        public abstract Task Execute();

        protected internal virtual bool IsMatches(string command)
        {
            return command.Contains(Command);
        }

        protected UpdateModel()
        {
            User = null;
        }

        protected UpdateModel(UserEntity user, Update update)
        {
            User = user;
            Update = update;
        }
    }
}