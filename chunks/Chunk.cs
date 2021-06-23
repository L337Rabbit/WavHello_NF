using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace com.okitoki.wavhello.chunks
{
    public abstract class Chunk
    {
        private bool isPadded = false;

        public string ChunkID { get; set; }

        public int ChunkSize { get; set; }

        public bool IsPadded
        {
            get { return this.isPadded; }
            set { this.isPadded = value; }
        }

        public abstract void Write(BinaryWriter bw);
    }
}
