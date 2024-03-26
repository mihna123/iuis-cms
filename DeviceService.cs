using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace CMSystem
{
    public class DeviceService
    {
        private static readonly string filename = "data/devices.xml";
        private static DeviceService instance;
        private List<Device> devices;

        private DeviceService()
        {
            devices = LoadDevices();
        }

        public static DeviceService GetInstance()
        {
            if (instance == null)
            {
                instance = new DeviceService();
            }

            return instance;
        }

        public List<Device> GetDevices()
        {
            return devices;
        }

        public void AddDevice(Device dev)
        {
            if (dev.ID == -1)
            {
                dev.ID = GetId();
            }
            else
            {
                var device = devices.Find(d => d.ID == dev.ID);
                if (device != null)
                {
                    device = dev;
                    SaveDevices();
                    return;
                }
            }

            this.devices.Add(dev);
            SaveDevices();
        }

        public void RemoveDevicesWithIds(ICollection<int> ids)
        {
            var n = 0;

            foreach (var id in ids)
            {
                n += this.devices.RemoveAll(d => d.ID == id);
            }

            if (n > 0)
            {
                SaveDevices();
            }
        }

        public void SaveDevices()
        {
            try
            {
                using (var writer = new StreamWriter(filename))
                {
                    var serializer = new XmlSerializer(typeof(List<Device>));
                    serializer.Serialize(writer, this.devices);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Couln't save devices");
                Console.Error.WriteLine(e.Message);
                Console.Error.WriteLine(e.StackTrace);
            }
        }

        public List<Device> LoadDevices()
        {
            try
            {
                using (var reader = new StreamReader(filename))
                {
                    var serializer = new XmlSerializer(typeof(List<Device>));
                    return serializer.Deserialize(reader) as List<Device>;
                }
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return new List<Device>();
            }
        }

        public string GetRtfPath(Device d)
        {
            if (d.ID == -1)
            {
                d.ID = GetId();
            }
            return string.Format("data/device{0}.rtf", d.ID);
        }

        private int GetId()
        {
            int id = devices.Count;
            while (devices.Exists(dev => dev.ID == id))
            {
                id++;
            }

            return id;
        }
    }
}
