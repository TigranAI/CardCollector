using System.Threading.Tasks;

namespace CardCollector.Commands
{
    public class IgnoreHandler : HandlerModel
    {
        protected override string CommandText => "";
        protected override Task Execute() => Task.CompletedTask;
        public override bool Match() => false;
    }
}