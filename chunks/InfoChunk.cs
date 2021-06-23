using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using com.okitoki.wavhello.utils;

namespace com.okitoki.wavhello.chunks
{
    public class InfoChunk : Chunk
    {
        private byte[] data;

        public string TextValue { get; set; }

        public byte[] Data 
        { 
            get { return this.data; }
            set { this.data = value; }
        }

        public static InfoChunk Read(FileStream fs, string chunkID, int chunkSize)
        {
            InfoChunk chunk = new InfoChunk();
            chunk.ChunkID = chunkID;
            chunk.ChunkSize = chunkSize;

            chunk.Data = new byte[chunkSize];
            BinaryFileUtils.Read(fs, chunk.Data.Length).CopyTo(chunk.Data, 0);
            chunk.TextValue = System.Text.Encoding.UTF8.GetString(chunk.Data);

            if (chunkSize % 2 == 1)
            {
                chunk.IsPadded = true;
                BinaryFileUtils.Read(fs, 1);
            }            
            
            return chunk;
        }

        public static void Write(BinaryWriter bw, InfoChunk chunk)
        {
            bw.Write(System.Text.Encoding.UTF8.GetBytes(chunk.ChunkID));
            bw.Write(chunk.ChunkSize);
            bw.Write(chunk.Data);

            if(chunk.IsPadded)
            {
                bw.Write((byte)0);
            }
        }

        public override void Write(BinaryWriter bw)
        {
            InfoChunk.Write(bw, this);
        }

        public override String ToString()
        {
            return "{\n" +
                "\tCHUNK ID: " + ChunkID + "\n" +
                "\tCHUNK SIZE: " + ChunkSize + "\n" +
                "\tText Value: " + TextValue + "\n" +
                "}";
        }
    }
}
