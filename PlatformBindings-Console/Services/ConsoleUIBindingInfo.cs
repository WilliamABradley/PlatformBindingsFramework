using System;
using System.Threading.Tasks;
using PlatformBindings.Services;

namespace PlatformBindings.ConsoleTools
{
    public class ConsoleUIBindingInfo : IUIBindingInfo
    {
        public void Execute(Action action)
        {
            action();
        }

        public Task ExecuteAsync(Action action)
        {
            action();
            return Task.FromResult(0);
        }
    }
}