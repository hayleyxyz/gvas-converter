using GvasFormat.Serialization.UETypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using static GvasFormat.Serialization.UETypes.UEMapProperty;

namespace GvasConverter
{
    internal class UEPropertyJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(UEProperty).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jsonObject = JObject.Load(reader);
            string typeName = jsonObject["Type"].Value<string>();

            if (typeName == null)
            {
                return default(object);
            }

            UEProperty ueProperty = typeName switch
            {
                "ArrayProperty" => new UEArrayProperty(),
                "EnumProperty" => new UEEnumProperty(),
                "Int8Property" => new UEInt8Property(),
                "BoolProperty" => new UEBoolProperty(),
                "FloatProperty" => new UEFloatProperty(),
                "IntProperty" => new UEIntProperty(),
                "NameProperty" => new UEStringProperty(),
                "StrProperty" => new UEStringProperty(),
                "ByteProperty" => new UEByteProperty(),
                "GenericStructProperty" => new UEGenericStructProperty(),
                "LinearColorStructProperty" => new UELinearColorStructProperty(),
                "StructProperty" => new UEStructProperty(),
                "GuidStructProperty" => new UEGuidStructProperty(),
                "MapProperty" => new UEMapProperty(),
                "TextProperty" => new UETextProperty(),
                "DateTimeStructProperty" => new UEDateTimeStructProperty(),
                "Int64Property" => new UEInt64Property(),
                "NoneProperty" => new UENoneProperty(),
                "VectorStructProperty" => new UEVectorStructProperty(),
                _ => throw new InvalidOperationException($"Unknown property type: {typeName}")
            };

            ueProperty.Name = jsonObject["Name"].Value<string>();


            if (ueProperty is UEBoolProperty boolProperty)
            {
                boolProperty.Value = jsonObject["Value"].Value<bool>();
            }
            else if (ueProperty is UEByteProperty byteProperty)
            {
                byteProperty.Value = jsonObject["Value"].Value<byte[]>();
            }
            else if (ueProperty is UEArrayProperty arrayProperty)
            {
                arrayProperty.Items = jsonObject["Items"].ToObject<List<UEProperty>>(serializer).ToArray();
            }
            else if (ueProperty is UEEnumProperty enumProperty)
            {
                enumProperty.Value = jsonObject["Value"].Value<string>();
            }
            else if (ueProperty is UEInt8Property int8Property)
            {
                int8Property.Value = jsonObject["Value"].Value<sbyte>();
            }
            else if (ueProperty is UEFloatProperty floatProperty)
            {
                floatProperty.Value = jsonObject["Value"].Value<float>();
            }
            else if (ueProperty is UEIntProperty intProperty)
            {
                intProperty.Value = jsonObject["Value"].Value<int>();
            }
            else if (ueProperty is UEStringProperty stringProperty)
            {
                stringProperty.Value = jsonObject["Value"].Value<string>();
            }
            else if (ueProperty is UEGenericStructProperty genericStructProperty)
            {
                genericStructProperty.Properties = jsonObject["Value"].ToObject<List<UEProperty>>(serializer);
            }
            else if (ueProperty is UELinearColorStructProperty linearColorStructProperty)
            {
                linearColorStructProperty.R = jsonObject["Value"]["R"].Value<float>();
                linearColorStructProperty.G = jsonObject["Value"]["G"].Value<float>();
                linearColorStructProperty.B = jsonObject["Value"]["B"].Value<float>();
                linearColorStructProperty.A = jsonObject["Value"]["A"].Value<float>();
            }
            else if (ueProperty is UEStructProperty structProperty)
            {
                structProperty.Properties = jsonObject["Properties"].ToObject<List<UEProperty>>(serializer);
            }
            else if (ueProperty is UEGuidStructProperty guidStructProperty)
            {
                guidStructProperty.Value = new Guid(jsonObject["Value"].Value<byte[]>());
            }
            else if (ueProperty is UEMapProperty mapProperty)
            {
                mapProperty.Map = jsonObject["Map"].ToObject<List<UEKeyValuePair>>(serializer);
            }
            else if (ueProperty is UETextProperty textProperty)
            {
                textProperty.Value = jsonObject["Value"].Value<string>();
            }
            else if (ueProperty is UEDateTimeStructProperty dateTimeStructProperty)
            {
                dateTimeStructProperty.Value = jsonObject["Value"].Value<DateTime>();
            }
            else if (ueProperty is UEInt64Property int64Property)
            {
                int64Property.Value = jsonObject["Value"].Value<long>();
            }
            else if (ueProperty is UENoneProperty noneProperty)
            {
                // Do nothing
            }
            else if (ueProperty is UEVectorStructProperty vectorStructProperty)
            {
                vectorStructProperty.X = jsonObject["Value"]["X"].Value<float>();
                vectorStructProperty.Y = jsonObject["Value"]["Y"].Value<float>();
                vectorStructProperty.Z = jsonObject["Value"]["Z"].Value<float>();
            }
            else
            {
                throw new InvalidOperationException($"Unknown property type: {typeName}");
            }

            return ueProperty;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
