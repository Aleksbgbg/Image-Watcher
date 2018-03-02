namespace ImageWatcher.Core.Models
{
    internal class Image
    {
        internal Image(string key, string filename)
        {
            Key = key;
            Filename = filename;
        }

        internal string Key { get; set; }

        internal string Filename { get; set; }

        public override string ToString()
        {
            return $"<BitmapImage x:Key=\"{Key}\" UriSource=\"/Images/{Filename}\"/>";
        }
    }
}