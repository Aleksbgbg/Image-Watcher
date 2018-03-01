namespace ImageWatcher
{
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using ImageWatcher.Core.FileSystem;

    internal static class Program
    {
        private static async Task Main()
        {
            Solution[] solutions = Directory.GetDirectories(@"E:\Programming\C#")
                                            .Where(directory => File.Exists(Path.Combine(directory, $"{Path.GetFileName(directory)}.sln")))
                                            .Select(directory => new Solution(directory))
                                            .ToArray();

            await Task.Delay(-1);
        }
    }
}