namespace ImageWatcher.Core.FileSystem
{
    using System;
    using System.IO;

    using ImageWatcher.Core.Models;

    internal class Project
    {
        private readonly string _solutionName;

        private readonly string _name;

        private readonly ResourceDictionary _imageDictionary = new ResourceDictionary();

        internal Project(string solutionName, string directory)
        {
            _solutionName = solutionName;
            _name = Path.GetFileName(directory);

            void SaveImageDictionary()
            {
                // XamlWriter.Save(_imageDictionary, new FileStream(Path.Combine(directory, "Themes", "Images.xaml"), FileMode.Open));
            }

            void AddNewImage(string fullpath)
            {
                string filename = Path.GetFileName(fullpath);

                _imageDictionary.Add(Path.GetFileNameWithoutExtension(filename),
                                     new BitmapImage(new Uri($"/Images/{filename}", UriKind.Relative)));
            }

            string imagesDirectory = Path.Combine(directory, "Images");

            foreach (string image in Directory.GetFiles(imagesDirectory))
            {
                AddNewImage(image);
            }

            SaveImageDictionary();

            FileSystemWatcher projectWatcher = new FileSystemWatcher(imagesDirectory) { EnableRaisingEvents = true };
            
            projectWatcher.Created += (sender, e) =>
            {
                string filename = Path.GetFileName(e.FullPath);

                AddNewImage(filename);

                SaveImageDictionary();

                Logger.Log(_solutionName, _name, $"Added image: '{filename}'");
            };
            projectWatcher.Deleted += (sender, e) =>
            {
                string filename = Path.GetFileName(e.FullPath);

                _imageDictionary.Remove(Path.GetFileNameWithoutExtension(filename));

                SaveImageDictionary();

                Logger.Log(_solutionName, _name, $"Deleted image: '{filename}'");
            };
            projectWatcher.Renamed += (sender, e) =>
            {
                string oldFilename = Path.GetFileName(e.OldFullPath);
                string oldFilenameWithoutExtension = Path.GetFileNameWithoutExtension(oldFilename);
                string newFilename = Path.GetFileName(e.FullPath);

                BitmapImage image = (BitmapImage)_imageDictionary[oldFilenameWithoutExtension];
                image.UriSource = new Uri($"/Images/{newFilename}", UriKind.Relative);

                _imageDictionary.Remove(oldFilenameWithoutExtension);
                _imageDictionary.Add(Path.GetFileNameWithoutExtension(newFilename), image);

                SaveImageDictionary();

                Logger.Log(_solutionName, _name, $"Renamed image: '{oldFilename}' -> '{newFilename}");
            };

            Logger.Log($"Registered /{_solutionName}/{_name}");
        }
    }
}