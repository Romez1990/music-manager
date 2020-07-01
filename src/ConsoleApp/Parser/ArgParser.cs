using System.Collections.Immutable;
using Core.CoreEngine;
using Core.Operation;

namespace ConsoleApp.Parser
{
    public class ArgParser
    {
        public ArgParser(ImmutableArray<string> args)
        {
            if (args.Length != 0)
                (_areArgsCorrect, _appOptions) = ConfigureConsoleApp(args);

            _appOptions = ConfigureGraphicalApp();
        }

        private readonly AppOptions _appOptions;

        private readonly bool _areArgsCorrect;
        private (bool, AppOptions) ConfigureConsoleApp(ImmutableArray<string> args)
        {
            var options = new AppOptions(
                false,
                null,
                Mode.Album,
                new ImmutableArray<IOperation>()
            );
        }

        private AppOptions ConfigureGraphicalApp()
        {
            return new AppOptions(
                true,
                null,
                Mode.Album,
                new ImmutableArray<IOperation>()
            );
        }

        public int Run()
        {
            
        }
    }
}
