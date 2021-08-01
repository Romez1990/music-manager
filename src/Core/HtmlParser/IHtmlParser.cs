using System.Threading.Tasks;

namespace Core.HtmlParser {
    public interface IHtmlParser {
        Task<IDocument> Parse(string html);
    }
}
