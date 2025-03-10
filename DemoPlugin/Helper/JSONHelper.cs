using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace DemoPlugin.Helper
{
    //Every JSON it pass, it will deserialize this obj and give the value of particular object pass to any function
    public static class JSONHelper // static class loaded immediately so then normal class so 
    {
        public static T Deserialize<T>(string json) // static function (generic func for any kind of object)
        {
            using (var memoryStream = new MemoryStream(Encoding.Unicode.GetBytes(json))) // dispose off  memory stream 
            {
                var serializer = new DataContractJsonSerializer(typeof(T)); // converts json into object
                return (T) serializer.ReadObject(memoryStream);
            }
        }

    }
}
