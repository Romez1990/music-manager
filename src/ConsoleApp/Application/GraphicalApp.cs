using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace ConsoleApp.Application
{
    public class GraphicalApp
    {
        public int Run()
        {
            var graphicalAppFileInfo = GetGraphicalAppFileInfo();

            if (!graphicalAppFileInfo.Exists)
            {
                Console.WriteLine("Graphical app not found");
                return 1;
            }

            Process.Start(graphicalAppFileInfo.FullName);
            return 0;
        }

        private FileInfo GetGraphicalAppFileInfo()
        {
            var assemblyDirectoryPath = Assembly.GetExecutingAssembly().Location;
            var graphicalAppPath = Path.Combine(assemblyDirectoryPath, "MusicManager.exe");
            return new FileInfo(graphicalAppPath);
        }
    }
}
