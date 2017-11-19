using PlatformBindings;
using System;
using System.Threading.Tasks;

namespace Tests.TestGenerator
{
    public class TestTask
    {
        public async void RunTest()
        {
            var result = await Test(AttachedUI);
            if (result != null) AppServices.UI.PromptUser("Test Complete", result, "OK");
        }

        public string Name { get; set; }
        public Func<object, Task<string>> Test { get; set; }
        public object AttachedUI { get; set; }
    }
}