using Core.IocContainer;
using LanguageExt;

namespace Core.Lyrics.TrackWriter {
    [Service]
    public class TrackLyricsWriter : ITrackLyricsWriter {
        public Either<LyricsException, Unit> WriteLyrics(ITrack track, string lyrics) {
            track.Lyrics = lyrics;
            track.Save();
            return Unit.Default;
        }
    }
}
