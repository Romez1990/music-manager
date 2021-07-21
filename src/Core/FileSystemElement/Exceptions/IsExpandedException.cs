using System;

namespace Core.FileSystemElement.Exceptions {
    public sealed class IsExpandedException : Exception {
        public IsExpandedException(bool isExpanded) : base($"Fs node is already {GetVerb(isExpanded)}") { }

        private static string GetVerb(bool isExpanded) =>
            isExpanded ? "expanded" : "collapsed";
    }
}
