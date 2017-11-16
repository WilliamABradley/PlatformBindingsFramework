using System;

namespace PlatformBindings.Exceptions
{
    public class DefaultAppNotFoundException : Exception
    {
        public DefaultAppNotFoundException(string FileName)
        {
            this.FileName = FileName;
            Extension = System.IO.Path.GetExtension(FileName);
        }

        public override string Message => $"No App was installed that could handle the File Type: {Extension}";

        public string FileName { get; }
        public string Extension { get; }
    }
}