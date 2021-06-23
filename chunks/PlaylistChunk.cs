using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using com.okitoki.wavhello.utils;

namespace com.okitoki.wavhello.chunks
{
    public class PlaylistChunk : Chunk
    {
        private List<PlaylistSegment> segments = new List<PlaylistSegment>();

        public int NumSegments { get; set; }

        public List<PlaylistSegment> Segments
        {
            get { return this.segments; }
            set { this.segments = value; }
        }

        public static PlaylistChunk Read(FileStream fs, string chunkID, int chunkSize)
        {
            PlaylistChunk chunk = new PlaylistChunk();
            chunk.ChunkID = chunkID;
            chunk.ChunkSize = chunkSize;
            chunk.NumSegments = BitConverter.ToInt32(BinaryFileUtils.Read(fs, 4), 0);

            for(int i = 0; i < chunk.NumSegments; i++)
            {
                PlaylistSegment segment = PlaylistSegment.Read(fs);
                chunk.Segments.Add(segment);
            }

            return chunk;
        }

        public static void Write(BinaryWriter bw, PlaylistChunk chunk)
        {
            bw.Write(System.Text.Encoding.UTF8.GetBytes(chunk.ChunkID));
            bw.Write(chunk.ChunkSize);
            bw.Write(chunk.NumSegments);

            for(int i = 0; i < chunk.NumSegments; i++)
            {
                PlaylistSegment.Write(bw, chunk.Segments[i]);
            }
        }

        public override void Write(BinaryWriter bw)
        {
            PlaylistChunk.Write(bw, this);
        }
    }

    public class PlaylistSegment
    {
        public string CuePointID { get; set; }

        public int LengthInSamples { get; set; }

        public int NumRepeats { get; set; }

        public static PlaylistSegment Read(FileStream fs)
        {
            PlaylistSegment segment = new PlaylistSegment();
            segment.CuePointID = System.Text.Encoding.UTF8.GetString(BinaryFileUtils.Read(fs, 4));
            segment.LengthInSamples = BitConverter.ToInt32(BinaryFileUtils.Read(fs, 4), 0);
            segment.NumRepeats = BitConverter.ToInt32(BinaryFileUtils.Read(fs, 4), 0);
            return segment;
        }

        public static void Write(BinaryWriter bw, PlaylistSegment segment)
        {
            bw.Write(System.Text.Encoding.UTF8.GetBytes(segment.CuePointID));
            bw.Write(segment.LengthInSamples);
            bw.Write(segment.NumRepeats);
        }
    }
}
