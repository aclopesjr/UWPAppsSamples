using System.ComponentModel;

namespace DownloadWithProgress.Model
{
    public class CustomProgressChangedEventArgs : ProgressChangedEventArgs
    {
        public int BytesReceived { get; private set; }
        public int BytesToReceive { get; private set; }

        public CustomProgressChangedEventArgs(int bytesReceived, int bytesToReceive, int progressPercentage, object userState = null)
            : base(progressPercentage, userState)
        {
            this.BytesReceived = bytesReceived;
            this.BytesToReceive = bytesToReceive;
        }
    }
}
