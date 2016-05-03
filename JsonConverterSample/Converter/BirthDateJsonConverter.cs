using Newtonsoft.Json;
using System;

namespace JsonConverterSample.Converter
{
    public class BirthDateJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(string));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var birthDate = Convert.ToDateTime(reader.Value.ToString());
            return ((DateTime.Now - birthDate).TotalDays / 365);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            double convertedValue = Convert.ToDouble(value) * 365;
            var birthDate = DateTime.Now.AddDays(-convertedValue);
            writer.WriteValue(birthDate);
        }
    }
}
