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

using PlatformBindings;
using System;
using System.Threading.Tasks;

namespace Tests.TestGenerator
{
    public class TestTask : ITestItem
    {
        public async void RunTest()
        {
            try
            {
                var result = await Test(AttachedUI);
                if (result != null) AppServices.Current.UI.PromptUser("Test Complete", result, "OK");
            }
            catch (Exception ex)
            {
                var message = $"Test Caused exception: \n{ex.Message}\nStackTrace:\n{ex.StackTrace}";
                AppServices.Current.UI.PromptUser("Test Failed", message, "OK");
            }
        }

        public string Name { get; set; }
        public Func<object, Task<string>> Test { get; set; }

        public event EventHandler<object> UIAttached;

        public object AttachedUI
        {
            get { return _AttachedUI; }
            set
            {
                _AttachedUI = value;
                UIAttached?.Invoke(this, _AttachedUI);
            }
        }

        private object _AttachedUI = null;
    }
}