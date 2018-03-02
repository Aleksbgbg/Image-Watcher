namespace ImageWatcher.Core.FileSystem
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
                                 .Where(Project.IsValid)
                                 .Select(directory => new Project(_name, directory))
                                 .ToArray();
        }
    }
}