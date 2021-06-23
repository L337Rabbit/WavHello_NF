using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using com.okitoki.wavhello.utils;

namespace com.okitoki.wavhello.chunks
{
    public class CueChunk : Chunk
    {
        private List<CuePoint> cuePoints = new List<CuePoint>();

        public int NumCuePoints { get; set; }

        public List<CuePoint> CuePoints
        {
            get { return this.cuePoints; }
            set { this.cuePoints = value; }
        }

        public static CueChunk Read(FileStream fs, string chunkID, int chunkSize)
        {
            CueChunk chunk = new CueChunk();
            chunk.ChunkID = chunkID;
            chunk.ChunkSize = chunkSize;
            chunk.NumCuePoints = BitConverter.ToInt32(BinaryFileUtils.Read(fs, 4), 0);
            
            for(int i = 0; i < chunk.NumCuePoints; i++)
            {
                CuePoint cuePoint = CuePoint.Read(fs);
                chunk.CuePoints.Add(cuePoint);
            }

            return chunk;
        }

        public static void Write(BinaryWriter bw, CueChunk chunk)
        {
            bw.Write(System.Text.Encoding.UTF8.GetBytes(chunk.ChunkID));
            bw.Write(chunk.ChunkSize);
            bw.Write(chunk.NumCuePoints);

            for(int i = 0; i < chunk.NumCuePoints; i++)
            {
                CuePoint.Write(bw, chunk.CuePoints[i]);
            }
        }

        public override void Write(BinaryWriter bw)
        {
            CueChunk.Write(bw, this);
        }
    }

    public class CuePoint
    {
        /// <summary>
        /// A unique ID used to make reference to the cue point.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// The position of the sample (offset in bytes) to which this cue applies within the entirety of the data.
        /// </summary>
        public int Position { get; set; }
        
        /// <summary>
        /// A reference to the ID used by the chunk containing the sample corresponding with the cue point. Possible values are "data" or "slnt".
        /// </summary>
        public string DataChunkID { get; set; }

        /// <summary>
        /// The byte offset into the Wave List Chunk of the chunk which contains the sample for this cue point. The first chunk in the Wave List Chunk 
        /// would be an offset value of 0. The value of this field will also be 0 if there is no Wave List Chunk in the file.
        /// </summary>
        public int ChunkStartOffset { get; set; }

        /// <summary>
        /// The byte offset into the data chunk (or silent chunk) to the start of the block containing the sample to which the cue point applies.
        /// </summary>
        public int BlockStart { get; set; }

        /// <summary>
        /// The offset into the block of the sample that corresponds to the cue point. For non-compressed data, this is the byte offset into the "data" 
        /// chunk. For compressed data, this is the number of samples from the block start to the sample of the cue point.
        /// </summary>
        public int SampleOffset { get; set; }

        public static CuePoint Read(FileStream fs)
        {
            CuePoint cuePoint = new CuePoint();
            cuePoint.ID = System.Text.Encoding.UTF8.GetString(BinaryFileUtils.Read(fs, 4));
            cuePoint.Position = BitConverter.ToInt32(BinaryFileUtils.Read(fs, 4), 0);
            cuePoint.DataChunkID = System.Text.Encoding.UTF8.GetString(BinaryFileUtils.Read(fs, 4));
            cuePoint.ChunkStartOffset = BitConverter.ToInt32(BinaryFileUtils.Read(fs, 4), 0);
            cuePoint.BlockStart = BitConverter.ToInt32(BinaryFileUtils.Read(fs, 4), 0);
            cuePoint.SampleOffset = BitConverter.ToInt32(BinaryFileUtils.Read(fs, 4), 0);

            return cuePoint;
        }

        public static void Write(BinaryWriter bw, CuePoint cuePoint)
        {
            bw.Write(System.Text.Encoding.UTF8.GetBytes(cuePoint.ID));
            bw.Write(cuePoint.Position);
            bw.Write(System.Text.Encoding.UTF8.GetBytes(cuePoint.DataChunkID));
            bw.Write(cuePoint.ChunkStartOffset);
            bw.Write(cuePoint.BlockStart);
            bw.Write(cuePoint.SampleOffset);
        }
    }
}
