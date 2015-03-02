using System;
using System.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using Krang.Client.Models;
using Krang.Client.Services;
using Microsoft.Win32;

namespace Krang.Client.ViewModels
{
    public sealed class ShellViewModel : Screen
    {
        private readonly IHadoukenService _hadoukenService;

        public ShellViewModel(IHadoukenService hadoukenService)
        {
            if (hadoukenService == null) throw new ArgumentNullException("hadoukenService");
            _hadoukenService = hadoukenService;

            Torrents = new BindableCollection<Torrent>();
        }

        public IObservableCollection<Torrent> Torrents { get; private set; }

        public async void ShowAddTorrentFileAsync()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Torrents (*.torrent)|*.torrent",
                Title = "Select a .torrent file to open"
            };

            if (dialog.ShowDialog() == true)
            {
                await _hadoukenService.AddTorrentFileAsync(dialog.FileName);
            }
        }

        protected override void OnActivate()
        {
            Task.Run(async () => await RunAsync());
        }

        private async Task RunAsync()
        {
            while (true)
            {
                var torrents = await _hadoukenService.GetTorrentsAsync();

                foreach (var torrent in torrents)
                {
                    var existingTorrent = Torrents.SingleOrDefault(t => t.InfoHash == torrent.Key);

                    if (existingTorrent != null)
                    {
                        existingTorrent.DownloadRate = torrent.Value.DownloadRate;
                        existingTorrent.Peers = torrent.Value.Peers;
                        existingTorrent.Progress = torrent.Value.Progress;
                        existingTorrent.Seeds = torrent.Value.Seeds;
                        existingTorrent.UploadRate = torrent.Value.UploadRate;
                    }
                    else
                    {
                        Torrents.Add(torrent.Value);
                    }
                }

                await Task.Delay(1000);
            }
        }
    }
}
