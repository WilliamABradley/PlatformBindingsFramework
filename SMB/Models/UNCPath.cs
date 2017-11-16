using System;

namespace PlatformBindings.Models
{
    public class UNCPath
    {
        public UNCPath(string Path)
        {
            this.Path = Path;
        }

        public string GetURL()
        {
            var parts = Path.Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries);
            return $"smb://" + string.Join("/", parts) + "/";
        }

        public string Path { get; }
    }
}