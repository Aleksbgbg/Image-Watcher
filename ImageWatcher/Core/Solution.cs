namespace ImageWatcher.Core
{
    using System.IO;
    using System.Linq;

    internal class Solution
    {
        private readonly Project[] _projects;

        internal Solution(string path)
        {
            _projects = Directory.GetDirectories(path)
                                 .Where(directory => File.Exists(Path.Combine(directory, $"{Path.GetFileName(directory)}.csproj")) &&
                                                     Directory.Exists(Path.Combine(directory, "Images")) &&
                                                     File.Exists(Path.Combine(directory, "Themes", "Images.xaml")))
                                 .Select(directory => new Project(directory))
                                 .ToArray();
        }
    }
}