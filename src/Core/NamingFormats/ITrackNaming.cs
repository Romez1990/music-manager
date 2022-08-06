using System.Collections.Generic;
using LanguageExt;

namespace Core.NamingFormats {
    public interface ITrackNaming {
        int FindPrefixLength(IEnumerable<string> trackNames);
        Option<string> Normalize(string trackName, int trackNumber, int trackNumberLength, int prefixLength);
    }
}
