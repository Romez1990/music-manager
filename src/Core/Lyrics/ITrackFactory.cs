using Core.FileSystemElement;

namespace Core.Lyrics {
    public interface ITrackFactory {
        ITrack CreateTrack(IFileElement file);
    }
}
