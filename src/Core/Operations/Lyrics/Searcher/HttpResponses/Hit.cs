namespace Core.Operations.Lyrics.Searcher.HttpResponses
{
    public class Hit
    {
        public Hit(HitResult result)
        {
            Result = result;
        }

        public HitResult Result { get; }
    }
}
