using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using com.okitoki.wavhello.utils;

namespace com.okitoki.wavhello.chunks
{
    public class WaveListChunk : Chunk
    {
        private List<Chunk> dataChunks = new List<Chunk>();

        public List<Chunk> DataChunks
        {
            get { return this.dataChunks; }
            set { this.dataChunks = value; }
        }

        public static WaveListChunk Read(FileStream fs, string chunkID, int chunkSize)
        {
            WaveListChunk chunk = new WaveListChunk();
            chunk.ChunkID = chunkID;
            chunk.ChunkSize = chunkSize;

            int bytesRead = 0;

            //Read sub-chunks
            while(bytesRead < chunk.ChunkSize)
            {
                Chunk subChunk = ReadNextChunk(fs);
                bytesRead += subChunk.ChunkSize;
                chunk.DataChunks.Add(subChunk);
            }

            return chunk;
        }

        public static void Write(BinaryWriter bw, WaveListChunk chunk)
        {
            bw.Write(System.Text.Encoding.UTF8.GetBytes(chunk.ChunkID));
            bw.Write(chunk.ChunkSize);

            //Write sub-chunks
            foreach(Chunk subChunk in chunk.DataChunks)
            {
                if(subChunk is DataChunk) 
                {
                    DataChunk.Write(bw, (DataChunk)subChunk);
                }
                else if(subChunk is SilentChunk)
                {
                    SilentChunk.Write(bw, (SilentChunk)subChunk);
                }
            }
        }

        private static Chunk ReadNextChunk(FileStream fs)
        {
            string chunkID = System.Text.Encoding.UTF8.GetString(BinaryFileUtils.Read(fs, 4));
            int chunkSize = BitConverter.ToInt32(BinaryFileUtils.Read(fs, 4), 0);

            switch(chunkID.ToUpper())
            {
                case "SLNT": return SilentChunk.Read(fs, chunkID, chunkSize);
                case "DATA": return DataChunk.Read(fs, chunkID, chunkSize);
            }

            return null;
        }

        public override void Write(BinaryWriter bw)
        {
            WaveListChunk.Write(bw, this);
        }
    }
}
