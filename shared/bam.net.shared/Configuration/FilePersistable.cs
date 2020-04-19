using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;
using YamlDotNet.Serialization;

namespace Bam.Net.Configuration
{
    public abstract class FilePersistable : IFilePersistable
    {
        public abstract ulong GetUniversalDeterministicId();
        
        public static T Load<T>(string filePath) where T : FilePersistable, new()
        {
            T instance = new T();
            instance.Load(typeof(T), filePath);
            return instance;
        }
        
        [Exclude]
        [XmlIgnore]
        [YamlIgnore]
        [JsonIgnore]
        public SerializationFormat Format { get; set; }

        public void Save()
        {
            Type currentType = GetType();
            SerializationFormat format = Format;
            if (format == SerializationFormat.Invalid)
            {
                format = SerializationFormat.Yaml;
            }

            ulong id = GetUniversalDeterministicId();
            Args.ThrowIf<InvalidOperationException>(id <= 0, "Unable to get universal deterministic id for type {0}", currentType.FullName);
            FileInfo file = Workspace.ForProcess().File($"{currentType.FullName}", $"{GetUniversalDeterministicId()}.{format.ToString().ToLowerInvariant()}");
            Save(file.FullName);
        }
        
        public void Save(string filePath)
        {
            switch (Format)
            {
                case SerializationFormat.Invalid:
                case SerializationFormat.Yaml:
                    this.ToYamlFile(filePath);
                    break;
                case SerializationFormat.Json:
                    this.ToJsonFile(filePath);
                    break;
                case SerializationFormat.Xml:
                    this.ToXmlFile(filePath);
                    break;
                case SerializationFormat.Binary:
                    this.ToBinaryFile(filePath);
                    break;
            }
        }

        public void Load(Type type, string filePath)
        {
            switch (Format)
            {
                case SerializationFormat.Invalid:
                case SerializationFormat.Yaml:
                    LoadYaml(type, filePath);
                    break;
                case SerializationFormat.Json:
                    LoadJson(type, filePath);
                    break;
                case SerializationFormat.Xml:
                    LoadXml(type, filePath);
                    break;
                case SerializationFormat.Binary:
                    LoadBinary(type, filePath);
                    break;
            }
        }
        
        public void LoadYaml(Type type, string filePath)
        {
            this.CopyProperties(filePath.FromYamlFile(type));
        }

        public void LoadJson(Type type, string filePath)
        {
            this.CopyProperties(filePath.FromJsonFile(type));
        }

        public void LoadXml(Type type, string filePath)
        {
            this.CopyProperties(filePath.FromXmlFile(type));
        }

        public void LoadBinary(Type type, string filePath)
        {
            this.CopyProperties(filePath.FromBinaryFile(type));
        }
    }
}