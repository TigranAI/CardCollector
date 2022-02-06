using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Commands;
using CardCollector.Controllers;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;

namespace CardCollector.Session
{
    public class UserSession
    {
        /* Ссылка на пользователя */
        public User user;
        /* Дата и время последней актвности пользователя */
        private DateTime _lastAccess = DateTime.Now;
        /* Текущее состояние пользователя */
        public UserState State = UserState.Default;
        /* Подключаемые модули */
        private readonly Dictionary<Type, Module> Modules = new();
        /* Сообщения в чате пользователя */
        public readonly List<int> StickerMessages = new();
        public readonly List<int> Messages = new();
        /* Последовательность вызова списка меню */
        private readonly Stack<MenuInformation> MenuStack = new();
        public Type PreviousCommandType;
        private Type CurrentCommandType;

        public UserSession(User user)
        {
            this.user = user;
        }

        public bool ChosenResultWithMessage = false;

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
            try { Modules[typeof(T)].Reset(); }
            catch (Exception) { /**/ }
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
                await MessageController.DeleteMessage(user, messageId);
            foreach (var messageId in StickerMessages.ToList())
                await MessageController.DeleteMessage(user, messageId);
        }

        public async Task ClearStickers()
        {
            foreach (var messageId in StickerMessages.ToList())
                await MessageController.DeleteMessage(user, messageId);
        }

        public async Task EndSession()
        {
            await ClearMessages();
            State = UserState.Default;
            foreach (var module in Modules.Values) module.Reset();
            Modules.Clear();
            MenuStack.Clear();
        }

        public bool TryGetPreviousMenu(out MenuInformation menu)
        {
            while (MenuStack.TryPeek(out menu) && CurrentCommandType == menu.GetMenuType()) {
                PopLast();
            }
            return MenuStack.TryPeek(out menu);
        }

        public void PopLast()
        {
            MenuStack.TryPop(out _);
        }

        public void AddMenuToStack(HandlerModel menu)
        {
            var menuInfo = new MenuInformation(menu, State);
            if (!MenuStack.Contains(menuInfo, new MenuComparer())) MenuStack.Push(menuInfo);
        }

        public void SetCurrentCommand(Type commandType)
        {
            PreviousCommandType = CurrentCommandType;
            /*if(commandType != typeof(Back)) CurrentCommandType = commandType;*/
        }

        public void UndoCurrentCommand()
        {
            if (MenuStack.TryPeek(out var menu) && menu.GetMenuType() == CurrentCommandType) MenuStack.Pop();
            CurrentCommandType = PreviousCommandType;
        }

        public void ClearMenuStack()
        {
            MenuStack.Clear();
            foreach (var module in Modules.Values)
            {
                if (module.GetType() == typeof(FiltersModule)) continue;
                module.Reset();
            }
        }
    }

    public class MenuComparer : IEqualityComparer<MenuInformation>
    {
        public bool Equals(MenuInformation x, MenuInformation y)
        {
            return x?.GetMenuType() == y?.GetMenuType();
        }

        public int GetHashCode(MenuInformation obj)
        {
            return base.GetHashCode();
        }
    }
}