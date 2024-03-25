using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace CMSystem
{
    public class IOService
    {
        private static readonly string filename = "data/devices.xml";

        public void SaveDevices(List<Device> devices)
        {
            var serializer = new XmlSerializer(typeof(List<Device>));
            var writer = new StreamWriter(filename);
            serializer.Serialize(writer, devices);
        }

        public List<Device> LoadDevices()
        {
            var serializer = new XmlSerializer(typeof(List<Device>));
            var reader = new StreamReader(filename);
            return serializer.Deserialize(reader) as List<Device>;
        }
    }
}
