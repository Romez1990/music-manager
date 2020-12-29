using System.Collections.Immutable;

namespace Core.Lyrics.Searcher.HttpResponses
{
    public class SearchResponse
    {
        public SearchResponse(ImmutableArray<Hit> hits)
        {
            Hits = hits;
        }

        public ImmutableArray<Hit> Hits { get; }
    }
}
