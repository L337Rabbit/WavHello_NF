using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using com.okitoki.wavhello.utils;

namespace com.okitoki.wavhello.chunks
{
    public class RIFFChunk : Chunk
    {
        public string Format { get; set; }

        public static RIFFChunk Read(FileStream fs, string chunkID, int chunkSize)
        {
            RIFFChunk chunk = new RIFFChunk();
            chunk.ChunkID = chunkID;
            chunk.ChunkSize = chunkSize;
            chunk.Format = System.Text.Encoding.UTF8.GetString(BinaryFileUtils.Read(fs, 4));
            return chunk;
        }

        public static void Write(BinaryWriter bw, RIFFChunk chunk)
        {
            bw.Write(System.Text.Encoding.UTF8.GetBytes(chunk.ChunkID));
            bw.Write(chunk.ChunkSize);
            bw.Write(System.Text.Encoding.UTF8.GetBytes(chunk.Format));
        }

        public override void Write(BinaryWriter bw)
        {
            RIFFChunk.Write(bw, this);
        }
    }
}
