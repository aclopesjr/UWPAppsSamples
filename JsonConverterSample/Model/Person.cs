using JsonConverterSample.Converter;
using Newtonsoft.Json;

namespace JsonConverterSample.Model
{
    public class Person
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("height"), JsonConverter(typeof(HeightJsonConverter))]
        public double Height { get; set; }

        [JsonProperty("birthDate"), JsonConverter(typeof(BirthDateJsonConverter))]
        public double Age { get; set; }

        [JsonProperty("gender"), JsonConverter(typeof(GenderJsonConverter))]
        public Gender Gender { get; set; }
    }

    public enum Height
    {
        VeryShort, Short, Normal, Tall, VeryTall
    }
    public enum Gender
    {
        Male, Female, Unknown
    }
}
