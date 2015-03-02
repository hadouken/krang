using System.Collections.Generic;
using System.Threading.Tasks;
using Krang.Client.Models;

namespace Krang.Client.Services
{
    public interface IHadoukenService
    {
        Task<IDictionary<string, Torrent>> GetTorrentsAsync();
        
        Task AddTorrentFileAsync(string fileName);
    }
}
