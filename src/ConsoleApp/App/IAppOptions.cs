using System.Collections.Generic;
using Core.OperationEngine;

namespace ConsoleApp.App {
    public interface IAppOptions {
        string Path { get; }
        DirectoryMode DirectoryMode { get; }
        IReadOnlyList<string> Operations { get; }
    }
}
