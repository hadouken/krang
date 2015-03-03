namespace Krang.Client.Models
{
    public enum TorrentState
    {
        QueuedForChecking,
        CheckingFiles,
        DownloadingMetadata,
        Downloading,
        Finished,
        Seeding,
        Allocating,
        CheckingResumeData
    }
}
