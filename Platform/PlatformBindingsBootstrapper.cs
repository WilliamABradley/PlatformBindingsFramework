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

namespace PlatformBindings
{
    public static class PlatformBindingsBootstrapper
    {
        public static void Initialise(bool HasUI)
        {
#if UWP
            new UWPAppServices(HasUI);
#elif ANDROID
            new AndroidAppServices(HasUI);
#elif WIN32
            new Win32AppServices(HasUI);
#elif NETCore
            new NETCoreServices(HasUI);
#else
            throw new NotImplementedException("This platform isn't supported yet.");
#endif
        }

#if UWP
        public static void AttachDispatcher(Windows.UI.Core.CoreDispatcher Dispatcher)
        {
            ((UWPAppServices)AppServices.Current).AttachDispatcher(Dispatcher);
        }
#endif
    }
}