namespace ImageWatcher.Core.FileSystem
{
    using System;
    using System.IO;
    using System.Linq;

    internal class Solution : IDisposable
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

        ~Solution()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            foreach (Project project in _projects)
            {
                project.Dispose();
            }
        }
    }
}