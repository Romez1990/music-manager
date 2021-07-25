using System.Collections.Generic;
using Core.OperationEngine;

namespace ConsoleApp.App {
    public class AppOptions : IAppOptions {
        public AppOptions(string path, DirectoryMode directoryMode, IReadOnlyList<string> operations) {
            Path = path;
            DirectoryMode = directoryMode;
            Operations = operations;
        }

        public string Path { get; }
        public DirectoryMode DirectoryMode { get; }
        public IReadOnlyList<string> Operations { get; }
    }
}
