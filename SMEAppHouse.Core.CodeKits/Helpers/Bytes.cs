using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace SMEAppHouse.Core.CodeKits.Helpers
{
    public class Bytes
    {
        //Convert a string to bytes for compression/decompression
        public static byte[] GetBytes(string str)
        {
            return Encoding.ASCII.GetBytes(str);
        }

        //After compressing/decompressing bytes this turns them back to a string
        public static string BytesToString(byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes);
        }

        public static byte[] CompressBytes(byte[] bytes)
        {
            //Simply write the bytes to memory using the .Net compression stream
            var output = new MemoryStream();
            var gzip = new GZipStream(output, CompressionMode.Compress, true);
            gzip.Write(bytes, 0, bytes.Length);
            gzip.Close();
            return output.ToArray();
        }

        public static byte[] DecompressBytes(byte[] bytes)
        {
            //Use the .Net decompression stream in memory
            var input = new MemoryStream();
            input.Write(bytes, 0, bytes.Length);
            input.Position = 0;

            var gzip = new GZipStream(input, CompressionMode.Decompress, true);
            var output = new MemoryStream();

            var buff = new byte[64]; //Compressed bytes are read in 64 bytes at a time
            var read = -1;
            read = gzip.Read(buff, 0, buff.Length);
            while (read > 0)
            {
                output.Write(buff, 0, read);
                read = gzip.Read(buff, 0, buff.Length);
            }

            gzip.Close();
            return output.ToArray();
        }

        public static byte[] ObjectToByteArray<T>(T obj)
        {
            if (obj == null) return null;
            using var ms = new MemoryStream();
            JsonSerializer.SerializeAsync(ms, obj).Wait();
            return ms.ToArray();
        }

        public static T ByteArrayToObject<T>(byte[] arrBytes)
        {
            using MemoryStream memStream = new(arrBytes);
            return JsonSerializer.Deserialize<T>(memStream, options: new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // If needed
            });
        }
    }
}
