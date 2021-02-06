using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace EditorPlugins.Engine
{
    internal static class Utils
    {
        public static string AppDataPath => Path.Combine(Environment.GetEnvironmentVariable("LocalAppData"), "GK.Common");

        public static string ConfigFile() => Path.Combine(AppDataPath, "pluginsettings.json");

        public static T FromJson<T>(string json)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                return (T)serializer.ReadObject(memoryStream);
        }

        public static List<T> FromJsonToList<T>(string json)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<T>));
            using (MemoryStream memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                return (List<T>)serializer.ReadObject(memoryStream);
        }

        public static bool HasInterface(Type type, Type interfacetype) => (type.GetInterfaces().Where(p => p == interfacetype).FirstOrDefault() != null);


        public static bool IsSimpleType(Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            return type.IsPrimitive || type.IsEnum || type.Equals(typeof(string)) || type.Equals(typeof(decimal));
        }

        public static string ToJson<T>(IEnumerable<T> items)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(IEnumerable<T>));
                serializer.WriteObject(stream, items);
                stream.Position = 0;
                using (StreamReader streamReader = new StreamReader(stream))
                    return streamReader.ReadToEnd();
            }
        }

        public static string ToJson<T>(T items)
        {
            using (MemoryStream stream = new MemoryStream()) 
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                serializer.WriteObject(stream, items);
                stream.Position = 0;
                using (StreamReader streamReader = new StreamReader(stream))
                    return streamReader.ReadToEnd();
            }
        }
    }
}