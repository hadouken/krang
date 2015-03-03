using System;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
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
        private Torrent _selectedTorrent;

        public ShellViewModel(IHadoukenService hadoukenService)
        {
            if (hadoukenService == null) throw new ArgumentNullException("hadoukenService");
            _hadoukenService = hadoukenService;

            Torrents = new BindableCollection<Torrent>();
        }

        public override string DisplayName
        {
            get { return "Krang"; }
            set { throw new NotImplementedException(); }
        }

        public IObservableCollection<Torrent> Torrents { get; private set; }

        public Torrent SelectedTorrent
        {
            get { return _selectedTorrent; }
            set
            {
                if (Equals(value, _selectedTorrent)) return;
                _selectedTorrent = value;
                NotifyOfPropertyChange();
            }
        }

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
            //Task.Run(async () => await ReadEventsAsync());
            Task.Run(async () => await UpdateAsync());
        }

        private async Task ReadEventsAsync()
        {
            var socket = new ClientWebSocket();
            await socket.ConnectAsync(new Uri("ws://localhost:7070/events"), CancellationToken.None);

            while (true)
            {
                var buffer = new byte[1024];
                var segment = new ArraySegment<byte>(buffer);

                var result = await socket.ReceiveAsync(segment, CancellationToken.None);

                var data = Encoding.UTF8.GetString(segment.Array, 0, result.Count);

                Debug.WriteLine(data);
            }
        }

        private async Task UpdateAsync()
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
                        existingTorrent.State = torrent.Value.State;
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
