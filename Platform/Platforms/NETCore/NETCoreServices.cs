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

using PlatformBindings.Services;
using System;

namespace PlatformBindings
{
    public class NETCoreServices : AppServices
    {
        internal NETCoreServices(bool HasUI) : base(HasUI, Enums.Platform.NETCore)
        {
            IO = new CoreIOBindings();
            UI = new CoreUIBindings();
            NetworkUtilities = new NetworkUtilities();
        }

        public static bool UseGlobalAppData = true;

        public override Version GetAppVersion()
        {
            throw new NotImplementedException();
        }
    }
}