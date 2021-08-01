using AngleSharp.Html.Dom;

namespace Core.HtmlParser {
    public class AngleSharpDocument : AngleSharpElementContainer, IDocument {
        public AngleSharpDocument(IHtmlDocument document) : base(document) {
            _document = document;
        }

        private readonly IHtmlDocument _document;

        public ITagElement CreateElement(string elementName) =>
            CreateTagElement(_document.CreateElement(elementName));
    }
}
