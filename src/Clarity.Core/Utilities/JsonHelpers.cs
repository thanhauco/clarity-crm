using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Clarity.Core.Utilities 
{ 
    public static class JsonHelpers 
    { 
        private static readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        public static string Serialize(object obj) 
        {
            return JsonConvert.SerializeObject(obj, _settings);
        }

        public static T Deserialize<T>(string json) 
        {
            if (string.IsNullOrWhiteSpace(json)) return default(T);
            return JsonConvert.DeserializeObject<T>(json, _settings);
        }
    } 
}
