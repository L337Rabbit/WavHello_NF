using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using com.okitoki.wavhello.utils;

namespace com.okitoki.wavhello.chunks
{
    public abstract class TextChunk : Chunk
    {
        public string CuePointID { get; set; }

        public string Text { get; set; }

        public static LabelChunk ReadLabelChunk(FileStream fs, string chunkID, int chunkSize)
        {
            LabelChunk chunk = new LabelChunk();
            chunk.ChunkID = chunkID;
            chunk.ChunkSize = chunkSize;

            Read(fs, chunk);
            return chunk;
        }

        public static NoteChunk ReadNoteChunk(FileStream fs, string chunkID, int chunkSize)
        {
            NoteChunk chunk = new NoteChunk();
            chunk.ChunkID = chunkID;
            chunk.ChunkSize = chunkSize;

            Read(fs, chunk);
            return chunk;
        }

        private static void Read(FileStream fs, TextChunk chunk)
        {
            chunk.CuePointID = System.Text.Encoding.UTF8.GetString(BinaryFileUtils.Read(fs, 4));
            chunk.Text = System.Text.Encoding.UTF8.GetString(BinaryFileUtils.Read(fs, chunk.ChunkSize - 4));

            if (chunk.ChunkSize % 2 == 1)
            {
                BinaryFileUtils.Read(fs, 1);
                chunk.IsPadded = true;
            }
        }

        public static void Write(BinaryWriter bw, TextChunk chunk)
        {
            bw.Write(System.Text.Encoding.UTF8.GetBytes(chunk.ChunkID));
            bw.Write(chunk.ChunkSize);
            bw.Write(System.Text.Encoding.UTF8.GetBytes(chunk.CuePointID));
            bw.Write(System.Text.Encoding.UTF8.GetBytes(chunk.Text));

            if (chunk.IsPadded)
            {
                bw.Write((byte)0);
            }
        }

        public override void Write(BinaryWriter bw)
        {
            TextChunk.Write(bw, this);
        }
    }
}
