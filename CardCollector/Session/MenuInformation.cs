﻿using System;
using System.Threading.Tasks;
using CardCollector.Commands;
using CardCollector.Resources;
using CardCollector.Session.Modules;

namespace CardCollector.Session
{
    public class MenuInformation
    {
        private UpdateModel Parent;
        private UserState State;

        public async Task BackToThis(UserSession session)
        {
            session.State = State;
            switch (State)
            {
                case UserState.ShopMenu:
                    session.ResetModule<ShopModule>();
                    break;
            }
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