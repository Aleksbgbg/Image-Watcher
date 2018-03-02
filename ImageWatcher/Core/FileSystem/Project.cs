namespace ImageWatcher.Core.FileSystem
{
    using System;
    using System.IO;

    internal class Project : IDisposable
    {
        private readonly string _solutionName;

        private readonly string _name;

        private readonly ImageFolderWatcher _imageFolderWatcher;

        internal Project(string solutionName, string directory)
        {
            _solutionName = solutionName;
            _name = Path.GetFileName(directory);

            _imageFolderWatcher = new ImageFolderWatcher(_solutionName, _name, directory);

            Logger.Log($"Registered /{_solutionName}/{_name}");
        }

        ~Project()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        internal static bool IsValid(string directory)
        {
            return File.Exists(Path.Combine(directory, $"{Path.GetFileName(directory)}.csproj")) &&
                   Directory.Exists(Path.Combine(directory, "Images")) &&
                   File.Exists(Path.Combine(directory, "Themes", "Images.xaml"));
        }

        internal void Unregister()
        {
            Logger.Log($"Unregistered /{_solutionName}/{_name}");
            Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _imageFolderWatcher.Dispose();
            }
        }
    }
}