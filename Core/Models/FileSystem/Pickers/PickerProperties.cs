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
    }
}