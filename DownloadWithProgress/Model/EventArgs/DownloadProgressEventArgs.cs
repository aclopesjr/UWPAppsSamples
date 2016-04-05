using System.ComponentModel;

namespace DownloadWithProgress.Model
{
    public class DownloadProgressChangedEventArgs : ProgressChangedEventArgs
    {
        public ulong BytesReceived { get; private set; }
        public ulong? BytesToReceive { get; private set; }
        public new double ProgressPercentage { get; set; }

        public DownloadProgressChangedEventArgs(ulong bytesReceived, ulong? bytesToReceive, double progressPercentage, object userState = null)
            : base((int)progressPercentage, userState)
        {
            this.BytesReceived = bytesReceived;
            this.BytesToReceive = bytesToReceive;
        }
    }
}
