namespace Core.Lyrics.Searcher.HttpResponses
{
    public class HitResult
    {
        public HitResult(string path)
        {
            Path = path;
        }

        public string Path { get; }
    }
}
