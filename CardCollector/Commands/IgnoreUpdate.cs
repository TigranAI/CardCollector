using System.Threading.Tasks;

namespace CardCollector.Commands
{
    /* Данный класс позволяет проигнорировать входящее обновление */
    public class IgnoreUpdate : UpdateModel
    {
        protected override string Command => "";
        public override Task Execute() { return  Task.CompletedTask; }
    }
}