using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardCollector.Commands;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;

namespace CardCollector.Session
{
    public class UserSession
    {
        /* Ссылка на пользователя */
        public User User;
        /* Дата и время последней актвности пользователя */
        private DateTime _lastAccess = DateTime.Now;
        /* Текущее состояние пользователя */
        public UserState State = UserState.Default;
        /* Подключаемые модули */
        private readonly Dictionary<Type, Module> _modules = new();
        /* Последовательность вызова списка меню */
        private readonly Stack<MenuInformation> _menuStack = new();
        public Type PreviousCommandType;
        private Type _currentCommandType;

        public UserSession(User user)
        {
            User = user;
        }

        public bool ChosenResultWithMessage = false;

        public T InitNewModule<T>() where T : Module
        {
            if (!_modules.ContainsKey(typeof(T))) _modules.Add(typeof(T), Activator.CreateInstance<T>());
            return (T) _modules[typeof(T)];
        }

        public T GetModule<T>() where T : Module
        {
            try {
                return (T) _modules[typeof(T)];
            } catch {
                return InitNewModule<T>();
            }
        }

        public void ResetModule<T>() where T : Module
        {
            try { _modules[typeof(T)].Reset(); }
            catch (Exception) { /**/ }
        }

        public void DeleteModule<T>() where T : Module
        {
            _modules.Remove(typeof(T));
        }
        
        public void UpdateLastAccess()
        {
            _lastAccess = DateTime.Now;
        }

        public int GetLastAccessInterval()
        {
            return (int) (DateTime.Now - _lastAccess).TotalMinutes;
        }

        public async Task EndSession()
        {
            await User.Messages.ClearChat(User);
            State = UserState.Default;
            foreach (var module in _modules.Values) module.Reset();
            _modules.Clear();
            _menuStack.Clear();
        }

        public bool TryGetPreviousMenu(out MenuInformation menu)
        {
            while (_menuStack.TryPeek(out menu) && _currentCommandType == menu.GetMenuType()) {
                PopLast();
            }
            return _menuStack.TryPeek(out menu);
        }

        public void PopLast()
        {
            _menuStack.TryPop(out _);
        }

        public void AddMenuToStack(HandlerModel menu)
        {
            var menuInfo = new MenuInformation(menu, User);
            if (!_menuStack.Contains(menuInfo, new MenuComparer())) _menuStack.Push(menuInfo);
        }

        public void SetCurrentCommand(Type commandType)
        {
            PreviousCommandType = _currentCommandType;
            /*if(commandType != typeof(Back)) CurrentCommandType = commandType;*/
        }

        public void UndoCurrentCommand()
        {
            if (_menuStack.TryPeek(out var menu) && menu.GetMenuType() == _currentCommandType) _menuStack.Pop();
            _currentCommandType = PreviousCommandType;
        }

        public void ClearMenuStack()
        {
            _menuStack.Clear();
            foreach (var module in _modules.Values)
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