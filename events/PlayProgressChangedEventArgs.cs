using System;
using System.Collections.Generic;
using System.Text;

namespace com.okitoki.wavhello.events
{
    public class PlayProgressChangedEventArgs : EventArgs
    {
        public float Progress { get; set; }

        public WaveFile WaveFile { get; set; }
    }
}
