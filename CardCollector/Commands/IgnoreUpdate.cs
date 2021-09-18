using System.Threading.Tasks;

namespace CardCollector.Commands
{
    /* Данный класс позволяет проигнорировать входящее обновление */
    public class IgnoreUpdate : UpdateModel
    {
        protected override string CommandText => "";
        public override Task Execute() { return  Task.CompletedTask; }
    }
}