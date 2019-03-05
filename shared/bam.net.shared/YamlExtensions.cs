/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Bam.Net;
using System.Reflection;
using System.Collections;
using YamlDotNet.Serialization;

namespace Bam.Net
{
    public static partial class YamlExtensions
    {
        public static string YamlToJson(this string yaml)
        {
            Deserializer deserializer = new Deserializer();
            object dict = deserializer.Deserialize(new StringReader(yaml));
            return dict.ToJson();
        }
        /// <summary>
        /// Serialize the specified object to yaml
        /// </summary>
        /// <param name="val"></param>
        /// <param name="conf"></param>
        /// <returns></returns>
        public static string ToYaml(this object val)
        {
            Serializer serializer = new Serializer();
            return serializer.Serialize(val);
        }

        public static void ToYamlFile(this object val, string path)
        {
            ToYamlFile(val, new FileInfo(path));
        }

        public static void ToYamlFile(this object val, FileInfo file)
        {
            if (!file.Directory.Exists)
            {
                file.Directory.Create();
            }
            using (StreamWriter sw = new StreamWriter(file.FullName))
            {
                sw.Write(val.ToYaml());
            }
        }

        public static object FromYamlFile(this string path)
        {
            Deserializer deserializer = new Deserializer();
            return deserializer.Deserialize(new FileInfo(path).ReadAllText(), typeof(Dictionary<object, object>));
        }

        public static object FromYamlFile(this FileInfo file)
        {
            Deserializer deserializer = new Deserializer();
            return deserializer.Deserialize(new StreamReader(file.FullName));
        }

        public static T FromYamlFile<T>(this string path)
        {
            return FromYamlFile<T>(new FileInfo(path));
        }

        public static T FromYamlFile<T>(this FileInfo fileInfo)
        {
            Deserializer deserializer = new Deserializer();
            return deserializer.Deserialize<T>(fileInfo.ReadAllText());
        }

        public static object FromYaml(this string yaml, Type type)
        {
            Deserializer deserializer = new Deserializer();
            return deserializer.Deserialize(yaml, type);
        }

        public static T FromYaml<T>(this string yaml)
        {
            Deserializer deserializer = new Deserializer();
            return deserializer.Deserialize<T>(yaml);
        }

        public static Stream ToYamlStream(this object value)
        {
            MemoryStream stream = new MemoryStream();
            ToYamlStream(value, stream);
            return stream;
        }

        public static void ToYamlStream(this object value, Stream stream)
        {
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(value.ToYaml());
            writer.Flush();
            stream.Seek(0, SeekOrigin.Begin);
        }

        public static object FromYamlStream(this Stream stream, Type type)
        {
            MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            Deserializer deserializer = new Deserializer();
            using (StreamReader sr = new StreamReader(ms))
            {
                return deserializer.Deserialize(sr);
            }
        }

        public static T FromYamlStream<T>(this Stream stream)
        {
            MemoryStream ms = new MemoryStream();
            stream.CopyTo(ms);
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            using (StreamReader sr = new StreamReader(ms))
            {
                return sr.ReadToEnd().FromYaml<T>();
            }
        }
        
        public static T[] ArrayFromYaml<T>(this string yaml)
        {
            Deserializer deserializer = new Deserializer();
            return (T[])deserializer.Deserialize(yaml, typeof(T[]));
        }
    }
}
