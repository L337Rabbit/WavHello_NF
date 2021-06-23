using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using com.okitoki.wavhello.chunks;

namespace com.okitoki.wavhello
{
    public class WaveFile
    {
        public List<Chunk> Chunks { get; set; } = new List<Chunk>();

        public string FilePath { get; set; }

        public int InfoChunkIdx { get; set; } = -1;

        public int FileSizeInBytes { get; set; }

        public int SampleSizeInBytes { get; set; }

        public int NumSamples { get; set; } = -1;

        public int SamplesPerSecond { get; set; }

        public int NumChannels { get; set; }

        public bool IsRead { get; set; } = false;

        public WaveFile(string filePath)
        {
            this.FilePath = filePath;
        }

        public void AddChunk(Chunk chunk)
        {
            Chunks.Add(chunk);

            if (chunk is RIFFChunk)
            {
                FileSizeInBytes = ((RIFFChunk)chunk).ChunkSize + 8;
            }
            else if (chunk is FormatChunk)
            {
                SamplesPerSecond = ((FormatChunk)chunk).SampleRate;
                NumChannels = ((FormatChunk)chunk).NumChannels;

                //Calculate the sample size in bytes based on the bits per sample.
                int bitsPerSample = ((FormatChunk)chunk).BitsPerSample;
                SampleSizeInBytes = (int)(((float)bitsPerSample / 8.0f));
                if (bitsPerSample % 8 > 0)
                {
                    SampleSizeInBytes++;
                }
            }
            else if (chunk is FactChunk)
            {
                SetNumSamples(((FactChunk)chunk).NumSamples);
            }
            else if (chunk is DataChunk)
            {
                SetNumSamples(((DataChunk)chunk).ChunkSize / SampleSizeInBytes);
            }
            else if (chunk is AssociatedDataListChunk)
            {
                if (((AssociatedDataListChunk)chunk).TypeID.ToUpper().Equals("INFO"))
                {
                    InfoChunkIdx = Chunks.Count - 1;
                }
            }
        }

        public void SetNumSamples(int numSamples)
        {
            if(numSamples > 0)
            {
                this.NumSamples = numSamples;
            }
        }

        public float LengthInSeconds()
        {
            return (((float)NumSamples) / SamplesPerSecond) / NumChannels;
        }

        public string GetArtist()
        {
            return GetInfoChunkValue("IART");
        }

        public string GetTrack()
        {
            return GetInfoChunkValue("INAM");
        }

        public string GetAlbum()
        {
            return GetInfoChunkValue("IPRD");
        }        

        public string GetInfoChunkValue(string chunkID)
        {
            if (InfoChunkIdx >= 0)
            {
                Chunk artistChunk = ((AssociatedDataListChunk)Chunks[InfoChunkIdx]).GetSubChunk(chunkID);
                return ((InfoChunk)artistChunk).TextValue;
            }

            return null;
        }

        public MemoryStream GetStream()
        {
            return WaveFile.GetStream(this);
        }

        public static MemoryStream GetStream(WaveFile wavefile)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(memStream);

            foreach (Chunk chunk in wavefile.Chunks)
            {
                chunk.Write(bw);
            }

            memStream.Seek(0, SeekOrigin.Begin);

            return memStream;
        }
    }
}
