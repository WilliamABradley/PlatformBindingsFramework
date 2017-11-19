using PlatformBindings.ViewModels;
using System.Collections.Generic;
using Tests.TestGenerator;

namespace Tests.Tests
{
    public abstract class TestPage : ViewModelBase
    {
        public TestPage(ITestPageGenerator PageGenerator)
        {
            this.PageGenerator = PageGenerator;
        }

        public void DisplayTests()
        {
            foreach (var property in Properties)
            {
                PageGenerator?.CreateTestProperty(property);
            }
            foreach (var test in Tests)
            {
                PageGenerator?.CreateTestUI(test);
            }
        }

        public void AddTest(TestTask Test)
        {
            _Tests.Add(Test);
        }

        public void AddProperty(TestProperty Property)
        {
            _Properties.Add(Property);
        }

        private List<TestTask> _Tests { get; } = new List<TestTask>();
        public IReadOnlyList<TestTask> Tests => _Tests;

        private List<TestProperty> _Properties { get; } = new List<TestProperty>();
        public IReadOnlyList<TestProperty> Properties => _Properties;

        public ITestPageGenerator PageGenerator { get; }
    }
}