#region Using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
#endregion Using

namespace CaseBusiness.Framework.DadosCompressao
{
    public class DadosCompressao
    {
        //http://madskristensen.net/post/compress-and-decompress-strings-in-c

        public static string Compress(string text)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(text);
            MemoryStream ms = new MemoryStream();
            using (GZipStream zip = new GZipStream(ms, CompressionMode.Compress, true))
            {
                zip.Write(buffer, 0, buffer.Length);
            }

            ms.Position = 0;
            MemoryStream outStream = new MemoryStream();

            byte[] compressed = new byte[ms.Length];
            ms.Read(compressed, 0, compressed.Length);

            byte[] gzBuffer = new byte[compressed.Length + 4];
            System.Buffer.BlockCopy(compressed, 0, gzBuffer, 4, compressed.Length);
            System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gzBuffer, 0, 4);
            return Convert.ToBase64String(gzBuffer);
        }

        public static string Decompress(string compressedText)
        {
            byte[] gzBuffer = Convert.FromBase64String(compressedText);
            using (MemoryStream ms = new MemoryStream())
            {
                int msgLength = BitConverter.ToInt32(gzBuffer, 0);
                ms.Write(gzBuffer, 4, gzBuffer.Length - 4);

                byte[] buffer = new byte[msgLength];

                ms.Position = 0;
                using (GZipStream zip = new GZipStream(ms, CompressionMode.Decompress))
                {
                    zip.Read(buffer, 0, buffer.Length);
                }

                return Encoding.UTF8.GetString(buffer);
            }
        }

        #region Em desuso

        ///// <summary>
        ///// Compresses byte array to new byte array.
        ///// </summary>
        //byte[] Compress_GZipStream(byte[] raw)
        ////public static byte[] Compress(byte[] raw)
        //{
        //    //http://www.dotnetperls.com/compress

        //    using (MemoryStream memory = new MemoryStream())
        //    {
        //        using (GZipStream gzip = new GZipStream(memory,
        //        CompressionMode.Compress, true))
        //        {
        //            gzip.Write(raw, 0, raw.Length);
        //        }
        //        return memory.ToArray();
        //    }
        //}

        //byte[] Decompress_GZipStream(byte[] gzip)
        ////static byte[] Decompress(byte[] gzip)
        //{
        //    //http://www.dotnetperls.com/decompress

        //    // Create a GZIP stream with decompression mode.
        //    // ... Then create a buffer and write into while reading from the GZIP stream.
        //    using (GZipStream stream = new GZipStream(new MemoryStream(gzip), CompressionMode.Decompress))
        //    {
        //        const int size = 4096;
        //        byte[] buffer = new byte[size];
        //        using (MemoryStream memory = new MemoryStream())
        //        {
        //            int count = 0;
        //            do
        //            {
        //                count = stream.Read(buffer, 0, size);
        //                if (count > 0)
        //                {
        //                    memory.Write(buffer, 0, count);
        //                }
        //            }
        //            while (count > 0);
        //            return memory.ToArray();
        //        }
        //    }
        //}
        #endregion Em desuso

    }
}
