namespace ImageWatcher.Core.Models
{
    internal class Image
    {
        internal Image(string key, string filename)
        {
            Key = key;
            Filename = filename;
        }

        private string _key;
        internal string Key
        {
            get => _key;

            set => _key = value.Replace(" ", string.Empty);
        }

        internal string Filename { get; set; }

        public override string ToString()
        {
            return $"<BitmapImage x:Key=\"{Key}\" UriSource=\"/Images/{Filename}\"/>";
        }
    }
}