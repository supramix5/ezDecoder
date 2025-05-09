using System;
using System.IO;
using System.Linq;
using SevenZip.Compression.LZMA;

namespace EzDecoder.Logic
{
    public static class Methods
    {
        public static void CsvTomlDecoder(string file)
        {
            byte[] content = File.ReadAllBytes(file);

            using (var tempData = new MemoryStream())
            {
                tempData.Write(content, 0, 8);
                tempData.Write(new byte[4], 0, 4);
                tempData.Write(content, 8, content.Length - 8);

                tempData.Position = 0;

                using (var output = new MemoryStream())
                {
                    Decoder decoder = new Decoder();

                    byte[] props = new byte[5];
                    tempData.Read(props, 0, 5);
                    decoder.SetDecoderProperties(props);

                    byte[] sizeBytes = new byte[8];
                    tempData.Read(sizeBytes, 0, 8);
                    long outSize = BitConverter.ToInt64(sizeBytes, 0);

                    byte[] compressedData = new byte[tempData.Length - tempData.Position];
                    tempData.Read(compressedData, 0, compressedData.Length);

                    using (var inputStream = new MemoryStream(compressedData))
                    {
                        decoder.Code(inputStream, output, compressedData.Length, outSize, null);
                    }

                    SaveDecoded(file, output.ToArray());
                }
            }
        }

        public static void BankDecoder(string file)
        {
            // Implement BANK decoding logic here
        }

        private static void SaveDecoded(string fileName, byte[] content)
        {
            var directory = Path.GetDirectoryName(fileName);
            var outputDirectory = Path.Combine(directory, "ezDecoder_output");
            Directory.CreateDirectory(outputDirectory);

            var outputFile = Path.Combine(outputDirectory, Path.GetFileName(fileName));
            File.WriteAllBytes(outputFile, content);
        }
    }
}