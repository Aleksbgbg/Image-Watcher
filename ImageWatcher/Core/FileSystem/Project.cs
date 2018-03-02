namespace ImageWatcher.Core.FileSystem
{
    using System.IO;

    internal class Project
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
    }
}