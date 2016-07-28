using System;

namespace SwipeControlSample.Model
{
    public class SwipeEventArgs : EventArgs
    {
        public SwipeDirection Direction { get; set; }

        public SwipeEventArgs()
            : base() { }

        public SwipeEventArgs(SwipeDirection swipeDirection)
            : this()
        {
            Direction = swipeDirection;
        }
    }
}
