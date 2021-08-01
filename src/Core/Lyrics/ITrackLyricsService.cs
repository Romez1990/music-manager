using Core.FileSystemElement;
using LanguageExt;

namespace Core.Lyrics {
    public interface ITrackLyricsService {
        EitherAsync<LyricsException, Unit> AddLyrics(IFileElement file);
    }
}
