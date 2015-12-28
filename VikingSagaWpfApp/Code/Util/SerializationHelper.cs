using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace VikingSaga.Code.Util
{
    public static class SerializationHelper
    {
        public static String Serialize(Object data)
        {

            XmlSerializer xmlSerializer = new XmlSerializer(data.GetType());
            StringWriter sw = new StringWriter();
            xmlSerializer.Serialize(sw, data);
            String xml = sw.GetStringBuilder().ToString();

            return xml;
        }
    }
}
