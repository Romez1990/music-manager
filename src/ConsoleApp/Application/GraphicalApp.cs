using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace ConsoleApp.Application
{
    public class GraphicalApp : IGraphicalApp
    {
        public GraphicalApp(IConsole console)
        {
            _console = console;
        }

        private readonly IConsole _console;

        public int Run()
        {
            var graphicalAppFileInfo = GetGraphicalAppFileInfo();

            if (!graphicalAppFileInfo.Exists)
            {
                _console.Print("Error: Graphical app not found");
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
