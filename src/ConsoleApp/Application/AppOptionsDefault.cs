using System;
using System.Collections.Immutable;
using Core.CoreEngine;

namespace ConsoleApp.Application
{
    public readonly struct AppOptionsDefault
    {
        public AppOptionsDefault(Func<string> path, Mode mode, Func<ImmutableArray<string>> operations)
        {
            Path = new Lazy<string>(path);
            Mode = mode;
            Operations = new Lazy<ImmutableArray<string>>(operations);
        }

        public readonly Lazy<string> Path;

        public readonly Mode Mode;

        public readonly Lazy<ImmutableArray<string>> Operations;
    }
}
