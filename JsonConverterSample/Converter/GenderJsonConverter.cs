using JsonConverterSample.Model;
using Newtonsoft.Json;
using System;

namespace JsonConverterSample.Converter
{
    public class GenderJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Gender gender = Gender.Unknown;
            Enum.TryParse(reader.Value.ToString(), out gender);

            return gender;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}
