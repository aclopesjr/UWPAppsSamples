using JsonConverterSample.Model;
using Newtonsoft.Json;
using System;

namespace JsonConverterSample.Converter
{
    public class HeightJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType.Equals(typeof(string));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Height result = Height.Normal;
            Enum.TryParse(reader.Value.ToString(), out result);
            switch (result)
            {
                case Height.VeryShort:
                    return 1.50;
                case Height.Short:
                    return 1.60;
                case Height.Tall:
                    return 1.80;
                case Height.VeryTall:
                    return 1.90;
                case Height.Normal:
                default:
                    return 1.70;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            double convertedValue = Convert.ToDouble(value);
            if (convertedValue < 1.6)
                writer.WriteValue(Height.VeryShort.ToString());
            else if (convertedValue < 1.7)
                writer.WriteValue(Height.Short.ToString());
            else if (convertedValue < 1.8)
                writer.WriteValue(Height.Normal.ToString());
            else if (convertedValue < 1.9)
                writer.WriteValue(Height.Tall.ToString());
            else
                writer.WriteValue(Height.VeryTall.ToString());
        }
    }
}
