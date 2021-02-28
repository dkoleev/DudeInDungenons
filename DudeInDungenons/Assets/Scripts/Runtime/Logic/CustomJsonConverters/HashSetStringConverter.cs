using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Runtime.Logic.CustomJsonConverters {
    public class HashSetStringConverter : JsonConverter<HashSet<string>> {
        public override void WriteJson(JsonWriter writer, HashSet<string> value, JsonSerializer serializer) {
            var jo = new JObject(value.Select(s => new JProperty(s, s)));
            jo.WriteTo(writer);
        }

        public override HashSet<string> ReadJson(JsonReader reader,
            Type objectType,
            HashSet<string> existingValue,
            bool hasExistingValue,
            JsonSerializer serializer) {
            
            var jo = JObject.Load(reader);
            
            return new HashSet<string>(jo.Properties().Select(p => p.Name));
        }
    }
}
