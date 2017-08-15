using System;
using System.Threading.Tasks;
using PlatformBindings.Enums;
using PlatformBindings.Models.Settings;
using PlatformBindings.Services;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using PlatformBindings.Models.FileSystem;

namespace PlatformBindings.Common
{
    public static class PlatformBindingHelpers
    {
        public static string ResolvePath(FolderPath Path)
        {
            char separator = System.IO.Path.DirectorySeparatorChar;

            var path = AppServices.Services.IO.GetBaseFolder(Path.Root).Path;

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

        public static List<string> GetPathPieces(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return new List<string>();

            path = path.Replace('/', '\\').TrimEnd('\\');
            return path.Split('\\').ToList();
        }

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

        public static async void OnUIThread(Action action)
        {
            await OnUIThreadAsync(action);
        }

        public static async Task OnUIThreadAsync(Action action)
        {
            await AppServices.Services.UI.DefaultUIBinding.ExecuteAsync(action);
        }

        public static void OnUIThread(IUIBindingInfo UIBinding, Action action)
        {
            UIBinding.Execute(action);
        }

        public static async Task OnUIThreadAsync(IUIBindingInfo UIBinding, Action action)
        {
            await UIBinding.ExecuteAsync(action);
        }

        public static ISettingsContainer GetSettingsContainer(bool GetLocal)
        {
            return GetLocal || !AppServices.Services.IO.SupportsRoaming ? AppServices.Services.IO.GetLocalSettingsContainer() : AppServices.Services.IO.GetRoamingSettingsContainer();
        }

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