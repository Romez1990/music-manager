using System.Collections.Generic;

namespace Core.Lyrics.PageSearcher.Exceptions {
    public sealed class MissingTagException : PageSearchException {
        public MissingTagException(IReadOnlyList<string> tags) {
            Tags = tags;
        }

        public IReadOnlyList<string> Tags;
    }
}
