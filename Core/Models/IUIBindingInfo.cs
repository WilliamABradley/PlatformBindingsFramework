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

using System;
using System.Threading.Tasks;

namespace PlatformBindings.Models
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