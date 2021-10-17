using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;

namespace CardCollector.Session
{
    public class UserSession
    {
        /* Ссылка на пользователя */
        private readonly UserEntity user;
        /* Дата и время последней актвности пользователя */
        private DateTime _lastAccess = DateTime.Now;
        /* Текущее состояние пользователя */
        public UserState State = UserState.Default;
        /* Подключаемые модули */
        private readonly Dictionary<Type, Module> Modules = new();
        /* Сообщения в чате пользователя */
        public readonly List<int> Messages = new();

        public UserSession(UserEntity user)
        {
            this.user = user;
        }

        public T InitNewModule<T>() where T : Module
        {
            if (!Modules.ContainsKey(typeof(T))) Modules.Add(typeof(T), Activator.CreateInstance<T>());
            return (T) Modules[typeof(T)];
        }

        public T GetModule<T>() where T : Module
        {
            try {
                return (T) Modules[typeof(T)];
            } catch {
                return InitNewModule<T>();
            }
        }

        public void ResetModule<T>() where T : Module
        {
            Modules[typeof(T)].Reset();
        }

        public void DeleteModule<T>() where T : Module
        {
            Modules.Remove(typeof(T));
        }
        
        public void UpdateLastAccess()
        {
            _lastAccess = DateTime.Now;
        }

        public int GetLastAccessInterval()
        {
            return (int) (DateTime.Now - _lastAccess).TotalMinutes;
        }


        public async Task ClearMessages()
        {
            foreach (var messageId in Messages.ToList())
                await MessageController.DeleteMessage(user, messageId, false);
            Messages.Clear();
        }

        public async Task EndSession()
        {
            await ClearMessages();
            State = UserState.Default;
            foreach (var module in Modules.Values) module.Reset();
            Modules.Clear();
        }
    }
}