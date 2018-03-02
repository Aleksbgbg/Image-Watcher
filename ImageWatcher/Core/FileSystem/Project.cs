namespace ImageWatcher.Core.FileSystem
{
    using System;
    using System.IO;

    using IODirectory = System.IO.Directory;

    internal class Project : IDisposable
    {
        private readonly string _solutionName;

        private readonly string _name;

        private readonly ImageFolderWatcher _imageFolderWatcher;

        private readonly FileSystemWatcher _folderValidityWatcher;

        private readonly FileSystemWatcher _themesValidityWatcher;

        internal Project(string solutionName, string directory)
        {
            _solutionName = solutionName;
            _name = Path.GetFileName(directory);
            _imageFolderWatcher = new ImageFolderWatcher(_solutionName, _name, directory);

            Directory = directory;

            _folderValidityWatcher = new FileSystemWatcher(directory) { EnableRaisingEvents = true };
            _themesValidityWatcher = new FileSystemWatcher(Path.Combine(directory, "Themes"), "Images.xaml") { EnableRaisingEvents = true };

            void UnregisterIfInvalid()
            {
                if (!IsValid(directory))
                {
                    Unregister();
                }
            }

            void WatcherDeleted(object sender, FileSystemEventArgs e)
            {
                UnregisterIfInvalid();
            }

            void WatcherRenamed(object sender, RenamedEventArgs e)
            {
                UnregisterIfInvalid();
            }

            _folderValidityWatcher.Deleted += WatcherDeleted;
            _folderValidityWatcher.Renamed += WatcherRenamed;

            _themesValidityWatcher.Deleted += WatcherDeleted;
            _themesValidityWatcher.Renamed += WatcherRenamed;

            Logger.Log($"Registered /{_solutionName}/{_name}");
        }

        ~Project()
        {
            Dispose(false);
        }

        internal event EventHandler Unregistered;

        internal string Directory { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal static bool IsValid(string directory)
        {
            return File.Exists(Path.Combine(directory, $"{Path.GetFileName(directory)}.csproj")) &&
                   IODirectory.Exists(Path.Combine(directory, "Images")) &&
                   File.Exists(Path.Combine(directory, "Themes", "Images.xaml"));
        }

        internal void Unregister()
        {
            Logger.Log($"Unregistered /{_solutionName}/{_name}");
            Dispose();
            Unregistered?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _imageFolderWatcher.Dispose();
                _folderValidityWatcher.Dispose();
                _themesValidityWatcher.Dispose();
            }
        }
    }
}