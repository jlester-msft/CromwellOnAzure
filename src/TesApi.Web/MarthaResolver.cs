using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TesApi.Web
{
    public class MarthaResolver
    {
        private static readonly HttpClient httpClient = new HttpClient();

        public string MarthaHost { get; set; }

        
        public MarthaResolver(string marthaHost)
        {
            MarthaHost = marthaHost;
        }

        public async Task<string> ResolveUrlAsync(string marthaUrl, string token)
        {
            using var message = new HttpRequestMessage(HttpMethod.Post, MarthaHost);
            message.Headers.Add("content-type", "application/json");
            message.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(new MarthaRequest { url = marthaUrl }));
            message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            using var response = await httpClient.SendAsync(message);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var marthaResponse = System.Text.Json.JsonSerializer.Deserialize<MarthaResponse>(content);
            return marthaResponse.accessUrl.url;
        }

        public async Task<(string, long)> GetAsync(string marthaUrl, string token)
        {
            using var message = new HttpRequestMessage(HttpMethod.Post, MarthaHost);
            message.Headers.Add("content-type", "application/json");
            message.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(new MarthaRequest { url = marthaUrl }));
            message.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            using var response = await httpClient.SendAsync(message);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var marthaResponse = System.Text.Json.JsonSerializer.Deserialize<MarthaResponse>(content);
            return (marthaResponse.accessUrl.url, marthaResponse.size);
        }

        public class MarthaRequest
        {
            public string url { get; set; }
        }

        public class MarthaResponse
        {
            public string contentType { get; set; }
            public long size { get; set; }
            public DateTime timeCreated { get; set; }
            public DateTime timeUpdated { get; set; }
            public string bucket { get; set; }
            public string name { get; set; }
            public string gsUri { get; set; }
            public object googleServiceAccount { get; set; }
            public string fileName { get; set; }
            public Accessurl accessUrl { get; set; }
            public Hashes hashes { get; set; }
        }

        public class Accessurl
        {
            public string url { get; set; }
            public Headers headers { get; set; }
        }

        public class Headers
        {
            public string Authorization { get; set; }
        }

        public class Hashes
        {
            public string md5 { get; set; }
            public string sha256 { get; set; }
            public string crc32c { get; set; }
        }
    }
}
