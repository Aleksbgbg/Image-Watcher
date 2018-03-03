namespace ImageWatcher.Core.FileSystem
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    internal class Solution : IDisposable
    {
        private readonly FileSystemWatcher _solutionWatcher;

        private readonly List<Project> _projects;

        internal Solution(string path)
        {
            Name = Path.GetFileName(path);

            _projects = Directory.GetDirectories(path)
                                 .Where(Project.IsValid)
                                 .Select(directory => new Project(Name, directory))
                                 .ToList();

            _solutionWatcher = new FileSystemWatcher(path) { EnableRaisingEvents = true };

            _solutionWatcher.Created += (sender, e) =>
            {
                if (Directory.Exists(e.FullPath) && Project.IsValid(e.FullPath))
                {
                    _projects.Add(new Project(Name, e.FullPath));
                }
            };
            _solutionWatcher.Deleted += (sender, e) =>
            {
                if (Path.GetFileName(e.FullPath) == $"{Name}.sln")
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

        internal string Name { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal static bool IsValid(string path)
        {
            return File.Exists(Path.Combine(path, $"{Path.GetFileName(path)}.sln"));
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            _solutionWatcher.Dispose();

            foreach (Project project in _projects)
            {
                project.Unregister();
            }

            _projects.Clear();
        }
    }
}