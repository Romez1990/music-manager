using Core.FileSystemElement;
using Core.IocContainer;

namespace Core.Lyrics {
    [Service]
    public class TrackFactory : ITrackFactory {
        public ITrack CreateTrack(IFileElement file) =>
            new Track(file);
    }
}
