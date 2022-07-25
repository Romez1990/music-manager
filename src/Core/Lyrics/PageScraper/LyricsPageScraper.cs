using System;
using Core.HtmlParser;
using Core.HttpClient;
using Core.IocContainer;
using Core.Lyrics.PageScraper.Exceptions;
using LanguageExt;
using Utils.Enumerable;
using static LanguageExt.Prelude;

namespace Core.Lyrics.PageScraper {
    [Service]
    public class LyricsPageScraper : ILyricsPageScraper {
        public LyricsPageScraper(IHttpClient httpClient, IHtmlParser htmlParser) {
            _httpClient = httpClient;
            _htmlParser = htmlParser;
        }

        public EitherAsync<PageScrapException, string> ScrapLyrics(string pageUrl) {
            var castException = new Func<PageScrapException, PageScrapException>(identity);
            return GetHtml(pageUrl).MapLeft(castException)
                .Bind(html => ParseHtml(html).MapLeft(castException));
        }

        private readonly IHttpClient _httpClient;

        private EitherAsync<IncorrectPageException, string> GetHtml(string pageUrl) =>
            _httpClient.GetText(pageUrl)
                .MapLeft(_ => new IncorrectPageException());

        private readonly IHtmlParser _htmlParser;

        private EitherAsync<IncorrectPageException, string> ParseHtml(string html) =>
            _htmlParser.Parse(html)
                .Map(GetLyricsElement)
                .ToEitherAsync(() => new IncorrectPageException())
                .Map(GetLyricsText);

        private Option<ITagElement> GetLyricsElement(IDocument document) {
            var methods = new Func<IDocument, Option<ITagElement>>[] {
                GetLyricsElementPrimaryMethod,
                GetLyricsElementSecondaryMethod,
            };
            return methods.Tail()
                .Fold(methods.Head()(document),
                    (result, method) => result.Match(Some, () => method(document)));
        }

        private Option<ITagElement> GetLyricsElementPrimaryMethod(IElementContainer document) =>
            document.Select(".lyrics");

        private Option<ITagElement> GetLyricsElementSecondaryMethod(IDocument document) =>
            document.Select("#lyrics-root :nth-child(2)")
                .Map(element => {
                    element.SelectAll("br")
                        .ForEach(br => {
                            var span = document.CreateElement("span");
                            span.Text = "\n";
                            br.ReplaceWith(span);
                        });
                    return element;
                });

        private string GetLyricsText(IElement element) =>
            element.Text.Trim();
    }
}
