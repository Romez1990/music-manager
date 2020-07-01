using Core.CoreEngine;

namespace ConsoleApp.Parser
{
    public interface IOptionsResolver
    {
        Mode ResolveMode(OptionsBase options);
    }
}
