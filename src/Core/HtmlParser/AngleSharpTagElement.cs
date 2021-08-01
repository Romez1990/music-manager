using AngleSharp.Dom;

namespace Core.HtmlParser {
    public class AngleSharpTagElement : AngleSharpElementContainer, ITagElement {
        public AngleSharpTagElement(AngleSharp.Dom.IElement element) : base(element) {
            _element = element;
        }

        private readonly AngleSharp.Dom.IElement _element;

        public string Text {
            get => _element.TextContent;
            set => _element.TextContent = value;
        }

        public void ReplaceWith(ITagElement element) {
            var replacingElement = ((AngleSharpTagElement)element)._element;
            _element.ReplaceWith(replacingElement);
        }
    }
}
