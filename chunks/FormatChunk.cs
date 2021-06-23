using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using com.okitoki.wavhello.utils;

namespace com.okitoki.wavhello.chunks
{
    public class FormatChunk : Chunk
    {

        public short AudioFormat { get; set; }

        public short NumChannels { get; set; }

        public int SampleRate { get; set; }

        public int BitRate { get; set; }

        public short BlockAlign { get; set; }

        public short BitsPerSample { get; set; }

        public static FormatChunk Read(FileStream fs, string chunkID, int chunkSize)
        {
            FormatChunk chunk = new FormatChunk();
            chunk.ChunkID = chunkID;
            chunk.ChunkSize = chunkSize;

            chunk.AudioFormat = BitConverter.ToInt16(BinaryFileUtils.Read(fs, 2), 0);
            chunk.NumChannels = BitConverter.ToInt16(BinaryFileUtils.Read(fs, 2), 0);
            chunk.SampleRate = BitConverter.ToInt32(BinaryFileUtils.Read(fs, 4), 0);
            chunk.BitRate = BitConverter.ToInt32(BinaryFileUtils.Read(fs, 4), 0);
            chunk.BlockAlign = BitConverter.ToInt16(BinaryFileUtils.Read(fs, 2), 0);
            chunk.BitsPerSample = BitConverter.ToInt16(BinaryFileUtils.Read(fs, 2), 0);

            if (chunk.AudioFormat == 1) { return chunk; }

            short extraParamsSize = BitConverter.ToInt16(BinaryFileUtils.Read(fs, 2), 0);
            BinaryFileUtils.Read(fs, extraParamsSize);

            return chunk;
        }

        public static void Write(BinaryWriter bw, FormatChunk chunk)
        {
            bw.Write(System.Text.Encoding.UTF8.GetBytes(chunk.ChunkID));
            bw.Write(chunk.ChunkSize);
            bw.Write(chunk.AudioFormat);
            bw.Write(chunk.NumChannels);
            bw.Write(chunk.SampleRate);
            bw.Write(chunk.BitRate);
            bw.Write(chunk.BlockAlign);
            bw.Write(chunk.BitsPerSample);
        }
        public override void Write(BinaryWriter bw)
        {
            FormatChunk.Write(bw, this);
        }

        public override string ToString()
        {
            return "{\n" +
                "\tCHUNK ID: " + ChunkID + "\n" +
                "\tCHUNK SIZE: " + ChunkSize + "\n" +
                "\tAudio Format: " + AudioFormat + "\n" +
                "\tChannels: " + NumChannels + "\n" +
                "\tSample Rate: " + SampleRate + "\n" +
                "\tBit Rate: " + BitRate + "\n" +
                "\tBlock Align: " + BlockAlign + "\n" +
                "\tBits Per Sample: " + BitsPerSample + "\n" +
                "}";
        }
    }
}
