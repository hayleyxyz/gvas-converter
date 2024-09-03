using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using GvasFormat;
using GvasFormat.Serialization;
using Newtonsoft.Json;

namespace GvasConverter
{
    class Program
    {
        enum Operation
        {
            Encode,
            Decode
        }

        static void Help()
        {
            var executableName = Path.GetFileName(Process.GetCurrentProcess().MainModule.FileName);
            Console.WriteLine($"Usage: {executableName} [-d --decode | -e --encode] <input file> [output file]");
        }

        static void Main(string[] args)
        {
            Operation operation;

            if (args.Length < 2)
            {
                Help();
                return;
            }
            else if (args[0] == "-d" || args[0] == "--decode")
            {
                operation = Operation.Decode;
            }
            else if (args[0] == "-e" || args[0] == "--encode")
            {
                operation = Operation.Encode;
            }
            else
            {
                Help();
                return;
            }

            if (operation == Operation.Decode)
            {
                DecodeSaveFile(args);
            }
            else
            {
                EncodeSaveFile(args);
            }
        }

        private static void EncodeSaveFile(string[] args)
        {
            var inputFilePath = args[1];

            Gvas save;
            using (var stream = File.Open(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                // Read the json file
                using (var reader = new StreamReader(stream, new UTF8Encoding(false)))
                {
                    var serializerSettings = new JsonSerializerSettings
                    {
                        Converters =
                        [
                            new UEPropertyJsonConverter()
                        ]
                    };

                    var json = reader.ReadToEnd();
                    save = JsonConvert.DeserializeObject<Gvas>(json, serializerSettings);
                }
            }
        }

        private static void DecodeSaveFile(string[] args)
        {
            var inputFilePath = args[1];

            Gvas save;
            using (var stream = File.Open(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                save = UESerializer.Read(stream);
            }

            Console.WriteLine("Converting to json");
            var serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                Converters =
                {
                    new ByteArrayConverter()
                }
            };

            var json = JsonConvert.SerializeObject(save, serializerSettings);

            var outputFilePath = args.Length > 2 ? args[2] : args[1] + ".json";

            using (var stream = File.Open(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.Read))
            {
                using (var writer = new StreamWriter(stream, new UTF8Encoding(false)))
                {
                    writer.Write(json);
                }
            }
        }
    }
}
