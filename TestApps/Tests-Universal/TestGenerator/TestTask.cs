using PlatformBindings;
using System;
using System.Threading.Tasks;

namespace Tests.TestGenerator
{
    public class TestTask : ITestItem
    {
        public async void RunTest()
        {
            var result = await Test(AttachedUI);
            if (result != null) AppServices.Current.UI.PromptUser("Test Complete", result, "OK");
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