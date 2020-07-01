using Core.CoreEngine;

namespace ConsoleApp.Application
{
    public class OptionsResolver : IOptionsResolver
    {
        public Mode ResolveMode(OptionsBase options)
        {
            if (options.Compilation)
                return Mode.Compilation;
            if (options.Band)
                return Mode.Band;
            return Mode.Album;
        }
    }
}
