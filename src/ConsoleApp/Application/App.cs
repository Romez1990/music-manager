using System.Collections.Immutable;
using ConsoleApp.ArgumentParser;
using ConsoleApp.FileSystemTree;
using Core.CoreEngine;
using Core.FileSystem;
using Core.Operations;

namespace ConsoleApp.Application
{
    public class App : IApp
    {
        public App(IEngine engine, IArgParser parser, IOperationRepository operationRepository,
            IFsTreePrinter fsTreePrinter, IConsole console, IEnvironment environment, IGraphicalApp graphicalApp)
        {
            _engine = engine;
            _parser = parser;
            _operationRepository = operationRepository;
            _fsTreePrinter = fsTreePrinter;
            _console = console;
            _environment = environment;
            _graphicalApp = graphicalApp;
        }

        private readonly IEngine _engine;
        private readonly IArgParser _parser;
        private readonly IOperationRepository _operationRepository;
        private readonly IFsTreePrinter _fsTreePrinter;
        private readonly IConsole _console;
        private readonly IEnvironment _environment;
        private readonly IGraphicalApp _graphicalApp;

        public int Run(string[] args)
        {
            if (args.Length == 0)
                return RunGraphicalApp();

            var appOptionsDefault = new AppOptionsDefault(
                () => _environment.CurrentDirectory,
                Mode.Band,
                () =>
                    _operationRepository
                        .FindAll()
                        .Map(o => o.Name)
                        .ToImmutableArray()
            );

            return _parser.Parse(args.ToImmutableArray(), appOptionsDefault)
                .Right(RunConsoleApp)
                .Left(_ => 0);
        }

        private int RunConsoleApp(AppOptions options) =>
            _engine.SetDirectory(options.Path)
                .Right(engineScanner =>
                {
                    var enginePerformer = engineScanner.Scan(options.Mode);
                    PrintFileSystem(enginePerformer.Directory);
                    var operationResult = enginePerformer.PerformOperations(options.Operations);
                    foreach (var e in operationResult.Exceptions)
                        _console.Error(e);
                    return 0;
                })
                .Left(e =>
                {
                    _console.Error(e);
                    return 1;
                });

        private void PrintFileSystem(IDirectoryElement directory)
        {
            var fsTree = _fsTreePrinter.Print(directory);
            _console.Print(fsTree);
        }

        private int RunGraphicalApp() =>
            _graphicalApp.Run();
    }
}
