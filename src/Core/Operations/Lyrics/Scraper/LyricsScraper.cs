using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Core.HttpClient;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Core.Operations.Lyrics.Scraper
{
    public class LyricsScraper : ILyricsScraper
    {
        public LyricsScraper(IHttp http)
        {
            _http = http;
            _htmlParser = new HtmlParser();
        }

        private readonly IHttp _http;
        private readonly HtmlParser _htmlParser;

        public OptionAsync<string> Scrap(string url) =>
            _http.Html(url)
                .ToOption()
                .MapAsync(_htmlParser.ParseDocumentAsync)
                .Bind(document => GetLyricsElement(document).ToAsync())
                .Map(GetLyricsText);

        private Option<IElement> GetLyricsElement(IParentNode parent) =>
            Optional(parent.QuerySelector(".lyrics"));

        private string GetLyricsText(IElement lyricsBlock) =>
            lyricsBlock.TextContent;
    }
}
