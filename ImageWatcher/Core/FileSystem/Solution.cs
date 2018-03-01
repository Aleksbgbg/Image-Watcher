﻿namespace ImageWatcher.Core.FileSystem
{
    using System.IO;
    using System.Linq;

    internal class Solution
    {
        private readonly string _name;

        private readonly Project[] _projects;

        internal Solution(string path)
        {
            _name = Path.GetFileName(path);

            _projects = Directory.GetDirectories(path)
                                 .Where(directory => File.Exists(Path.Combine(directory, $"{Path.GetFileName(directory)}.csproj")) &&
                                                     Directory.Exists(Path.Combine(directory, "Images")) &&
                                                     File.Exists(Path.Combine(directory, "Themes", "Images.xaml")))
                                 .Select(directory => new Project(_name, directory))
                                 .ToArray();
        }
    }
}