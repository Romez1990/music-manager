using Core.Lyrics.PageSearcher.Exceptions;
using LanguageExt;

namespace Core.Lyrics.PageSearcher {
    public interface ILyricsPageSearcher {
        EitherAsync<PageSearchException, string> SearchPageUrl(ITrack track);
    }
}
