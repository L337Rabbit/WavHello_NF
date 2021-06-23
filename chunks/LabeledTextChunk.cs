using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using com.okitoki.wavhello.utils;

namespace com.okitoki.wavhello.chunks
{
    public class LabeledTextChunk : Chunk
    {
        public string CuePointID { get; set; }

        public int SampleLength { get; set; }

        public string PurposeID { get; set; }

        public string Country { get; set; }

        public string Language { get; set; }

        public string Dialect { get; set; }

        public string CodePage { get; set; }

        public string Text { get; set; }

        public static LabeledTextChunk Read(FileStream fs, string chunkID, int chunkSize)
        {
            LabeledTextChunk chunk = new LabeledTextChunk();
            chunk.ChunkID = chunkID;
            chunk.ChunkSize = chunkSize;
            chunk.CuePointID = System.Text.Encoding.UTF8.GetString(BinaryFileUtils.Read(fs, 4));
            chunk.SampleLength = BitConverter.ToInt32(BinaryFileUtils.Read(fs, 4), 0);
            chunk.PurposeID = System.Text.Encoding.UTF8.GetString(BinaryFileUtils.Read(fs, 4));
            chunk.Country = System.Text.Encoding.UTF8.GetString(BinaryFileUtils.Read(fs, 2));
            chunk.Language = System.Text.Encoding.UTF8.GetString(BinaryFileUtils.Read(fs, 2));
            chunk.Dialect = System.Text.Encoding.UTF8.GetString(BinaryFileUtils.Read(fs, 2));
            chunk.CodePage = System.Text.Encoding.UTF8.GetString(BinaryFileUtils.Read(fs, 2));

            chunk.Text = System.Text.Encoding.UTF8.GetString(BinaryFileUtils.Read(fs, chunk.ChunkSize - 20));

            if (chunk.ChunkSize % 2 == 1)
            {
                BinaryFileUtils.Read(fs, 1);
                chunk.IsPadded = true;
            }

            return chunk;
        }

        public static void Write(BinaryWriter bw, LabeledTextChunk chunk)
        {
            bw.Write(chunk.ChunkID);
            bw.Write(chunk.ChunkSize);
            bw.Write(System.Text.Encoding.UTF8.GetBytes(chunk.CuePointID));
            bw.Write(chunk.SampleLength);
            bw.Write(System.Text.Encoding.UTF8.GetBytes(chunk.PurposeID));
            bw.Write(System.Text.Encoding.UTF8.GetBytes(chunk.Country));
            bw.Write(System.Text.Encoding.UTF8.GetBytes(chunk.Language));
            bw.Write(System.Text.Encoding.UTF8.GetBytes(chunk.Dialect));
            bw.Write(System.Text.Encoding.UTF8.GetBytes(chunk.CodePage));
            bw.Write(System.Text.Encoding.UTF8.GetBytes(chunk.Text));

            if (chunk.IsPadded)
            {
                bw.Write((byte)0);
            }
        }

        public override void Write(BinaryWriter bw)
        {
            LabeledTextChunk.Write(bw, this);
        }
    }
}
