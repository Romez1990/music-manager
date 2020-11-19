using System.Collections.Generic;
using System.Linq;
using Core.Configuration;
using Core.HttpClient;
using Core.Operations.Lyrics.Searcher.HttpResponses;
using LanguageExt;
using static LanguageExt.Prelude;

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

        public OptionAsync<string> GetLyricsLink(string searchQuery) =>
            SendRequest(searchQuery)
                .ToOption()
                .Bind(response => GetLyricsLinkFromResponse(response).ToAsync())
                .Map(MakeFullUrl);

        private EitherAsync<RequestException, HttpResponse<SearchResponse>> SendRequest(string searchQuery) =>
            _http.Get<HttpResponse<SearchResponse>>("https://api.genius.com/search", new Dictionary<string, string>
            {
                {"access_token", _config.GetString("GeniusApiToken")},
                {"q", searchQuery},
            });

        private Option<string> GetLyricsLinkFromResponse(HttpResponse<SearchResponse> response)
        {
            var hits = response.Response.Hits;
            if (hits.IsEmpty) return None;
            var hit = hits.First();
            return hit.Result.Path;
        }

        private string MakeFullUrl(string path) =>
            $"https://genius.com{path}";
    }
}
