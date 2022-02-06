using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CardCollector.DataBase
{
    public static class ContextManager
    {
        private static Dictionary<double, BotDatabaseContext> _contextsList = new ();

        public static void AddNewContext(double id, BotDatabaseContext botDatabaseContext)
        {
            _contextsList.Add(id, botDatabaseContext);
        }

        public static void DeleteContext(double id)
        {
            _contextsList.Remove(id);
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