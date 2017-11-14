using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace PlatformBindings.Models.FileSystem
{
    /// <summary>
    /// The File Wrapper for PlatformBindings, use this for File handling on each platform
    /// </summary>
    public abstract class FileContainer : FileSystemContainer
    {
        /// <summary>
        /// Opens the File as a Stream for Reading or Writing.
        /// </summary>
        /// <param name="CanWrite"></param>
        /// <returns>A Filestream for Manipulation.</returns>
        public abstract Task<Stream> OpenAsStream(bool CanWrite);

        /// <summary>
        /// Returns the contents of the file as a String.
        /// </summary>
        /// <returns></returns>
        public abstract Task<string> ReadFileAsText();

        /// <summary>
        /// Deserialises an object from the file text, via JSON Parsing.
        /// </summary>
        /// <typeparam name="T">Type of object to Deserialise.</typeparam>
        /// <returns>Operation Success</returns>
        public async Task<T> ReadAsJson<T>()
        {
            var content = await ReadFileAsText();
            return JsonConvert.DeserializeObject<T>(content);
        }

        /// <summary>
        /// Saves the contents of the file to a string.
        /// </summary>
        /// <param name="Text">Content to Save.</param>
        /// <returns></returns>
        public abstract Task<bool> SaveText(string Text);

        /// <summary>
        /// Serialises the object as Json, then saves it to the file.
        /// </summary>
        /// <typeparam name="T">Type of object for serialisation.</typeparam>
        /// <param name="Data">Object for serialisation.</param>
        /// <returns>Operation Success</returns>
        public Task<bool> SaveJson<T>(T Data)
        {
            var content = JsonConvert.SerializeObject(Data);
            return SaveText(content);
        }
    }
}