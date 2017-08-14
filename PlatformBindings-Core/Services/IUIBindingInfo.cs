using System;
using System.Threading.Tasks;

namespace PlatformBindings.Services
{
    public interface IUIBindingInfo
    {
        void Execute(Action action);

        Task ExecuteAsync(Action action);
    }
}