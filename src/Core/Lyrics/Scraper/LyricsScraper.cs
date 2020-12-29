using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using Core.FileSystem;
using Core.HttpClient;
using Core.Lyrics.Exceptions;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Core.Lyrics.Scraper
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

        private Either<LyricsNotFoundException, IElement> GetLyricsElement(IFileElement file, IDocument document) =>
            GetLyricsElement1Method(document)
                .Some(Some)
                .None(() => GetLyricsElement2Method(document))
                .ToEither(() => new LyricsNotFoundException(file));

        private Option<IElement> GetLyricsElement1Method(IParentNode parent) =>
            Optional(parent.QuerySelector(".lyrics"));

        private Option<IElement> GetLyricsElement2Method(IDocument document) =>
            Optional(document.QuerySelector("main > :nth-child(2) > :nth-child(3)"))
                .Map(element =>
                {
                    foreach (var br in element.QuerySelectorAll("br"))
                    {
                        var span = document.CreateElement("span");
                        span.TextContent = "\n";
                        br.ReplaceWith(span);
                    }

                    return element;
                });

        private string GetLyricsText(IElement lyricsBlock) =>
            lyricsBlock.TextContent;
    }
}
