using System.Collections.Generic;
using LanguageExt;

namespace Core.Lyrics {
    public interface ITrack {
        Option<IEnumerable<string>> Artists { get; }
        Option<string> Title { get; }
        string Lyrics { get; set; }
        void Save();
    }
}
