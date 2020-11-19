#nullable enable
using System.Collections.Generic;
using LanguageExt;

namespace Core.HttpClient
{
    public interface IHttp
    {
        EitherAsync<RequestException, string> Html(string url);
        EitherAsync<RequestException, T> Get<T>(string url, IDictionary<string, string>? parameters = null);
    }
}
