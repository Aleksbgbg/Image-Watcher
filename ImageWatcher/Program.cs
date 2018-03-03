namespace ImageWatcher
{
    using System;
    using System.Threading.Tasks;

    using ImageWatcher.Core.FileSystem;

    internal static class Program
    {
        private static async Task Main()
        {
            RootDirectoryManager rootDirectoryManager = new RootDirectoryManager(AppDomain.CurrentDomain.BaseDirectory);

            await Task.Delay(-1);
        }
    }
}