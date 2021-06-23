using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.okitoki.wavhello.utils;

namespace com.okitoki.wavhello.chunks
{
    public class UnknownChunk : Chunk
    {
        public byte[] ChunkData { get; set; }

        public static UnknownChunk Read(FileStream fs, string chunkID, int chunkSize)
        {
            UnknownChunk chunk = new UnknownChunk();
            chunk.ChunkID = chunkID;
            chunk.ChunkSize = chunkSize;
            chunk.ChunkData = BinaryFileUtils.Read(fs, chunkSize);
            return chunk;
        }


        public static void Write(BinaryWriter bw, UnknownChunk chunk)
        {
            bw.Write(System.Text.Encoding.UTF8.GetBytes(chunk.ChunkID));
            bw.Write(chunk.ChunkSize);
            bw.Write(chunk.ChunkData);
        }

        public override void Write(BinaryWriter bw)
        {
            UnknownChunk.Write(bw, this);
        }
    }
}
