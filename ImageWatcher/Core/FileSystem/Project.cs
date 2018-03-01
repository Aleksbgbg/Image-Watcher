namespace ImageWatcher.Core.FileSystem
{
    using System;
    using System.IO;
    using System.Windows;
    using System.Windows.Markup;
    using System.Windows.Media.Imaging;
    using System.Xml;

    internal class Project
    {
        private readonly ResourceDictionary _imageDictionary;

        private readonly XmlWriter _imagesWriter;

        internal Project(string directory)
        {
            _imagesWriter = XmlWriter.Create(Path.Combine(directory, "Themes", "Images.xaml"));

            void SaveImageDictionary()
            {
                XamlWriter.Save(_imageDictionary, _imagesWriter);
            }

            FileSystemWatcher projectWatcher = new FileSystemWatcher(Path.Combine(directory, "Images")) { EnableRaisingEvents = true };

            projectWatcher.Created += (sender, e) =>
            {
                string filename = Path.GetFileName(e.FullPath);

                _imageDictionary.Add(Path.GetFileNameWithoutExtension(filename),
                                     new BitmapImage(new Uri($"/Images/{filename}")));

                SaveImageDictionary();
            };
            projectWatcher.Deleted += (sender, e) =>
            {
                _imageDictionary.Remove(Path.GetFileNameWithoutExtension(e.FullPath));

                SaveImageDictionary();
            };
            projectWatcher.Renamed += (sender, e) =>
            {
                string oldFilename = Path.GetFileNameWithoutExtension(e.OldFullPath);
                string newFilename = Path.GetFileName(e.FullPath);

                BitmapImage image = (BitmapImage)_imageDictionary[oldFilename];
                image.UriSource = new Uri($"/Images/{newFilename}");

                _imageDictionary.Remove(oldFilename);
                _imageDictionary.Add(Path.GetFileNameWithoutExtension(newFilename), image);

                SaveImageDictionary();
            };
        }
    }
}