using System.Collections.Generic;
using System.Linq;
using Core.Configuration;
using Core.FileSystem;
using Core.HttpClient;
using Core.Operations.Lyrics.Exceptions;
using Core.Operations.Lyrics.Searcher.HttpResponses;
using LanguageExt;

namespace Core.Operations.Lyrics.Searcher
{
    public class LyricsSearcher : ILyricsSearcher
    {
        public LyricsSearcher(IHttp http, IConfig config)
        {
            _http = http;
            _config = config;
        }

        private readonly IHttp _http;
        private readonly IConfig _config;

        public EitherAsync<LyricsException, string> GetLyricsLink(IFileElement file, string searchQuery) =>
            SendRequest(file, searchQuery)
                .MapLeft(e => (LyricsException)e)
                .Bind(response => GetLyricsLinkFromResponse(file, response)
                    .MapLeft(e => (LyricsException)e)
                    .ToAsync())
                .Map(MakeFullUrl);

        private EitherAsync<LyricsNotFoundException, HttpResponse<SearchResponse>> SendRequest(IFileElement file,
            string searchQuery) =>
            _http.Get<HttpResponse<SearchResponse>>("https://api.genius.com/search", new Dictionary<string, string>
                {
                    {"access_token", _config.GetString("GeniusApiToken")},
                    {"q", searchQuery},
                })
                .MapLeft(_ => new LyricsNotFoundException(file));

        private Either<LyricsNotFoundException, string> GetLyricsLinkFromResponse(IFileElement file,
            HttpResponse<SearchResponse> response)
        {
            var hits = response.Response.Hits;
            if (hits.IsEmpty) return new LyricsNotFoundException(file);
            var hit = hits.First();
            return hit.Result.Path;
        }

        private string MakeFullUrl(string path) =>
            $"https://genius.com{path}";
    }
}
