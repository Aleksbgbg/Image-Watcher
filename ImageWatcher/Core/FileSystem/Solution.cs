namespace ImageWatcher.Core.FileSystem
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    internal class Solution : IDisposable
    {
        private readonly string _name;

        private readonly FileSystemWatcher _solutionWatcher;

        private readonly List<Project> _projects;

        internal Solution(string path)
        {
            _name = Path.GetFileName(path);

            _projects = Directory.GetDirectories(path)
                                 .Where(Project.IsValid)
                                 .Select(directory => new Project(_name, directory))
                                 .ToList();

            _solutionWatcher = new FileSystemWatcher(path) { EnableRaisingEvents = true };

            _solutionWatcher.Created += (sender, e) =>
            {
                if (Directory.Exists(e.FullPath) && Project.IsValid(e.FullPath))
                {
                    _projects.Add(new Project(_name, e.FullPath));
                }
            };
            _solutionWatcher.Deleted += (sender, e) =>
            {
                if (Path.GetFileName(e.FullPath) == $"{_name}.sln")
                {
                    Dispose();
                    return;
                }

                Project firstMatch = _projects.FirstOrDefault(project => project.Directory == e.FullPath);

                if (firstMatch == null) return;

                firstMatch.Unregister();
                _projects.Remove(firstMatch);
            };
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

            _solutionWatcher.Dispose();

            foreach (Project project in _projects)
            {
                project.Unregister();
            }
        }
    }
}