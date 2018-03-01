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
        private readonly ResourceDictionary _imageDictionary = new ResourceDictionary();

        internal Project(string directory)
        {
            XmlWriter imagesWriter = XmlWriter.Create(Path.Combine(directory, "Themes", "Images.xaml"));

            void SaveImageDictionary()
            {
                XamlWriter.Save(_imageDictionary, imagesWriter);
            }

            void AddNewImage(string fullpath)
            {
                string filename = Path.GetFileName(fullpath);

                _imageDictionary.Add(Path.GetFileNameWithoutExtension(filename),
                                     new BitmapImage(new Uri($"/Images/{filename}")));
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
                AddNewImage(e.FullPath);

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