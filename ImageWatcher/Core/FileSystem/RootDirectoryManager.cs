namespace ImageWatcher.Core.FileSystem
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    internal class RootDirectoryManager : IDisposable
    {
        private readonly List<Solution> _solutions;

        private readonly FileSystemWatcher _rootDirectoryWatcher;

        internal RootDirectoryManager(string path)
        {
            _solutions = Directory.GetDirectories(path)
                                  .Where(Solution.IsValid)
                                  .Select(directory => new Solution(directory))
                                  .ToList();

            _rootDirectoryWatcher = new FileSystemWatcher(path) { EnableRaisingEvents = true };

            _rootDirectoryWatcher.Created += (sender, e) =>
            {
                if (Directory.Exists(e.FullPath) && Solution.IsValid(e.FullPath))
                {
                    _solutions.Add(new Solution(e.FullPath));
                }
            };
            _rootDirectoryWatcher.Deleted += (sender, e) =>
            {
                string name = Path.GetFileName(e.FullPath);

                Solution firstMatch = _solutions.FirstOrDefault(solution => solution.Name == name);

                if (firstMatch == null) return;

                _solutions.Remove(firstMatch);
                firstMatch.Dispose();
            };
        }

        ~RootDirectoryManager()
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

            _rootDirectoryWatcher.Dispose();

            foreach (Solution solution in _solutions)
            {
                solution.Dispose();
            }

            _solutions.Clear();
        }
    }
}