// ******************************************************************
// Copyright (c) William Bradley
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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