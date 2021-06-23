using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.IO;
using com.okitoki.wavhello.utils;

namespace com.okitoki.wavhello.chunks
{
    public class FactChunk : Chunk
    {
        public int NumSamples { get; set; }

        public static FactChunk Read(FileStream fs, string chunkID, int chunkSize)
        {
            FactChunk chunk = new FactChunk();
            chunk.ChunkID = chunkID;
            chunk.ChunkSize = chunkSize;
            chunk.NumSamples = BitConverter.ToInt32(BinaryFileUtils.Read(fs, 4), 0);
            return chunk;
        }

        public static void Write(BinaryWriter bw, FactChunk chunk)
        {
            bw.Write(System.Text.Encoding.UTF8.GetBytes(chunk.ChunkID));
            bw.Write(chunk.ChunkSize);
            bw.Write(chunk.NumSamples);
        }

        public override void Write(BinaryWriter bw)
        {
            FactChunk.Write(bw, this);
        }
    }
}
