using Core.Operations.Operation;

namespace Core.Operations.Lyrics.Exceptions
{
    public class LyricsException : OperationException
    {
        protected LyricsException(string message) : base(message)
        {
        }
    }
}
