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

using System.Collections.Generic;
using PlatformBindings.Enums;

namespace PlatformBindings.Models.FileSystem
{
    /// <summary>
    /// Customises the visibility and starting location of Files in the Picker.
    /// </summary>
    public abstract class PickerProperties
    {
        /// <summary>
        /// List of File Extensions to Filter by. (Requires the "." prefix)
        /// </summary>
        public IList<string> FileTypes { get; } = new List<string>();

        /// <summary>
        /// Starting Location of the Picker, some options might be unavailable on some platforms, this property might have no effect on some platforms.
        /// </summary>
        public PathRoot? StartingLocation { get; set; }

        /// <summary>
        /// The Suggested file/folder to start with.
        /// </summary>
        public StorageContainer SuggestedStorageItem { get; set; }

        /// <summary>
        /// A Title for the use of the Picker Prompt.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// An identifier for the picker. In UWP, this allows pickers to share a starting location.
        /// </summary>
        public string PickerIdentifier { get; set; }
    }
}