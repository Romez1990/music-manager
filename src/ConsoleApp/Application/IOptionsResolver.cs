using System;
using System.Collections.Generic;
using Core.CoreEngine;

namespace ConsoleApp.Application
{
    public interface IOptionsResolver
    {
        Mode ResolveMode(OptionsBase options);
    }
}
