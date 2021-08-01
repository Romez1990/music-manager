using System.Collections.Generic;
using LanguageExt;

namespace Core.HtmlParser {
    public interface IElementContainer {
        IEnumerable<IElement> Children { get; }
        Option<ITagElement> Select(string selector);
        IEnumerable<ITagElement> SelectAll(string selector);
    }
}
