using System;
using System.Threading.Tasks;
using PlatformBindings.Enums;
using PlatformBindings.Models.Settings;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using PlatformBindings.Models.FileSystem;
using PlatformBindings.Models;

namespace PlatformBindings.Common
{
    public static class PlatformBindingHelpers
    {
        /// <summary>
        /// Resolves the OS Independent File or Folder Path, into an OS Specific FileSystem Path.
        /// </summary>
        /// <param name="Path">Path to Resolve</param>
        /// <returns></returns>
        public static string ResolvePath(FolderPath Path)
        {
            char separator = System.IO.Path.DirectorySeparatorChar;

            var path = AppServices.IO.GetBaseFolder(Path.Root).Path;

            foreach (var piece in GetPathPieces(Path.Path))
            {
                path += separator + piece;
            }

            if (Path is FilePath file)
            {
                path += separator + file.FileName;
            }

            return path;
        }

        /// <summary>
        /// Gets a List of the Directories that make up the provided path
        /// </summary>
        /// <param name="path">Path to Separate</param>
        /// <returns>A List of Directories that make up the path</returns>
        public static List<string> GetPathPieces(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return new List<string>();

            path = path.Replace('/', '\\').TrimEnd('\\');
            return path.Split('\\').ToList();
        }

        /// <summary>
        /// Converts a <see cref="Stream"/> into a byte Array
        /// </summary>
        /// <param name="Stream">Stream to Convert</param>
        /// <returns>Byte Array of Stream</returns>
        public static byte[] GetByteArrayFromStream(this Stream Stream)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = Stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Performs an Action on the UI Thread
        /// </summary>
        /// <param name="action">Action to perform on the UI Thread</param>
        public static async void OnUIThread(Action action)
        {
            await OnUIThreadAsync(action);
        }

        /// <summary>
        /// Performs an Action on the UI Thread
        /// </summary>
        /// <param name="action">Action to perform on the UI Thread</param>
        /// <returns>Continuation Task</returns>
        public static async Task OnUIThreadAsync(Action action)
        {
            await AppServices.UI.DefaultUIBinding.ExecuteAsync(action);
        }

        /// <summary>
        /// Performs an Action on the UI Thread
        /// </summary>
        /// <param name="UIBinding">UI Context for Dispatching</param>
        /// <param name="action">Action to perform on the UI Thread</param>
        public static void OnUIThread(IUIBindingInfo UIBinding, Action action)
        {
            UIBinding.Execute(action);
        }

        /// <summary>
        /// Performs an Action on the UI Thread
        /// </summary>
        /// <param name="UIBinding">UI Context for Dispatching</param>
        /// <param name="action">Action to perform on the UI Thread</param>
        /// <returns>Continuation Task</returns>
        public static async Task OnUIThreadAsync(IUIBindingInfo UIBinding, Action action)
        {
            await UIBinding.ExecuteAsync(action);
        }

        /// <summary>
        /// Gets the Supported Settings Container, If GetLocal is false, it will attempt to get the Roaming Container if Supported, otherwise it will return the Local Settings Cluster.
        /// </summary>
        /// <param name="GetLocal">Get the Local Settings Container?</param>
        /// <returns>Local/Roaming Settings Container</returns>
        public static ISettingsContainer GetSettingsContainer(bool GetLocal)
        {
            return GetLocal || !AppServices.IO.SupportsRoaming ? AppServices.IO.GetLocalSettingsContainer() : AppServices.IO.GetRoamingSettingsContainer();
        }

        /// <summary>
        /// Determines the Type of a Generic, useful for performing actions dependent on Type.
        /// </summary>
        /// <typeparam name="T">Generic to Determine</typeparam>
        /// <returns>Type of Generic</returns>
        public static ObjectType DetermineGeneric<T>()
        {
            var generic = typeof(T);
            if (generic == typeof(string))
            {
                return ObjectType.String;
            }
            else if (generic == typeof(int))
            {
                return ObjectType.Int;
            }
            else if (generic == typeof(int?))
            {
                return ObjectType.NullableInt;
            }
            else if (generic == typeof(long))
            {
                return ObjectType.Long;
            }
            else if (generic == typeof(long?))
            {
                return ObjectType.NullableLong;
            }
            else if (generic == typeof(bool))
            {
                return ObjectType.Bool;
            }
            else if (generic == typeof(bool?))
            {
                return ObjectType.NullableBool;
            }
            else if (generic == typeof(float))
            {
                return ObjectType.Float;
            }
            else if (generic == typeof(float?))
            {
                return ObjectType.NullableFloat;
            }
            else
            {
                return ObjectType.ComplexObject;
            }
        }
    }
}