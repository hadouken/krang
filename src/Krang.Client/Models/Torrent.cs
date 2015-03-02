using Caliburn.Micro;
using Newtonsoft.Json;

namespace Krang.Client.Models
{
    public sealed class Torrent : PropertyChangedBase
    {
        private string _name;
        private string _infoHash;
        private float _progress;
        private int _downloadRate;
        private int _uploadRate;
        private int _seeds;
        private int _peers;

        [JsonProperty("name")]
        public string Name
        {
            get { return _name; }
            set
            {
                if(value == _name) return;;
                _name = value;
                NotifyOfPropertyChange();
            }
        }

        [JsonProperty("infoHash")]
        public string InfoHash
        {
            get { return _infoHash; }
            set
            {
                if (value == _infoHash) return;
                _infoHash = value;
                NotifyOfPropertyChange();
            }
        }

        [JsonProperty("progress")]
        public float Progress
        {
            get { return _progress; }
            set
            {
                if (value.Equals(_progress)) return;
                _progress = value;
                NotifyOfPropertyChange();
            }
        }

        [JsonProperty("downloadRate")]
        public int DownloadRate
        {
            get { return _downloadRate; }
            set
            {
                if (value == _downloadRate) return;
                _downloadRate = value;
                NotifyOfPropertyChange();
            }
        }

        [JsonProperty("uploadRate")]
        public int UploadRate
        {
            get { return _uploadRate; }
            set
            {
                if (value == _uploadRate) return;
                _uploadRate = value;
                NotifyOfPropertyChange();
            }
        }

        [JsonProperty("numSeeds")]
        public int Seeds
        {
            get { return _seeds; }
            set
            {
                if (value == _seeds) return;
                _seeds = value;
                NotifyOfPropertyChange();
            }
        }

        [JsonProperty("numPeers")]
        public int Peers
        {
            get { return _peers; }
            set
            {
                if (value == _peers) return;
                _peers = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
