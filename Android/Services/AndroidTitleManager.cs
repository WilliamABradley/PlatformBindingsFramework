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

using PlatformBindings.Common;
using System;

namespace PlatformBindings.Services
{
    public class AndroidTitleManager : ITitleManager
    {
        public string WindowTitle { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

        public string PageTitle
        {
            get
            {
                return AndroidHelpers.GetCurrentActivity().Title;
            }
            set
            {
                try
                {
                    AndroidHelpers.GetCurrentActivity().Title = value;
                }
                catch { }
            }
        }

        public bool SupportsWindowTitle => false;

        public bool SupportsPageTitle => true;
    }
}