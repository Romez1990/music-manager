using System;
using System.Collections.Generic;
using Core.CoreEngine;

namespace ConsoleApp.Application
{
    public interface IOptionsResolver
    {
        Mode ResolveMode(bool compilation, bool band, bool album);
        IEnumerable<Action> ResolveActions(bool rename, bool lyrics);
    }
}
