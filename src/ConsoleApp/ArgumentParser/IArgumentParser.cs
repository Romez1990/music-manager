using System.Collections.Generic;
using ConsoleApp.App;
using LanguageExt;

namespace ConsoleApp.ArgumentParser {
    public interface IArgumentParser {
        Either<ArgumentParserException, IAppOptions> Parse(IEnumerable<string> args, IAppOptions defaultAppOptions);
    }
}
