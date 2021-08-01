using LanguageExt;

namespace Core.Lyrics.TrackWriter {
    public interface ITrackLyricsWriter {
        Either<LyricsException, Unit> WriteLyrics(ITrack track, string lyrics);
    }
}
