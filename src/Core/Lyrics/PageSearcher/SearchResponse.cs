using System.Collections.Generic;

namespace Core.Lyrics.PageSearcher {
    public record GeniusResponse<T>(T Response) where T : class;

    public record SearchResponse(IReadOnlyList<Hit> Hits);

    public record Hit(HitResult Result);

    public record HitResult(string Path);
}
