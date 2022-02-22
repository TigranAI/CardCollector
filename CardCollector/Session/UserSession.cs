using System;
using System.Collections.Generic;
using CardCollector.Attributes.Menu;
using CardCollector.Commands;
using CardCollector.Others;
using CardCollector.Resources;
using CardCollector.Resources.Enums;
using CardCollector.Session.Modules;

namespace CardCollector.Session
{
    public class UserSession
    {
        private DateTime _lastAccess = DateTime.Now;
        public UserState State = UserState.Default;
        private readonly Dictionary<Type, Module> _modules = new();
        private readonly UniqueStack<HandlerModel> _commandStack = new(new HandlerEqualityComparer());

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

        public void EndSession()
        {
            State = UserState.Default;
            foreach (var module in _modules.Values) module.Reset();
            _modules.Clear();
            _commandStack.Clear();
        }

        public HandlerModel? GetPreviousCommand()
        {
            if (!_commandStack.TryPop(out var current)) return null;
            while (_commandStack.TryPop(out var previousCommand))
            {
                if (Attribute.IsDefined(previousCommand.GetType(), typeof(MenuPoint))) return previousCommand;
            }
            return null;
        }

        public void AddCommandToStack(HandlerModel menu)
        {
            _commandStack.Push(menu);
        }

        public void ClearMenuStack()
        {
            _commandStack.Clear();
        }

        public void PopLastCommand()
        {
            _commandStack.TryPop(out _);
        }

        public void ResetModules()
        {
            foreach (var module in _modules.Values)
            {
                if (module.GetType() == typeof(FiltersModule)) continue;
                module.Reset();
            }
        }
    }
}