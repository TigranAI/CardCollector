using System.Collections.Concurrent;
using CardCollector.DataBase.Entity;
using CardCollector.Session;

namespace CardCollector.Controllers
{
    public static class SessionController
    {
        private static ConcurrentDictionary<long, UserSession> _openedSessions = new();

        public static UserSession FindSession(User user)
        {
            if (_openedSessions.TryGetValue(user.Id, out var session))
                session.User = user;
            return _openedSessions.GetOrAdd(user.Id, new UserSession(user));
        }
    }
}