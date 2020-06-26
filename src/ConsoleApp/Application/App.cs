using System;
using System.Collections.Generic;
using System.Diagnostics;
using CommandLine;
using ConsoleApp.FileSystemTree;
using Core.Engines;

namespace ConsoleApp.Application
{
    public class App : IApp
    {
        public App(IEngine engine, IOptionsResolver optionsResolver, IFsTreePrinter fsTreePrinter)
        {
            _engine = engine;
            _optionsResolver = optionsResolver;
            _fsTreePrinter = fsTreePrinter;
        }

        private readonly IEngine _engine;

        private readonly IOptionsResolver _optionsResolver;

        private readonly IFsTreePrinter _fsTreePrinter;

        private ParserResult<Options> _parserResult;

        private bool _runGraphicApp;

        public void Configure(string[] args)
        {
            _runGraphicApp = args.Length == 0;

            if (_runGraphicApp) return;

            _parserResult = Parser.Default.ParseArguments<Options>(args);
        }

        public int Run()
        {
            if (_runGraphicApp)
            {
                return RunGraphicApp();
            }

            return _parserResult.MapResult(RunWithOptions, HandleParsingErrors);
        }

        private int RunWithOptions(Options options)
        {
            var directoryPath = options.Path ?? Environment.CurrentDirectory;
            if (!_engine.SetDirectory(directoryPath))
            {
                return 1;
            }

            var mode = _optionsResolver.ResolveMode(options.Compilation, options.Band, options.Album);
            _engine.Scan(mode);

            PrintFileSystem();

            var actions = _optionsResolver.ResolveActions(options.Rename, options.Lyrics);
             _engine.PerformActions(actions);

            return 0;
        }

        private void PrintFileSystem()
        {
            var directoryElement = _engine.DirectoryElement;
            _fsTreePrinter.PrintTree(directoryElement);
        }

        private int HandleParsingErrors(IEnumerable<Error> errors)
        {
            return 1;
        }

        private int RunGraphicApp()
        {
            // get assembly directory
            Process.Start("MusicManager.exe");
            return 0;
        }
    }
}
