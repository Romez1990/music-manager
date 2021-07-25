using System;
using System.Collections.Generic;
using Core.OperationEngine;

namespace ConsoleApp.App {
    public class LazyAppOptions : IAppOptions {
        public LazyAppOptions(Func<string> getPath, DirectoryMode directoryMode,
            Func<IReadOnlyList<string>> getOperations) {
            _path = new Lazy<string>(getPath);
            DirectoryMode = directoryMode;
            _operations = new Lazy<IReadOnlyList<string>>(getOperations);
        }

        private readonly Lazy<string> _path;
        private readonly Lazy<IReadOnlyList<string>> _operations;

        public string Path => _path.Value;
        public DirectoryMode DirectoryMode { get; }
        public IReadOnlyList<string> Operations => _operations.Value;
    }
}
