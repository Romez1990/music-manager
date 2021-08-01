using System;
using System.Collections.Generic;
using System.Linq;
using Core.HttpClient;
using Core.IocContainer;
using Core.Lyrics.PageSearcher.Exceptions;
using Core.UserConfig;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Core.Lyrics.PageSearcher {
    [Service]
    public class LyricsPageSearcher : ILyricsPageSearcher {
        public LyricsPageSearcher(IHttpClient httpClient, IUserConfigService userConfigService) {
            _httpClient = httpClient;
            _userConfig = userConfigService.Config;
        }

        public EitherAsync<PageSearchException, string> SearchPageUrl(ITrack track) {
            var castException = new Func<PageSearchException, PageSearchException>(identity);
            return GetSearchQuery(track).MapLeft(castException).ToAsync()
                .Bind(searchQuery => SendSearchRequest(searchQuery).MapLeft(castException))
                .Bind(response => GetPageUrl(response).MapLeft(castException).ToAsync());
        }

        private Either<MissingTagException, string> GetSearchQuery(ITrack track) {
            var missingTags = new List<string>();
            AddMissingTag(missingTags, track.Artists, "artists");
            AddMissingTag(missingTags, track.Title, "title");
            return track.Artists.Bind(artists =>
                    track.Title.Map(title => MakeSearchQuery(artists, title)))
                .ToEither(() => new MissingTagException(missingTags));
        }

        private void AddMissingTag(ICollection<string> missingTags, Option<object> tag, string tagName) {
            if (tag.IsNone) {
                missingTags.Add(tagName);
            }
        }

        private string MakeSearchQuery(IEnumerable<string> artists, string title) =>
            $"{string.Join(' ', artists)} {title}";

        private const string BaseUrl = "https://api.genius.com";

        private readonly IHttpClient _httpClient;

        private readonly UserConfig.UserConfig _userConfig;

        private EitherAsync<UnknownApiException, GeniusResponse<SearchResponse>>
            SendSearchRequest(string searchQuery) =>
            _httpClient.Get<GeniusResponse<SearchResponse>>($"{BaseUrl}/search", new Dictionary<string, string> {
                    { "access_token", _userConfig.GeniusApiToken },
                    { "q", searchQuery },
                })
                .MapLeft(_ => new UnknownApiException());

        private Either<LyricsNotFoundException, string> GetPageUrl(GeniusResponse<SearchResponse> response) {
            var hits = response.Response.Hits;
            if (!hits.Any())
                return new LyricsNotFoundException();
            var hit = hits.First();
            var path = hit.Result.Path;
            return $"https://genius.com{path}";
        }
    }
}
