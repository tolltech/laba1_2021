using System;

namespace Laba1
{
    class Program
    {
        static void Main(string[] args)
        {
            var ssdDisk = new SsdDisk
            {
                DiskSizeMb = 500,
                DataTransferRateMbps = 2000,
                FormFactor = FormFactor.M2,
                Model = "Panterra Ultra Soft"
            };

            Console.WriteLine($"Несериализованный объект {ssdDisk}");

            var serializer = new Serializer();
            var ssdDiskSerialized = serializer.SerializeJson(ssdDisk);

            Console.WriteLine($"Cериализованный объект {ssdDiskSerialized}");

            var deserializedDisk = serializer.DeserializeJson<SsdDisk>(ssdDiskSerialized);

            Console.WriteLine($"Десериализованный диск" +
                              $"DTR - {deserializedDisk.DataTransferRateMbps} \n" +
                              $"DS - {deserializedDisk.DiskSizeMb} \n" +
                              $"FF - {deserializedDisk.FormFactor}\n" +
                              $"M - {deserializedDisk.Model}.");

            var ssdDiskSerializedXml = serializer.SerializeXml(ssdDisk);

            Console.WriteLine($"Cериализованный объект XML {ssdDiskSerializedXml}");

            var deserializedDiskXml = serializer.DeserializeXml<SsdDisk>(ssdDiskSerializedXml);

            Console.WriteLine($"Десериализованный диск XML \n" +
                              $"DTR - {deserializedDiskXml.DataTransferRateMbps} \n" +
                              $"DS - {deserializedDiskXml.DiskSizeMb} \n" +
                              $"FF - {deserializedDiskXml.FormFactor}\n" +
                              $"M - {deserializedDiskXml.Model}.");
        }
    }
}