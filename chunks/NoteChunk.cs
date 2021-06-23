using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace com.okitoki.wavhello.chunks
{
    public class NoteChunk : TextChunk
    {
        public override void Write(BinaryWriter bw)
        {
            NoteChunk.Write(bw, this);
        }
    }
}
