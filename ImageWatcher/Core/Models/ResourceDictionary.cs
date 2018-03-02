namespace ImageWatcher.Core.Models
{
    using System.Collections.Generic;
    using System.IO;

    internal class ResourceDictionary : SortedDictionary<string, Image>
    {
        private readonly string _path;

        internal ResourceDictionary(string path)
        {
            _path = path;
        }

        internal void Save()
        {
            File.WriteAllText(_path, $@"<ResourceDictionary xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
                    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml"">
    {string.Join("\r\n\t", Values)}
</ResourceDictionary>");
        }
    }
}