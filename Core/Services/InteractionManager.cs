using System;
using System.Collections.Generic;
using System.Linq;
using PlatformBindings.Enums;
using PlatformBindings.Models.Events;
using PlatformBindings.Models;

namespace PlatformBindings.Services
{
    public abstract class InteractionManager
    {
        public InteractionManager(IUIBindingInfo UIBinding)
        {
            this.UIBinding = UIBinding;
        }

        public void AddInteractionRequest(EventHandler<InteractionRequestArgs> action)
        {
            InteractionList.Insert(0, action);
        }

        public void RemoveInteractionRequest(EventHandler<InteractionRequestArgs> action)
        {
            InteractionList.Remove(action);
        }

        protected void Request(object Source, InteractionType Request)
        {
            var args = new InteractionRequestArgs { RequestType = Request, Handled = false };
            foreach (var action in InteractionList.ToList())
            {
                action(Source, args);
            }
        }

        public IUIBindingInfo UIBinding { get; }

        public abstract bool ControlKeyDown { get; }
        public abstract bool ShiftKeyDown { get; }
        public abstract bool AltKeyDown { get; }

        private static List<EventHandler<InteractionRequestArgs>> InteractionList = new List<EventHandler<InteractionRequestArgs>>();
    }
}