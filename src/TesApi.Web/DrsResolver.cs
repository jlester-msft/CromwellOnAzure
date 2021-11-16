using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TesApi.Web
{
    public class DrsResolver
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public async Task<string> ResolveAsync(string drsUri, string bearerToken)
        {
            // TODO support Compact Identifier-based DRS URIs
            //drs://drs.example.org/314159
            //GET https://drs.example.org/ga4gh/drs/v1/objects/314159

            var uri = new Uri(drsUri);
            string httpsUrl = $"https://{uri.Host}/ga4gh/drs/v1/objects{uri.AbsolutePath}";
            using var message = new HttpRequestMessage(HttpMethod.Get, httpsUrl);
            message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
            using var response = await httpClient.SendAsync(message);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return drsUri;
        }
    }
}
