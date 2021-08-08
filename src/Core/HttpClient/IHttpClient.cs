using System.Collections.Generic;
using Core.HttpClient.Exceptions;
using LanguageExt;

namespace Core.HttpClient {
    public interface IHttpClient {
        EitherAsync<RequestException, string> GetText(string url,
            IReadOnlyDictionary<string, string> parameters = null);
        EitherAsync<RequestException, T> Get<T>(string url, IReadOnlyDictionary<string, string> parameters = null);
    }
}
