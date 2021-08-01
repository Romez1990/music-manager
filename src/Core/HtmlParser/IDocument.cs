namespace Core.HtmlParser {
    public interface IDocument : IElementContainer {
        ITagElement CreateElement(string elementName);
    }
}
