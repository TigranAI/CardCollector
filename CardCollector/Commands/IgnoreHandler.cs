using System.Threading.Tasks;

namespace CardCollector.Commands
{
    public class IgnoreHandler : HandlerModel
    {
        protected override string CommandText => "";

        public override Task PrepareAndExecute() => Task.CompletedTask;
        protected override Task Execute() => Task.CompletedTask;
        public override bool Match() => false;
    }
}