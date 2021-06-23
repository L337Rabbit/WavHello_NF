using System;
using System.Collections.Generic;
using System.Text;

namespace com.okitoki.wavhello.events
{
    public class PlayInterruptedEventArgs : EventArgs
    {
        public WaveFile WaveFile { get; set; }
    }
}
