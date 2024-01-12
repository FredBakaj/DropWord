using System.Text;
using Newtonsoft.Json;

namespace DropWord.Infrastructure.Utils;

public class RestApiClient
{
    public async Task<TOut> PostAsync<TOut>(string apiUrl, Dictionary<string, string> headers, object body)
    {
        using (HttpClient httpClient = new HttpClient())
        {
            foreach (var headKey in headers.Keys)
            {
                httpClient.DefaultRequestHeaders.Add(headKey, headers[headKey]);
            }

            string jsonBody = JsonConvert.SerializeObject(body);
            var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                TOut result =
                    JsonConvert.DeserializeObject<TOut>(responseBody)!;
                return result;
            }

            response.EnsureSuccessStatusCode();
        }

        throw new InvalidOperationException("post request has problem");
    }
}
