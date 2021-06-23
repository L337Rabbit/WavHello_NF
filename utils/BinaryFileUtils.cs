using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace com.okitoki.wavhello.utils
{
    public class BinaryFileUtils
    {
        public static byte[] Read(FileStream fileStream, int numBytes)
        {
            return ReadBigEndian(fileStream, numBytes);
        }

        public static byte[] ReadBigEndian(FileStream fileStream, int numBytes)
        {
            byte[] buffer = new byte[numBytes];
            fileStream.Read(buffer, 0, numBytes);

            return buffer;
        }

        public static byte[] ReadLittleEndian(FileStream fileStream, int numBytes)
        {
            byte[] buffer = new byte[numBytes];
            fileStream.Read(buffer, 0, numBytes);
            Array.Reverse(buffer);

            return buffer;
        }
    }
}
