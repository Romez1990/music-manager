using Core.FileSystem;

namespace Core.Lyrics.Exceptions
{
    public class LyricsNotFoundException : LyricsException
    {
        public LyricsNotFoundException(IFileElement file) : base($"Lyrics not found for {file.Name}")
        {
        }
    }
}
