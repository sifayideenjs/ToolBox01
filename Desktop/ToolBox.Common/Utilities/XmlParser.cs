using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ToolBox.Common.Utilities
{
    public class XmlParser
    {
        public T ConvertFromXml<T>(string xml)
        {
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(new StringReader(xml));
        }

        public void ConvertToXml<T>(T instance, string fileName)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(fileStream, instance);
            }
        }
    }
}
