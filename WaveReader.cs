using com.okitoki.wavhello.chunks;
using com.okitoki.wavhello.utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Threading;

namespace com.okitoki.wavhello
{
    public class WaveReader
    {
        public WaveReader() { }

        public static WaveFile ReadFile(string filePath)
        {
            WaveFile wavefile = new WaveFile(filePath);
            FileStream fileStream = new FileStream(filePath, FileMode.Open);

            while (fileStream.Position < fileStream.Length)
            {
                Chunk chunk = ReadChunk(fileStream);
                wavefile.AddChunk(chunk);
            }

            fileStream.Close();
            wavefile.IsRead = true;

            return wavefile;
        }

        private static Chunk ReadChunk(FileStream fileStream)
        {
            Chunk chunk = null;
            string chunkID = System.Text.Encoding.UTF8.GetString(BinaryFileUtils.Read(fileStream, 4));
            int chunkSize = BitConverter.ToInt32(BinaryFileUtils.Read(fileStream, 4), 0);

            switch (chunkID.ToUpper())
            {
                case "RIFF": return RIFFChunk.Read(fileStream, chunkID, chunkSize);
                case "FMT ": return FormatChunk.Read(fileStream, chunkID, chunkSize);
                case "DATA": return DataChunk.Read(fileStream, chunkID, chunkSize);
                case "FACT": return FactChunk.Read(fileStream, chunkID, chunkSize);
                case "WAVL": return WaveListChunk.Read(fileStream, chunkID, chunkSize);
                case "CUE ": return CueChunk.Read(fileStream, chunkID, chunkSize);
                case "PLST": return PlaylistChunk.Read(fileStream, chunkID, chunkSize);
                case "LIST": return AssociatedDataListChunk.Read(fileStream, chunkID, chunkSize);
                case "SMPL": return ReadSamplerChunk();
                case "INST": return ReadInstrumentChunk();
            }

            return chunk;
        }

        private static Chunk ReadSamplerChunk()
        {
            throw new NotImplementedException("Sampler chunks have not been implmented.");
        }

        private static Chunk ReadInstrumentChunk()
        {
            throw new NotImplementedException("Instrument chunks have not been implemented.");
        }
    }
}
