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

namespace PlatformBindings.Models
{
    public class ConsoleTitleManager : ITitleManager
    {
        public string PageTitle { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

        public string WindowTitle
        {
            get
            {
                return Console.Title;
            }
            set
            {
                Console.Title = value;
            }
        }

        public bool SupportsWindowTitle => true;

        public bool SupportsPageTitle => false;
    }
}