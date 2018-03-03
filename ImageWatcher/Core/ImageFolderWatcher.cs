namespace ImageWatcher.Core
{
    using System;
    using System.IO;

    using ImageWatcher.Core.Models;

    internal sealed class ImageFolderWatcher : IDisposable
    {
        private readonly string _solutionName;

        private readonly string _projectName;

        private readonly ResourceDictionary _imageDictionary;

        private readonly FileSystemWatcher _projectWatcher;

        internal ImageFolderWatcher(string solutionName, string projectName, string directory)
        {
            _solutionName = solutionName;
            _projectName = projectName;
            _imageDictionary = new ResourceDictionary(Path.Combine(directory, "Themes", "Images.xaml"));

            void SaveImageDictionary()
            {
                _imageDictionary.Save();
            }

            void AddNewImage(string fullpath)
            {
                // Do not include 'Icon.ico' files, as these are not used in project
                // Consider adding a rules file to make this flexible
                if (Path.GetFileName(fullpath) == "Icon.ico") return;

                string filename = Path.GetFileName(fullpath);
                string key = Path.GetFileNameWithoutExtension(filename);

                _imageDictionary.Add(key, new Image(key, filename));
            }

            string imagesDirectory = Path.Combine(directory, "Images");

            foreach (string image in Directory.GetFiles(imagesDirectory))
            {
                AddNewImage(image);
            }

            SaveImageDictionary();

            _projectWatcher = new FileSystemWatcher(imagesDirectory) { EnableRaisingEvents = true };

            _projectWatcher.Created += (sender, e) =>
            {
                string filename = Path.GetFileName(e.FullPath);

                AddNewImage(filename);

                SaveImageDictionary();

                Logger.Log(_solutionName, _projectName, $"Added image: '{filename}'");
            };
            _projectWatcher.Deleted += (sender, e) =>
            {
                string filename = Path.GetFileName(e.FullPath);

                _imageDictionary.Remove(Path.GetFileNameWithoutExtension(filename));

                SaveImageDictionary();

                Logger.Log(_solutionName, _projectName, $"Deleted image: '{filename}'");
            };
            _projectWatcher.Renamed += (sender, e) =>
            {
                string oldFilename = Path.GetFileName(e.OldFullPath);
                string oldFilenameWithoutExtension = Path.GetFileNameWithoutExtension(oldFilename);
                string newFilename = Path.GetFileName(e.FullPath);
                string newFilenameWithoutExtension = Path.GetFileNameWithoutExtension(newFilename);

                Image image = _imageDictionary[oldFilenameWithoutExtension];
                image.Key = newFilenameWithoutExtension;
                image.Filename = newFilename;

                _imageDictionary.Remove(oldFilenameWithoutExtension);
                _imageDictionary.Add(newFilenameWithoutExtension, image);

                SaveImageDictionary();

                Logger.Log(_solutionName, _projectName, $"Renamed image: '{oldFilename}' -> '{newFilename}'");
            };
        }

        ~ImageFolderWatcher()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _projectWatcher.Dispose();
            }
        }
    }
}