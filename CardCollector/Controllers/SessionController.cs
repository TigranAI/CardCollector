using System.Collections.Generic;
using CardCollector.DataBase.Entity;
using CardCollector.Session;

namespace CardCollector.Controllers
{
    public static class SessionController
    {
        private static Dictionary<long, UserSession> _openedSessions = new();

        public static UserSession? FindSession(User user)
        {
            if (_openedSessions.ContainsKey(user.Id)) return _openedSessions[user.Id];
            return null;
        }

        public static void AddSession(User user)
        {
            if (!_openedSessions.ContainsKey(user.Id)) _openedSessions.Add(user.Id, user.Session);
        }
    }
}