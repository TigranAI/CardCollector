using System;
using System.Threading.Tasks;
using CardCollector.Commands;
using CardCollector.Resources;

namespace CardCollector.Session
{
    public class MenuInformation
    {
        private UpdateModel Parent;
        private UserState State;

        public async Task BackToThis(UserSession session)
        {
            session.State = State;
            await Parent.PrepareAndExecute();
        }

        public Type GetMenuType()
        {
            return Parent.GetType();
        }

        public MenuInformation(UpdateModel parent, UserState state)
        {
            Parent = parent;
            State = state;
        }
    }
}