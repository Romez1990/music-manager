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

        public IEnumerable<Action> ResolveActions(bool rename, bool lyrics)
        {
            var actions = new List<Action>();

            if (rename)
                actions.Add(Action.Rename);
            if (lyrics)
                actions.Add(Action.Lyrics);

            if (actions.Count != 0)
                return actions;

            return new[] {Action.Rename, Action.Lyrics};
        }
    }
}
