using System;
using System.Collections.Generic;
using Core.CoreEngine;

namespace ConsoleApp.Application
{
    public class OptionsResolver : IOptionsResolver
    {
        public Mode ResolveMode(bool compilation, bool band, bool album)
        {
            if (compilation)
                return Mode.Compilation;
            if (band)
                return Mode.Band;
            return Mode.Album;
        }
    }
}
