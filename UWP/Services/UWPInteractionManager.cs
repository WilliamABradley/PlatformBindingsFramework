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
using System.Linq;
using PlatformBindings.Enums;
using Windows.Gaming.Input;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml.Automation.Peers;
using Windows.UI.Xaml.Automation.Provider;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using PlatformBindings.Common;
using PlatformBindings.Models;

namespace PlatformBindings.Services
{
    public class UWPInteractionManager : InteractionManager
    {
        public UWPInteractionManager(IUIBindingInfo UIBinding) : base(UIBinding)
        {
            UIBinding.Execute(() =>
            {
                Window = CoreWindow.GetForCurrentThread();
                Window.KeyDown += Window_KeyDown;
                SystemNavigationManager.GetForCurrentView().BackRequested += InteractionManager_BackRequested;
                UINavigationController.UINavigationControllerAdded += delegate { ControllerAttached?.Invoke(this, null); isControllerAttached = UINavigationController.UINavigationControllers.Any(); };
                UINavigationController.UINavigationControllerRemoved += delegate { ControllerRemoved?.Invoke(this, null); isControllerAttached = UINavigationController.UINavigationControllers.Any(); };
            });
        }

        private void Window_KeyDown(CoreWindow sender, KeyEventArgs args)
        {
            switch (args.VirtualKey)
            {
                case VirtualKey.NavigationMenu:
                case VirtualKey.GamepadMenu:
                    Request(InteractionType.Menu);
                    break;

                case VirtualKey.GamepadX:
                case VirtualKey.Application:
                    Request(InteractionType.ContextMenu);
                    break;

                case VirtualKey.GamepadA:
                    var currentElement = FocusManager.GetFocusedElement();
                    if (currentElement is ListViewItem listitem)
                    {
                        ListViewItemAutomationPeer peer = new ListViewItemAutomationPeer(listitem);
                        if (peer.GetPattern(PatternInterface.Invoke) is IInvokeProvider invoker) invoker.Invoke();
                        else Request(InteractionType.EnterObject);
                    }
                    break;

                case VirtualKey.F1:
                    Request(InteractionType.Help);
                    break;

                case VirtualKey.F5:
                    Request(InteractionType.SyncOrRefresh);
                    break;

                case VirtualKey.F:
                    if (ControlKeyDown) Request(InteractionType.Find);
                    break;

                case VirtualKey.G:
                    if (ControlKeyDown) Request(InteractionType.Menu);
                    break;

                case VirtualKey.Q:
                    if (ControlKeyDown) Request(InteractionType.Share);
                    break;

                case VirtualKey.Delete:
                    if (ContentDialogHelper.ActiveDialog == null) Request(InteractionType.Remove);
                    break;

                default:
                    args.Handled = false;
                    break;
            }
        }

        private void InteractionManager_BackRequested(object sender, BackRequestedEventArgs e)
        {
            Request(InteractionType.Back);
            e.Handled = true;
        }

        private bool IsKeyActive(CoreVirtualKeyStates state)
        {
            var DownAndLocked = CoreVirtualKeyStates.Down | CoreVirtualKeyStates.Locked;
            return state == CoreVirtualKeyStates.Down || state == DownAndLocked;
        }

        private void Request(InteractionType Type)
        {
            Request(Window, Type);
        }

        public override bool ControlKeyDown { get { return IsKeyActive(CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Control)); } }
        public override bool AltKeyDown { get { return IsKeyActive(CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Menu)); } }
        public override bool ShiftKeyDown { get { return IsKeyActive(CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Shift)); } }

        private CoreWindow Window;
        public bool isControllerAttached = false;

        public event EventHandler ControllerAttached;

        public event EventHandler ControllerRemoved;
    }
}