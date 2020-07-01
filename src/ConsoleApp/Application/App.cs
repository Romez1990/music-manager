using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;
using ConsoleApp.FileSystemTree;
using ConsoleApp.Parser;
using Core.CoreEngine;
using Core.FileSystem;

namespace ConsoleApp.Application
{
    public class App : IApp
    {
        public App(IEngine engine, IFsTree fsTree)
        {
            Console.OutputEncoding = Encoding.UTF8;
            _engine = engine;
            _fsTree = fsTree;
        }

        private readonly IEngine _engine;

        private readonly IFsTree _fsTree;

        public void Configure(string[] args)
        {
            _runGraphicApp = args.Length == 0;

            if (_runGraphicApp) return;

            _parserResult = Parser.Default.ParseArguments<OptionsBase>(args);
        }

        public int Run()
        {

            if (_runGraphicApp)
            {
                return RunGraphicalApp();
            }

            return _parserResult.MapResult(RunConsoleApp, HandleParsingErrors);
        }

        private int RunConsoleApp(AppOptions options)
        {
            var directoryPath = options.Path ?? Environment.CurrentDirectory;
            if (!_engine.SetDirectory(directoryPath))
            {
                return 1;
            }

            _engine.Scan(options.Mode);
            PrintFileSystem(_engine.DirectoryElement);
            _engine.PerformOperations(options.Operations);
            return 0;
        }

        private void PrintFileSystem(IDirectoryElement directoryElement)
        {
            _fsTree.DirectoryElement = directoryElement;
            Console.WriteLine(_fsTree);
        }

        private int HandleParsingErrors(IEnumerable<Error> errors)
        {
            return 1;
        }

        private int RunGraphicalApp()
        {
            var graphicalApp = new GraphicalApp();
            return graphicalApp.Run();
        }
    }
}
