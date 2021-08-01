namespace Core.HtmlParser {
    public interface ITagElement : IElement, IElementContainer {
        void ReplaceWith(ITagElement element);
    }
}
