using System.Collections.Generic;
using Core.Engines;
using Core.FileSystem;
using Core.Renamers;

namespace Core.Actions
{
    public class ActionsMapper : IActionsMapper
    {
        private readonly Dictionary<Action, IAction> _dictionary = new Dictionary<Action, IAction>
        {
            {Action.Rename, new Renamer()},
            {Action.Lyrics, null},
        };

        public void Perform(Action action, IDirectoryElement directoryElement, Mode mode)
        {
            var iAction = _dictionary[action];
            iAction.Perform(directoryElement, mode);
        }
    }
}
