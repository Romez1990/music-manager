using Core.FileSystem;

namespace Core.Operations.Lyrics.Exceptions
{
    public class MissingTagsException : LyricsException
    {
        public MissingTagsException(IFileElement file) : base($"Missing tags in file {file.Name}")
        {
            File = file;
        }

        public IFileElement File { get; }
    }
}
