namespace ImageWatcher.Core
{
    using System.IO;
    using System.Linq;

    internal class Solution
    {
        private readonly Project[] _projects;

        internal Solution(string path)
        {
            _projects = Directory.GetDirectories(path).Where(directory =>
            {
                string[] subDirectories = Directory.GetDirectories(directory);

                return Directory.GetFiles(directory).Any(file => Path.GetExtension(file) == ".csproj") &&
                       subDirectories.Contains("Images") &&
                       subDirectories.Contains("Themes") &&
                       File.Exists(Path.Combine(directory, "Themes", "Images.xaml"));
            }).Select(directory => new Project(directory)).ToArray();
        }
    }
}