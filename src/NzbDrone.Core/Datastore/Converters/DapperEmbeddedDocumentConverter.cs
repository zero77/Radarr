using System.Data;
using Dapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace NzbDrone.Core.Datastore.Converters
{
    public class DapperEmbeddedDocumentConverter<T> : SqlMapper.TypeHandler<T>
    {
        private readonly JsonSerializerSettings SerializerSetting;

        public DapperEmbeddedDocumentConverter()
        {
            SerializerSetting = new JsonSerializerSettings
            {
                DateTimeZoneHandling = DateTimeZoneHandling.Utc,
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                DefaultValueHandling = DefaultValueHandling.Include,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            SerializerSetting.Converters.Add(new StringEnumConverter { NamingStrategy = new CamelCaseNamingStrategy() });
            SerializerSetting.Converters.Add(new VersionConverter());
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
            return JsonConvert.DeserializeObject<T>((string) value, SerializerSetting);
        }

        public override void SetValue(IDbDataParameter parameter, T doc)
        {
            parameter.Value = JsonConvert.SerializeObject(doc, SerializerSetting);
        }
    }
}
