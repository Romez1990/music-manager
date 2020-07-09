using System;
using System.Collections.Immutable;
using System.Linq;
using ConsoleApp.ArgumentParser;
using ConsoleApp.FileSystemTree;
using Core.CoreEngine;
using Core.FileSystem;
using Core.Operation;

namespace ConsoleApp.Application
{
    public class App : IApp
    {
        public App(IEngine engine, IArgParser parser, IOperationRepository operationRepository,
            IFsTreePrinter fsTreePrinter, IGraphicalApp graphicalApp)
        {
            _engine = engine;
            _parser = parser;
            _operationRepository = operationRepository;
            _fsTreePrinter = fsTreePrinter;
            _graphicalApp = graphicalApp;
        }

        private readonly IEngine _engine;

        private readonly IArgParser _parser;

        private readonly IOperationRepository _operationRepository;

        private readonly IFsTreePrinter _fsTreePrinter;

        private readonly IGraphicalApp _graphicalApp;

        public int Run(string[] args)
        {
            if (args.Length == 0)
                return RunGraphicalApp();

            var appOptionsDefault = new AppOptionsDefault(
                () => Environment.CurrentDirectory,
                Mode.Band,
                () =>
                    _operationRepository
                        .FindAll()
                        .Select(o => o.Name)
                        .ToImmutableArray()
            );

            return _parser.Parse(args.ToImmutableArray(), appOptionsDefault)
                .Right(RunConsoleApp)
                .Left(e => 0);
        }

        private int RunConsoleApp(AppOptions options)
        {
            return _engine.SetDirectory(options.Path)
                .Right(engineScanner =>
                {
                    var enginePerformer = engineScanner.Scan(options.Mode);
                    PrintFileSystem(enginePerformer.DirectoryElement);
                    enginePerformer.PerformOperations(options.Operations);
                    return 0;
                })
                .Left(e =>
                {
                    Console.WriteLine($"Error: Directory {options.Path} not found");
                    return 1;
                });
        }

        private void PrintFileSystem(IDirectoryElement directoryElement)
        {
            var fsTree = _fsTreePrinter.Print(directoryElement);
            Console.WriteLine(fsTree);
        }

        private int RunGraphicalApp()
        {
            return _graphicalApp.Run();
        }
    }
}
