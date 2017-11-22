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