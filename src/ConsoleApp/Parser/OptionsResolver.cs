using Core.CoreEngine;

namespace ConsoleApp.Parser
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
