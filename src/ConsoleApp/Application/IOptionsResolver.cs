using System.Collections.Generic;
using Core.Actions;
using Core.Engines;

namespace ConsoleApp.Application
{
    public interface IOptionsResolver
    {
        Mode ResolveMode(bool compilation, bool band, bool album);
        IEnumerable<Action> ResolveActions(bool rename, bool lyrics);
    }
}
