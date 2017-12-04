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

using PlatformBindings.ViewModels;
using System.Collections.Generic;
using System.Linq;
using Tests.TestGenerator;

namespace Tests.Tests
{
    public abstract class TestPage : ViewModelBase
    {
        public TestPage(string PageName, ITestPageGenerator PageGenerator)
        {
            this.PageName = PageName;
            this.PageGenerator = PageGenerator;
        }

        public void DisplayTests()
        {
            var extendedItems = TestService.GetExtensions(GetType());
            _Items.AddRange(extendedItems);

            foreach (var item in Items.OfType<TestProperty>())
            {
                PageGenerator?.CreateTestProperty(item);
            }

            foreach (var item in Items.OfType<TestTask>())
            {
                PageGenerator?.CreateTestUI(item);
            }
        }

        public void AddTestItem(ITestItem Test)
        {
            _Items.Add(Test);
        }

        private List<ITestItem> _Items { get; } = new List<ITestItem>();
        public IReadOnlyList<ITestItem> Items => _Items;

        public ITestPageGenerator PageGenerator { get; }
        public string PageName { get; }
    }
}