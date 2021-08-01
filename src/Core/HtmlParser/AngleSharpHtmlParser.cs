using System.Threading;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using Core.IocContainer;
using LanguageExt;

namespace Core.HtmlParser {
    [Service]
    public class AngleSharpHtmlParser : IHtmlParser {
        public AngleSharpHtmlParser() {
            _htmlParser = new AngleSharp.Html.Parser.HtmlParser();
        }

        private readonly AngleSharp.Html.Parser.HtmlParser _htmlParser;

        public Task<IDocument> Parse(string html) =>
            _htmlParser.ParseDocumentAsync(html, CancellationToken.None)
                .Map(CreateDocument);

        private IDocument CreateDocument(IHtmlDocument htmlDocument) =>
            new AngleSharpDocument(htmlDocument);
    }
}
