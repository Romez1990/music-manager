using System.Collections.Generic;
using System.Linq;
using ConsoleApp.ArgumentParser;
using ConsoleApp.Env;
using ConsoleApp.FsTreeFormatter;
using ConsoleApp.Logger;
using Core.IocContainer;
using Core.Operation;
using Core.OperationEngine;
using static LanguageExt.Prelude;

namespace ConsoleApp.App {
    [Service]
    public class App : IApp {
        public App(IArgumentParser argumentParser, IEnv env, IOperationRepository operations,
            IOperationEngine operationEngine, IFsTreeFormatter fsTreeFormatter, ILogger logger) {
            _argumentParser = argumentParser;
            _env = env;
            _operations = operations;
            _defaultOptions = GetDefaultOptions();
            _operationEngine = operationEngine;
            _fsTreeFormatter = fsTreeFormatter;
            _logger = logger;
        }

        private readonly IArgumentParser _argumentParser;
        private readonly IEnv _env;
        private readonly IOperationRepository _operations;
        private readonly IAppOptions _defaultOptions;
        private readonly IOperationEngine _operationEngine;
        private readonly IFsTreeFormatter _fsTreeFormatter;
        private readonly ILogger _logger;

        private IAppOptions GetDefaultOptions() =>
            new LazyAppOptions(
                () => _env.CurrentDirectory,
                DirectoryMode.Band,
                () => _operations.FindAll()
                    .Map(operation => operation.Name)
                    .ToArray()
            );

        public int Run(IEnumerable<string> args) =>
            _argumentParser.Parse(args, _defaultOptions)
                .Right(RunEngine)
                .Left(constant<int, ArgumentParserException>(1));

        private int RunEngine(IAppOptions options) =>
            _operationEngine.SetDirectory(options.Path)
                .Match(
                    engineScanner => {
                        var enginePerformer = engineScanner.Scan(options.DirectoryMode);
                        var fsTree = _fsTreeFormatter.ToString(enginePerformer.Directory);
                        _logger.Info(fsTree);
                        var operationResult = enginePerformer.PerformOperations(options.Operations);
                        operationResult.Exceptions.ForEach(_logger.Error);
                        return 0;
                    },
                    e => {
                        _logger.Error(e);
                        return 1;
                    }
                );
    }
}
