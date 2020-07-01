using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;
using ConsoleApp.FileSystemTree;
using Core.CoreEngine;
using Core.FileSystem;

namespace ConsoleApp.Application
{
    public class App : IApp
    {
        public App(IEngine engine, IOptionsResolver optionsResolver, IFsTree fsTree)
        {
            Console.OutputEncoding = Encoding.UTF8;
            _engine = engine;
            _optionsResolver = optionsResolver;
            _fsTree = fsTree;
        }

        private readonly IEngine _engine;

        private readonly IOptionsResolver _optionsResolver;

        private readonly IFsTree _fsTree;

        private ParserResult<OptionsBase> _parserResult;

        private bool _runGraphicApp;

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

            return _parserResult.MapResult(RunWithOptions, HandleParsingErrors);
        }

        private int RunWithOptions(OptionsBase optionsBase)
        {
            var directoryPath = optionsBase.Path ?? Environment.CurrentDirectory;
            if (!_engine.SetDirectory(directoryPath))
            {
                return 1;
            }

            var mode = _optionsResolver.ResolveMode(optionsBase);
            _engine.Scan(mode);

            PrintFileSystem(_engine.DirectoryElement);

            // var actions = _optionsResolver.ResolveActions(options.Rename, options.Lyrics);
            // _engine.PerformActions(actions);

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
