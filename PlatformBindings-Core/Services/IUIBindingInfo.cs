using System;
using System.Threading.Tasks;

namespace PlatformBindings.Services
{
    /// <summary>
    /// A UI Context Instance. Contains Dispatcher Information for Handling UI.
    /// </summary>
    public interface IUIBindingInfo
    {
        /// <summary>
        /// Performs an Action on the UI Thread without waiting to Complete.
        /// </summary>
        /// <param name="action">Action to Perform on UI Thread</param>
        void Execute(Action action);

        /// <summary>
        /// Performs an Action on the UI Thread, waiting for it to Complete. (Warning doesn't wait).
        /// </summary>
        /// <param name="action">Action to Perform on UI Thread</param>
        /// <returns>Completion Task</returns>
        Task ExecuteAsync(Action action);
    }
}