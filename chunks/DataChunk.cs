using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using com.okitoki.wavhello.utils;

namespace com.okitoki.wavhello.chunks
{
    public class DataChunk : Chunk
    {
        public byte[] Data { get; set; }

        public static DataChunk Read(FileStream fs, string chunkID, int chunkSize)
        {
            DataChunk chunk = new DataChunk();
            chunk.ChunkID = chunkID;
            chunk.ChunkSize = chunkSize;

            if(chunk.ChunkSize % 2 == 1)
            {
                //We need to add an extra byte for padding
                chunk.IsPadded = true;
                chunk.Data = new byte[chunk.ChunkSize + 1];
            }
            else
            {
                chunk.Data = new byte[chunk.ChunkSize];
            }

            //Read data into the chunk.
            BinaryFileUtils.Read(fs, chunk.ChunkSize).CopyTo(chunk.Data, 0);

            return chunk;
        }

        public static void Write(BinaryWriter bw, DataChunk chunk)
        {
            bw.Write(System.Text.Encoding.UTF8.GetBytes(chunk.ChunkID));
            bw.Write(chunk.ChunkSize);
            bw.Write(chunk.Data);
        }

        public override void Write(BinaryWriter bw)
        {
            DataChunk.Write(bw, this);
        }
    }
}
