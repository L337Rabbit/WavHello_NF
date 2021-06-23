using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace com.okitoki.wavhello.chunks
{
    public class LabelChunk : TextChunk
    {
        public override void Write(BinaryWriter bw)
        {
            TextChunk.Write(bw, this);
        }
    }
}
