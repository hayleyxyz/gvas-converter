using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace GvasConverter
{
    internal class ByteArrayConverter : JsonConverter<byte[]>
    {
        public override byte[] ReadJson(JsonReader reader, Type objectType, byte[] existingValue, bool hasExistingValue, Newtonsoft.Json.JsonSerializer serializer)
        {
            var token = JArray.Load(reader);

            var bytes = new byte[token.Count];

            for (int i = 0; i < token.Count; i++)
            {
                bytes[i] = token[i].Value<byte>();
            }

            return bytes;
        }

        public override void WriteJson(JsonWriter writer, byte[] value, Newtonsoft.Json.JsonSerializer serializer)
        {
            writer.WriteStartArray();

            foreach (var b in value)
            {
                writer.WriteValue(b);
            }

            writer.WriteEndArray();
        }
    }
}
