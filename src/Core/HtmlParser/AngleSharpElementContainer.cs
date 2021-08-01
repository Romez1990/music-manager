using System;
using System.Collections.Generic;
using AngleSharp.Dom;
using LanguageExt;
using static LanguageExt.Prelude;

namespace Core.HtmlParser {
    public abstract class AngleSharpElementContainer : IElementContainer {
        protected AngleSharpElementContainer(IParentNode element) {
            _element = element;
            _children = new Lazy<IEnumerable<IElement>>(GetChildren);
        }

        private readonly IParentNode _element;

        private readonly Lazy<IEnumerable<IElement>> _children;
        public IEnumerable<IElement> Children => _children.Value;

        private IEnumerable<IElement> GetChildren() =>
            _element.Children
                .Map(CreateTagElement);

        public Option<ITagElement> Select(string selector) =>
            Optional(_element.QuerySelector(selector))
                .Map(CreateTagElement);

        public IEnumerable<ITagElement> SelectAll(string selector) =>
            _element.QuerySelectorAll(selector)
                .Map(CreateTagElement);

        protected ITagElement CreateTagElement(AngleSharp.Dom.IElement element) =>
            new AngleSharpTagElement(element);
    }
}
