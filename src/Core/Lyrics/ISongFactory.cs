using Core.FileSystem;

namespace Core.Lyrics
{
    public interface ISongFactory
    {
        ISong CreateSong(IFileElement file);
    }
}
