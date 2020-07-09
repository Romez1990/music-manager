using System.Collections.Immutable;
using ConsoleApp.Application;
using LanguageExt;

namespace ConsoleApp.ArgumentParser
{
    public interface IArgParser
    {
        Either<ArgumentParserException, AppOptions> Parse(ImmutableArray<string> args,
            AppOptionsDefault appOptionsDefault);
    }
}
