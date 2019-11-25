using System.Data;
using Dapper;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NzbDrone.Core.Datastore.Converters
{
    public class DapperEmbeddedDocumentConverter<T> : SqlMapper.TypeHandler<T>
    {
        private readonly JsonSerializerOptions SerializerSetting;

        public DapperEmbeddedDocumentConverter()
        {
            SerializerSetting = new JsonSerializerOptions
            {
                AllowTrailingCommas = true,
                IgnoreNullValues = false,
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,

                // DateTimeZoneHandling = DateTimeZoneHandling.Utc,
            };

            SerializerSetting.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, false));
            SerializerSetting.Converters.Add(new TimeSpanConverter());
        }

        public DapperEmbeddedDocumentConverter(params JsonConverter[] converters) : this()
        {
            foreach (var converter in converters)
            {
                SerializerSetting.Converters.Add(converter);
            }
        }

        public override T Parse(object value)
        {
            return JsonSerializer.Deserialize<T>((string) value, SerializerSetting);
        }

        public override void SetValue(IDbDataParameter parameter, T doc)
        {
            parameter.Value = JsonSerializer.Serialize(doc, SerializerSetting);
        }
    }
}
