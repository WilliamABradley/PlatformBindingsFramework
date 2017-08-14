using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using PlatformBindings.Enums;
using PlatformBindings.Models.FileSystem;
using PlatformBindings.Services;

namespace PlatformBindings.Common
{
    public static class IOHelpers
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

        public static async Task<IReadOnlyList<IFileSystemContainer>> GetItems(this IFolderContainer Folder)
        {
            var folders = await Folder.GetFolders();
            var files = await Folder.GetFiles();

            var result = new List<IFileSystemContainer>();
            result.AddRange(folders);
            result.AddRange(files);
            return result;
        }

        public static List<string> GetPathPieces(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return new List<string>();

            path = path.Replace('/', '\\').TrimEnd('\\');
            return path.Split('\\').ToList();
        }

        public static byte[] GetByteArrayFromStream(Stream Stream)
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

        public static Task<bool> SaveJson<T>(this IFileContainer File, T Data)
        {
            var content = JsonConvert.SerializeObject(Data);
            return File.SaveText(content);
        }

        public static async Task<T> ReadAsJson<T>(this IFileContainer File)
        {
            var content = await File.ReadFileAsText();
            return JsonConvert.DeserializeObject<T>(content);
        }

        public static async Task<IFileContainer> CreateOrGetFile(this IFolderContainer Folder, string FileName)
        {
            try
            {
                return await Folder.GetFile(FileName);
            }
            catch
            {
                return await Folder.CreateFile(FileName);
            }
        }

        public static async Task<IFileContainer> CreateOrGetFile(this IOBindings IO, FilePath Path)
        {
            try
            {
                return await IO.GetFile(Path);
            }
            catch
            {
                return await IO.CreateFile(Path);
            }
        }

        public static async Task<FileQueryResult> CreateFileQueryAsync(this IFolderContainer Folder, QueryOptions Options)
        {
            FileQueryResult result = new FileQueryResult();
            foreach (var item in await Folder.GetItems())
            {
                if (item is IFolderContainer subFolder && Options.Depth == FolderDepth.Deep)
                {
                    var subresult = await CreateFileQueryAsync(subFolder, Options);
                    result.Files.AddRange(subresult.Files);
                }
                else if (item is IFileContainer file)
                {
                    var extension = Path.GetExtension(item.Path);
                    if (Options.FileTypes.FirstOrDefault(ext => ext.FileExtension == extension) != null)
                    {
                        result.Files.Add(file);
                    }
                }
            }

            return result;
        }
    }
}