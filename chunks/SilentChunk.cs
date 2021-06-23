using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using com.okitoki.wavhello.utils;

namespace com.okitoki.wavhello.chunks
{
    public class SilentChunk : Chunk
    {
        public int NumSilentSamples { get; set; }

        public static SilentChunk Read(FileStream fs, string chunkID, int chunkSize)
        {
            SilentChunk chunk = new SilentChunk();
            chunk.ChunkID = chunkID;
            chunk.ChunkSize = chunkSize;
            chunk.NumSilentSamples = BitConverter.ToInt32(BinaryFileUtils.Read(fs, 4), 0);
            return chunk;
        }

        public static void Write(BinaryWriter bw, SilentChunk chunk)
        {
            bw.Write(System.Text.Encoding.UTF8.GetBytes(chunk.ChunkID));
            bw.Write(chunk.ChunkSize);
            bw.Write(chunk.NumSilentSamples);
        }

        public override void Write(BinaryWriter bw)
        {
            SilentChunk.Write(bw, this);
        }
    }
}
