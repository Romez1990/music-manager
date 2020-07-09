using System.Collections.Immutable;
using Core.CoreEngine;

namespace ConsoleApp.Application
{
    public readonly struct AppOptions
    {
        public AppOptions(string path, Mode mode, ImmutableArray<string> operations)
        {
            Path = path;
            Mode = mode;
            Operations = operations;
        }

        public readonly string Path;

        public readonly Mode Mode;

        public readonly ImmutableArray<string> Operations;
    }
}
