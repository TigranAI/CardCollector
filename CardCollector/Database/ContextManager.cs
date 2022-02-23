using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace CardCollector.Database
{
    public static class ContextManager
    {
        private static ConcurrentDictionary<double, BotDatabaseContext> _contextsList = new ();

        public static void AddNewContext(double id, BotDatabaseContext botDatabaseContext)
        {
            _contextsList.TryAdd(id, botDatabaseContext);
        }

        public static void DeleteContext(double id)
        {
            _contextsList.TryRemove(id, out _);
        }

        public static async Task DisposeAllAsync()
        {
            foreach (var context in _contextsList.Values.ToList())
            {
                await context.SaveChangesAsync();
                await context.DisposeAsync();
            }
        }
    }
}