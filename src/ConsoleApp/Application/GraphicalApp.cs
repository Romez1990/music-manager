using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace ConsoleApp.Application
{
    public class GraphicalApp : IGraphicalApp
    {
        public int Run()
        {
            var graphicalAppFileInfo = GetGraphicalAppFileInfo();

            if (!graphicalAppFileInfo.Exists)
            {
                Console.WriteLine("Error: Graphical app not found");
                return 1;
            }

            Process.Start(graphicalAppFileInfo.FullName);
            return 0;
        }

        private FileInfo GetGraphicalAppFileInfo()
        {
            var assemblyDirectoryPath = Assembly.GetExecutingAssembly().Location;
            var graphicalAppPath = Path.Combine(assemblyDirectoryPath, "Music Manager.exe");
            return new FileInfo(graphicalAppPath);
        }
    }
}
