namespace ImageWatcher.Core.Models
{
    using System.IO;

    internal class Image
    {
        private string _resourceString;

        internal Image(string filepath)
        {
            GenerateResourceString(filepath);
        }

        public override string ToString()
        {
            return _resourceString;
        }

        internal void Rename(string newpath)
        {
            GenerateResourceString(newpath);
        }

        private void GenerateResourceString(string filepath)
        {
            string filename = Path.GetFileName(filepath);

            _resourceString = $"<BitmapImage x:Key=\"{Path.GetFileNameWithoutExtension(filename)}\" UriSource=\"/Images/{filename}\"/>";
        }
    }
}