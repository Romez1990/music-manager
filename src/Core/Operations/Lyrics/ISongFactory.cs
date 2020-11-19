using Core.FileSystem;

namespace Core.Operations.Lyrics
{
    public interface ISongFactory
    {
        ISong CreateSong(IFileElement file);
    }
}
