using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Core.FileSystem;
using Core.HttpClient;
using Core.Operations.Lyrics.Exceptions;
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

        public EitherAsync<LyricsNotFoundException, string> Scrap(IFileElement file, string url) =>
            _http.Html(url)
                .MapLeft(_ => new LyricsNotFoundException(file))
                .MapAsync(_htmlParser.ParseDocumentAsync)
                .Bind(document => GetLyricsElement(file, document).ToAsync())
                .Map(GetLyricsText);

        private Either<LyricsNotFoundException, IElement> GetLyricsElement(IFileElement file, IParentNode parent) =>
            Optional(parent.QuerySelector(".lyrics"))
                .ToEither(() => new LyricsNotFoundException(file));

        private string GetLyricsText(IElement lyricsBlock) =>
            lyricsBlock.TextContent;
    }
}
