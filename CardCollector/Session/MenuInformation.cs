using System;
using System.Threading.Tasks;
using CardCollector.Commands;
using CardCollector.DataBase.Entity;
using CardCollector.Resources;
using CardCollector.Session.Modules;

namespace CardCollector.Session
{
    public class MenuInformation
    {
        private HandlerModel Parent;
        private UserState State;
        protected long UserId;

        public async Task BackToThis(UserSession session)
        {
            session.State = State;
            switch (State)
            {
                case UserState.ShopMenu:
                    session.ResetModule<ShopModule>();
                    break;
                case UserState.CollectionMenu:
                    session.ResetModule<CollectionModule>();
                    session.ResetModule<CombineModule>();
                    break;
            }
            await Parent.InitNewContext(UserId);
            await Parent.PrepareAndExecute();
        }

        public Type GetMenuType()
        {
            return Parent.GetType();
        }

        public MenuInformation(HandlerModel parent, User user)
        {
            Parent = parent;
            State = user.Session.State;
            UserId = user.Id;
        }
    }
}