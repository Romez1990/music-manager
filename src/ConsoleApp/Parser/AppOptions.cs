using System.Collections.Immutable;
using Core.CoreEngine;
using Core.Operation;

namespace ConsoleApp.Parser
{
    public readonly struct AppOptions
    {
        public AppOptions(bool runGraphicalApp, string path, Mode mode, ImmutableArray<IOperation> operations)
        {
            RunGraphicalApp = runGraphicalApp;
            Path = path;
            Operations = operations;
            Mode = mode;
        }

        public readonly bool RunGraphicalApp;

        public readonly string Path;

        public readonly Mode Mode;

        public readonly ImmutableArray<IOperation> Operations;
    }
}
