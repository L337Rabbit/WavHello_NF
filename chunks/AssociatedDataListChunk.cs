using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using com.okitoki.wavhello.utils;

namespace com.okitoki.wavhello.chunks
{
    public class AssociatedDataListChunk : Chunk
    {
        private List<Chunk> associatedDataChunks = new List<Chunk>();

        private Dictionary<string, int> subChunkMap = new Dictionary<string, int>();

        public string TypeID { get; set; }

        public List<Chunk> AssociatedDataChunks
        {
            get { return this.associatedDataChunks; }
            set { this.associatedDataChunks = value; }
        }

        public static AssociatedDataListChunk Read(FileStream fs, string chunkID, int chunkSize)
        {
            AssociatedDataListChunk chunk = new AssociatedDataListChunk();
            chunk.ChunkID = chunkID;
            chunk.ChunkSize = chunkSize;
            chunk.TypeID = System.Text.Encoding.UTF8.GetString(BinaryFileUtils.Read(fs, 4));

            int bytesRead = 4;

            while (bytesRead < chunk.ChunkSize) 
            {
                Chunk subChunk = ReadNextChunk(fs);
                bytesRead += subChunk.ChunkSize + 8;

                if(subChunk.IsPadded)
                {
                    //Increment the byte counter due to padding, since that isn't taken into account in the chunk size.
                    bytesRead++;
                }

                chunk.AssociatedDataChunks.Add(subChunk);
                chunk.subChunkMap.Add(subChunk.ChunkID, chunk.AssociatedDataChunks.Count - 1);

                if(chunk.TypeID.ToUpper().Equals("INFO"))
                {
                    Console.WriteLine(subChunk);
                }
            }

            return chunk;
        }

        public static void Write(BinaryWriter bw, AssociatedDataListChunk chunk)
        {
            bw.Write(System.Text.Encoding.UTF8.GetBytes(chunk.ChunkID));
            bw.Write(chunk.ChunkSize);
            bw.Write(System.Text.Encoding.UTF8.GetBytes(chunk.TypeID));

            foreach(Chunk subChunk in chunk.AssociatedDataChunks) 
            {
                if(subChunk is TextChunk)
                {
                    TextChunk.Write(bw, (TextChunk)subChunk);
                }
                else if(subChunk is LabeledTextChunk)
                {
                    LabeledTextChunk.Write(bw, (LabeledTextChunk)subChunk);
                }
                else if(subChunk is InfoChunk)
                {
                    InfoChunk.Write(bw, (InfoChunk)subChunk);
                }
            }
        }

        private static Chunk ReadNextChunk(FileStream fs)
        {
            string chunkID = System.Text.Encoding.UTF8.GetString(BinaryFileUtils.Read(fs, 4));
            int chunkSize = BitConverter.ToInt32(BinaryFileUtils.Read(fs, 4), 0);

            switch (chunkID.ToUpper())
            {
                case "LABL": return TextChunk.ReadLabelChunk(fs, chunkID, chunkSize);
                case "NOTE": return TextChunk.ReadNoteChunk(fs, chunkID, chunkSize);
                case "LTXT": return LabeledTextChunk.Read(fs, chunkID, chunkSize);
            }

            return InfoChunk.Read(fs, chunkID, chunkSize);
        }

        public override void Write(BinaryWriter bw)
        {
            AssociatedDataListChunk.Write(bw, this);
        }

        public override string ToString()
        {
            return "" +
                "{" +
                "\tCHUNK ID: " + this.ChunkID + "\n" +
                "\tCHUNK SIZE:" + this.ChunkSize + "\n" +
                "\tTYPE ID: " + TypeID + "\n" +
                "\tSUBCHUNK COUNT: " + associatedDataChunks.Count + "\n" +
                "}";
        }

        public Chunk GetSubChunk(string chunkID)
        {
            if(subChunkMap.ContainsKey(chunkID)) 
            { 
                return associatedDataChunks[subChunkMap[chunkID]];
            }

            return null;
        }
    }
}
