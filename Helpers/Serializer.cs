using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace BigfootDNN.Helpers
{
    public class Serializer
    {
        #region "Serilizers"

        //public static string ToXml(object data)
        //{
        //    var ser = new SoapFormatter();
        //    using (var ms = new MemoryStream())
        //    {
        //        ser.Serialize(ms, data);
        //        ms.Flush();
        //        ms.Position = 0;
        //        using (var sr = new StreamReader(ms))
        //        {
        //            return sr.ReadToEnd();
        //        }
        //    }
        //}

        //public static object FromXml(string data)
        //{
        //    var ser = new SoapFormatter();
        //    using (var ms = new MemoryStream())
        //    {
        //        using (var sw = new StreamWriter(ms))
        //        {
        //            sw.Write(data);
        //            sw.Flush();
        //            ms.Position = 0;
        //            return ser.Deserialize(ms);
        //        }
        //    }

        //}

        public static string ToJson(object data)
        {
            return ToJson(data, false);
        }

        public static string ToJson(object data, bool ConvertToBase64)
        {
            var json = JSONHelper.Serialize(data); //ser.Serialize(data);

            if (ConvertToBase64) json = ToBase64(json);
            return json;
        }

        public static T FromJson<T>(string data)
        {
            return FromJson<T>(data, false);
        }

        public static T FromJson<T>(string data, bool ConvertFromBase64)
        {
            if (ConvertFromBase64) data = FromBase64(data);

            return JSONHelper.Deserialize<T>(data); 
        }

        public static string ToBase64(string data)
        {
            var enc = new UTF8Encoding();
            return Convert.ToBase64String(enc.GetBytes(data));
        }

        public static string FromBase64(string data)
        {
            var enc = new UTF8Encoding();
            return enc.GetString(Convert.FromBase64String(data));
        }

        public static string ToBinary(object data)
        {
            var ser = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                ser.Serialize(ms, data);
                ms.Flush();
                ms.Position = 0;
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public static T FromBinary<T>(string base64string)
        {
            using (var ms = new MemoryStream(Convert.FromBase64String(base64string)))
            {
                ms.Position = 0;
                var ser = new BinaryFormatter();
                return (T)ser.Deserialize(ms);
            }
        }

        #endregion

        public class JSONHelper
        {
            public static string Serialize<T>(T obj)
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
                using (var ms = new MemoryStream())
                {
                    serializer.WriteObject(ms, obj);
                    string retVal = Encoding.Default.GetString(ms.ToArray());                    
                    return retVal;
                }
            }

            public static T Deserialize<T>(string json)
            {
                T obj = Activator.CreateInstance<T>();
                using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
                {
                    System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
                    obj = (T)serializer.ReadObject(ms);
                    return obj;    
                }
            }
        }
    }
}
