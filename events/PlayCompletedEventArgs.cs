using System;
using System.Collections.Generic;
using System.Text;

namespace com.okitoki.wavhello.events
{
    public class PlayCompletedEventArgs : EventArgs
    {
        public WaveFile WaveFile { get; set; }
    }
}
