namespace Core.Operations.Lyrics.Searcher.HttpResponses
{
    public class HttpResponse<T> where T : class
    {
        public HttpResponse(T response)
        {
            Response = response;
        }

        public T Response { get; }
    }
}
