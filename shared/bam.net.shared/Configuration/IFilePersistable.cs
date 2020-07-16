using System;
using Bam.Net.Data;

namespace Bam.Net.Configuration
{
    public interface IFilePersistable: IHasUniversalDeterministicId
    {
        SerializationFormat Format { get; set; }
        void Save(string filePath);
        void Load(Type type, string filePath);
        void LoadYaml(Type type, string filePath);
        void LoadJson(Type type, string filePath);
        void LoadXml(Type type, string filePath);
        void LoadBinary(Type type, string filePath);
    }
}