using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Krang.Client.Models;
using Newtonsoft.Json;

namespace Krang.Client.Services
{
    public sealed class HadoukenService : IHadoukenService
    {
        public class JsonRpcResponse<T>
        {
            [JsonProperty("result")]
            public T Result { get; set; }
        }

        public async Task<IDictionary<string, Torrent>> GetTorrentsAsync()
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent("{ \"jsonrpc\": \"2.0\", \"id\": 1, \"method\": \"session.getTorrents\", \"params\": [] }");
                var response = await client.PostAsync("http://localhost:7070/api", content);
                var responseJson = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<JsonRpcResponse<IDictionary<string, Torrent>>>(responseJson).Result;
            }
        }

        public async Task AddTorrentFileAsync(string fileName)
        {
            var data = File.ReadAllBytes(fileName);
            var encoded = Convert.ToBase64String(data);

            using (var client = new HttpClient())
            {
                var content =
                    new StringContent(
                        string.Format(
                            "{{ \"jsonrpc\": \"2.0\", \"id\": 1, \"method\": \"session.addTorrentFile\", \"params\": [ \"{0}\" ] }}",
                            encoded));

                await client.PostAsync("http://localhost:7070/api", content);
            }
        }
    }
}