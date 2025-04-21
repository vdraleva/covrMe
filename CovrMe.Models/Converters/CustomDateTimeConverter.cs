using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovrMe.Models.Converters
{
    public class CustomDateTimeConverter : JsonConverter<DateTime?>
    {
        private readonly string _dateFormat;
        private readonly CultureInfo _culture;

        public CustomDateTimeConverter(string dateFormat, string cultureName)
        {
            _dateFormat = dateFormat;
            _culture = new CultureInfo(cultureName);
        }

        public override void WriteJson(JsonWriter writer, DateTime? value, JsonSerializer serializer)
        {
            if (value.HasValue)
            {
                writer.WriteValue(value.Value.ToString(_dateFormat, _culture));
            }
            else
            {
                writer.WriteNull();
            }
        }

        public override DateTime? ReadJson(JsonReader reader, Type objectType, DateTime? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            if (reader.TokenType == JsonToken.Date)
            {
                var dateString = ((DateTime)reader.Value).ToString();
                if (DateTime.TryParseExact(dateString, _dateFormat, _culture, DateTimeStyles.None, out DateTime date))
                {
                    var result = date.ToLocalTime();
                    return result;
                }
            }

            throw new JsonSerializationException($"Невалиден формат на дата: {reader.Value}");
        }
    }
}
