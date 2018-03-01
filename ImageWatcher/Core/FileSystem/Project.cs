namespace ImageWatcher.Core.FileSystem
{
    using System.IO;

    internal class Project
    {
        internal Project(string directory)
        {
            void OnImagesModified()
            {
            }

            FileSystemWatcher projectWatcher = new FileSystemWatcher(Path.Combine(directory, "Images")) { EnableRaisingEvents = true };

            projectWatcher.Created += (sender, e) => OnImagesModified();
            projectWatcher.Deleted += (sender, e) => OnImagesModified();
            projectWatcher.Renamed += (sender, e) => OnImagesModified();
        }
    }
}