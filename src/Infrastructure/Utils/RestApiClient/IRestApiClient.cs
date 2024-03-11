namespace DropWord.Infrastructure.Utils.RestApiClient;

public interface IRestApiClient
{
    public Task<TOut> PostAsync<TOut>(string apiUrl, Dictionary<string, string> headers, object body);

}
