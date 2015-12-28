using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace VikingSaga.Code.Campaign.PEE.Maps
{
    public class LocationSerialization
    {
        public List<MapLocationData> LocationData = new List<MapLocationData>();
        public List<MapLocationLinkData> LocationLinks = new List<MapLocationLinkData>();

        public static string Serialize(LocationSerialization mapData)
        {
            var serializer = new XmlSerializer(typeof(LocationSerialization));
            var sw = new StringWriter();
            serializer.Serialize(sw, mapData);
            return sw.ToString();
        }

        public static LocationSerialization Deserialize(string data)
        {
            var serializer = new XmlSerializer(typeof(LocationSerialization));
            var result = (LocationSerialization)serializer.Deserialize(new StringReader(data));
            return result;
        }
    }
}
