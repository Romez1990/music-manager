using Core.Operations;

namespace Core.Lyrics.Exceptions
{
    public class LyricsException : OperationException
    {
        protected LyricsException(string message) : base(message)
        {
        }
    }
}
