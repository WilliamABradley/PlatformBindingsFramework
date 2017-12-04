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

using PlatformBindings.Models;
using System;

namespace PlatformBindings.Services
{
    public abstract class NavigationManager
    {
        public NavigationManager(Navigator Navigator)
        {
            this.Navigator = Navigator;
        }

        public void Navigate(object Page)
        {
            Navigator.Navigate(Page);
        }

        public void Navigate(object Page, string Parameter)
        {
            Navigator.Navigate(Page, Parameter);
        }

        public void Navigate(object Page, string Parameter, bool ClearBackStack)
        {
            Navigator.Navigate(Page, Parameter, ClearBackStack);
        }

        public abstract void GoBack();

        public abstract bool CanGoBack { get; }

        public abstract bool ShowBackButton { get; set; }

        public virtual event EventHandler<bool> BackButtonStateChanged;

        public INavigationMenuHandler Menu { get; set; }

        public Navigator Navigator { get; }
    }
}