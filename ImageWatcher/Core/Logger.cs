namespace ImageWatcher.Core
{
    using System;

    using static System.Console;

    internal static class Logger
    {
        internal static void Log(string message)
        {
            WriteLine("[{0}] {1}\n", DateTime.Now, message);
        }

        internal static void Log(string solutionName, string projectName, string message)
        {
            WriteLine("[{0}] /{1}/{2}\n{{\n\t{3}\n}}\n", DateTime.Now, solutionName, projectName, message);
        }
    }
}