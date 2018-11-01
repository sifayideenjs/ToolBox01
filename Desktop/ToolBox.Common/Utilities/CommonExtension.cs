using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.Common.Utilities
{
    public static class CommonExtension
    {
        public static T Deserialize<T>(this string json)
        {
            T obj = Activator.CreateInstance<T>();
            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(json));
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            obj = (T)serializer.ReadObject(ms);
            ms.Close();
            return obj;
        }

        public static T JSONDeSerialize<T>(this string obj)
        {
            var stringToParse = obj;
            stringToParse = stringToParse.Trim('\"');
            stringToParse = stringToParse.Replace("\\", "");
            return JsonConvert.DeserializeObject<T>(stringToParse, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
    }
}
