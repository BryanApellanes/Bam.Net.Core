using Bam.Net.Data.Dynamic.Data;
using Bam.Net.Data.Dynamic.Data.Dao.Repository;
using Bam.Net.Data.Repositories;
using Bam.Net.Logging;
using Bam.Net.Logging.Counters;
using CsvHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Bam.Net.Data.Dynamic
{
    /// <summary>
    /// A class used to manage dynamic data and generated types.
    /// </summary>
    public class DynamicDataManager: Loggable
    {
        public DynamicDataManager() : this(new DynamicTypeDataRepository(), DataProvider.Current, new DynamicTypeManager())
        { }

        public DynamicDataManager(DynamicTypeDataRepository descriptorRepository, IDataDirectoryProvider directorySettings, DynamicTypeManager dynamicTypeManager)
        {
            DynamicTypeDataRepository = descriptorRepository;
            DataDirectorySettings = directorySettings;
            DynamicTypeManager = dynamicTypeManager;
            DynamicTypeNameResolver = new DynamicTypeNameResolver();
            CsvFileProcessor = new BackgroundThreadQueue<DataFile>()
            {
                Process = (df) =>
                {
                    ProcessCsvFile(df.TypeName, df.FileInfo);
                }
            };
            JsonFileProcessor = new BackgroundThreadQueue<DataFile>()
            {
                Process = (df) =>
                {
                    ProcessJsonFile(df.TypeName, df.FileInfo);
                }
            };
            YamlFileProcessor = new BackgroundThreadQueue<DataFile>()
            {
                Process = (df) =>
                {
                    ProcessYamlFile(df.TypeName, df.FileInfo);
                }
            };
        }

        public List<DynamicDataSaveResult> ProcessDataFiles(DirectoryInfo appData)
        {
            return ProcessDataFiles(appData, DynamicNamespaceDescriptor.DefaultNamespace);
        }

        public List<DynamicDataSaveResult> ProcessDataFiles(DirectoryInfo appData, string nameSpace)
        {
            List<DynamicDataSaveResult> results = new List<DynamicDataSaveResult>();
            results.AddRange(ProcessCsv(appData, nameSpace));
            results.AddRange(ProcessJson(appData, nameSpace));
            results.AddRange(ProcessYaml(appData, nameSpace));
            return results;
        }

        protected DynamicTypeManager DynamicTypeManager { get; set; }
        public IDataDirectoryProvider DataDirectorySettings { get; set; }
        public DynamicTypeNameResolver DynamicTypeNameResolver { get; set; }
        public DynamicTypeDataRepository DynamicTypeDataRepository { get; set; }
        public DirectoryInfo CsvDirectory { get; set; }
        public DirectoryInfo JsonDirectory { get; set; }
        public DirectoryInfo YamlDirectory { get; set; }
        public BackgroundThreadQueue<DataFile> CsvFileProcessor { get; }
        public BackgroundThreadQueue<DataFile> JsonFileProcessor { get; }
        public BackgroundThreadQueue<DataFile> YamlFileProcessor { get; }

        public event EventHandler ProcessingYamlFile;
        public event EventHandler ProcessedYamlFile;

        protected DynamicDataSaveResult ProcessYamlFile(string typeName, FileInfo yamlFile, string nameSpace = null)
        {
            string yaml = yamlFile.ReadAllText();
            string rootHash = yaml.Sha256();
            DynamicTypeDataRepository.SaveAsync(new RootDocument { FileName = yamlFile.Name, Content = yaml, ContentHash = rootHash });
            string json = yaml.YamlToJson();

            JObject jobj = (JObject)JsonConvert.DeserializeObject(json);
            Dictionary<object, object> valueDictionary = jobj.ToObject<Dictionary<object, object>>();
            FireEvent(ProcessingYamlFile, new DynamicDataSaveResultEventArgs { File = yamlFile });
            Timer yamlTimer = Stats.Start($"{typeName}::{yamlFile.FullName}");
            DynamicDataSaveResult saveResult = SaveRootData(rootHash, typeName, valueDictionary, nameSpace);
            Stats.End(yamlTimer, timer => FireEvent(ProcessedYamlFile, new DynamicDataSaveResultEventArgs { Result = saveResult, Timer = timer }));
            return saveResult;
        }

        public event EventHandler ProcessingCsvFile;
        public event EventHandler ProcessedCsvFile;

        protected DynamicDataSaveResult ProcessCsvFile(string typeName, FileInfo csvFile, string nameSpace = null)
        {
            string content = csvFile.ReadAllText();
            string rootHash = content.Sha256();
            DynamicTypeDataRepository.SaveAsync(new RootDocument { FileName = csvFile.Name, Content = content, ContentHash = rootHash });
            using (StreamReader sr = new StreamReader(csvFile.FullName))
            using (CsvReader csvReader = new CsvReader(sr))
            using (CsvDataReader csvDataReader = new CsvDataReader(csvReader))
            {
                DataTable dataTable = new DataTable();
                dataTable.Load(csvDataReader);
                FireEvent(ProcessingCsvFile, new DynamicDataSaveResultEventArgs { File = csvFile });
                Timer csvTimer = Stats.Start($"{typeName}::{csvFile.FullName}");
                DynamicDataSaveResult saveResult = SaveRootData(rootHash, typeName, dataTable.ToDictionaries(), nameSpace);
                Stats.End(csvTimer, timer => FireEvent(ProcessedCsvFile, new DynamicDataSaveResultEventArgs { Result = saveResult, Timer = timer }));
                return saveResult;
            }
        }

        public event EventHandler ProcessingJsonFile;
        public event EventHandler ProcessedJsonFile;

        protected DynamicDataSaveResult ProcessJsonFile(string typeName, FileInfo jsonFile, string nameSpace = null)
        {
            string json = jsonFile.ReadAllText();
            string rootHash = json.Sha256();
            DynamicTypeDataRepository.SaveAsync(new RootDocument { FileName = jsonFile.Name, Content = json, ContentHash = rootHash });
            JObject jobj = (JObject)JsonConvert.DeserializeObject(json);
            Dictionary<object, object> valueDictionary = jobj.ToObject<Dictionary<object, object>>();
            FireEvent(ProcessingJsonFile, new DynamicDataSaveResultEventArgs { File = jsonFile });
            Timer jsonTimer = Stats.Start($"{typeName}::{jsonFile.FullName}");
            DynamicDataSaveResult saveResult = SaveRootData(rootHash, typeName, valueDictionary, nameSpace);
            Stats.End(jsonTimer, timer => FireEvent(ProcessedJsonFile, new DynamicDataSaveResultEventArgs { Result = saveResult, Timer = timer }));
            return saveResult;
        }

        protected internal DynamicDataSaveResult SaveRootData(string rootHash, string typeName, List<Dictionary<object, object>> valueDictionary, string nameSpace = null)
        {
            DynamicDataSaveResult result = new DynamicDataSaveResult()
            {
                DynamicTypeDescriptor = DynamicTypeManager.SaveTypeDescriptor(typeName, valueDictionary.First(), nameSpace),
                DataInstances = valueDictionary.Select(d => SaveDataInstance(rootHash, typeName, d)).ToList()
            };
            return result;
        }

        protected DataInstance SaveDataInstance(string rootHash, string typeName, Dictionary<object, object> valueDictionary)
        {
            return SaveDataInstance(rootHash, rootHash, typeName, valueDictionary);
        }

        static Dictionary<string, object> _parentLocks = new Dictionary<string, object>();
        protected DataInstance SaveDataInstance(string rootHash, string parentHash, string typeName, Dictionary<object, object> valueDictionary)
        {
            string instanceHash = valueDictionary.ToJson().Sha256();
            DynamicTypeDescriptor typeDescriptor = DynamicTypeManager.GetDynamicTypeDescriptor(typeName, out DynamicNamespaceDescriptor dynamicNamespaceDescriptor);
            DataInstance data = DynamicTypeDataRepository.DataInstancesWhere(di => di.Instancehash == instanceHash && di.ParentHash == parentHash && di.TypeName == typeName).FirstOrDefault();
            if (data == null)
            {
                _parentLocks.AddMissing(parentHash, new object());
                lock (_parentLocks[parentHash])
                {
                    data = DynamicTypeDataRepository.Save(new DataInstance
                    {
                        TypeName = typeName,
                        RootHash = rootHash,
                        ParentHash = parentHash,
                        Instancehash = instanceHash,
                        Properties = new List<DataInstancePropertyValue>()
                    });
                }
            }

            foreach (object key in valueDictionary.Keys)
            {
                object value = valueDictionary[key];
                if (value != null)
                {
                    Type childType = value.GetType();
                    string childTypeName = $"{typeName}.{key}";
                    // 3. for each property where the type is JObject
                    //      - repeat from 1
                    if (childType == typeof(JObject))
                    {
                        SaveJObjectData(childTypeName, rootHash, instanceHash, (JObject)value);
                    }
                    // 4. for each property where the type is JArray
                    //      foreach object in jarray
                    //          - repeat from 1
                    else if (childType == typeof(JArray))
                    {
                        foreach (object obj in (JArray)value)
                        {
                            if (obj is JObject jobj)
                            {
                                SaveJObjectData(childTypeName, rootHash, instanceHash, jobj);
                            }
                            else
                            {
                                data.Properties.Add(DynamicTypeDataRepository.Save(new DataInstancePropertyValue
                                {
                                    RootHash = rootHash,
                                    InstanceHash = instanceHash,
                                    ParentTypeName = typeName,
                                    PropertyName = key.ToString(),
                                    Value = obj.ToString()
                                }));
                            }
                        }
                    }
                    else
                    {
                        data.Properties.Add(DynamicTypeDataRepository.Save(new DataInstancePropertyValue
                        {
                            RootHash = rootHash,
                            InstanceHash = instanceHash,
                            ParentTypeName = typeName,
                            PropertyName = key.ToString(),
                            Value = value.ToString()
                        }));
                    }
                }
            }

            return DynamicTypeDataRepository.Save(data);
        }

        public List<DynamicDataSaveResult> ProcessCsv(DirectoryInfo appData, string nameSpace = null)
        {
            DirectoryInfo csvDirectory = new DirectoryInfo(Path.Combine(appData.FullName, "csv"));
            List<DynamicDataSaveResult> results = new List<DynamicDataSaveResult>();
            foreach (FileInfo csvFile in csvDirectory.GetFiles("*.csv"))
            {
                string typeName = Path.GetFileNameWithoutExtension(csvFile.Name);
                results.Add(ProcessCsvFile(typeName, csvFile, nameSpace ?? DynamicNamespaceDescriptor.DefaultNamespace));
            }
            return results;
        }

        public List<DynamicDataSaveResult> ProcessJson(DirectoryInfo appData, string nameSpace = null)
        {
            DirectoryInfo jsonDirectory = new DirectoryInfo(Path.Combine(appData.FullName, "json"));
            List<DynamicDataSaveResult> results = new List<DynamicDataSaveResult>();
            foreach (FileInfo jsonFile in jsonDirectory.GetFiles("*.json"))
            {
                string typeName = DynamicTypeNameResolver.ResolveJsonTypeName(jsonFile.ReadAllText());
                results.Add(ProcessJsonFile(typeName, jsonFile, nameSpace ?? DynamicNamespaceDescriptor.DefaultNamespace));
            }
            return results;
        }

        public List<DynamicDataSaveResult> ProcessYaml(DirectoryInfo appData, string nameSpace = null)
        {
            DirectoryInfo yamlDirectory = new DirectoryInfo(Path.Combine(appData.FullName, "yaml"));
            List<DynamicDataSaveResult> results = new List<DynamicDataSaveResult>();
            foreach (FileInfo yamlFile in yamlDirectory.GetFiles("*.yaml"))
            {
                string typeName = DynamicTypeNameResolver.ResolveYamlTypeName(yamlFile.ReadAllText());
                results.Add(ProcessYamlFile(typeName, yamlFile, nameSpace ?? DynamicNamespaceDescriptor.DefaultNamespace));
            }
            return results;
        }

        public void ProcessYaml(string yaml)
        {
            ProcessYaml(DynamicTypeNameResolver.ResolveYamlTypeName(yaml), yaml);
        }

        public void ProcessJson(string json)
        {
            ProcessJson(DynamicTypeNameResolver.ResolveJsonTypeName(json), json);
        }

        public void ProcessJson(string typeName, string json)
        {
            string filePath = WriteJsonFile(json);
            JsonFileProcessor.Enqueue(new DataFile { FileInfo = new FileInfo(filePath), TypeName = typeName });
        }

        public void ProcessYaml(string typeName, string yaml)
        {
            string filePath = Path.Combine(YamlDirectory.FullName, $"{yaml.Sha512()}.yaml").GetNextFileName();
            yaml.SafeWriteToFile(filePath);
            YamlFileProcessor.Enqueue(new DataFile { FileInfo = new FileInfo(filePath), TypeName = typeName });
        }

        protected string WriteJsonFile(string json)
        {
            string filePath = Path.Combine(JsonDirectory.FullName, $"{json.Sha512()}.json").GetNextFileName();
            json.SafeWriteToFile(filePath);
            return filePath;
        }

        protected DynamicDataSaveResult SaveRootData(string rootHash, string typeName, Dictionary<object, object> valueDictionary, string nameSpace = null)
        {
            DynamicDataSaveResult result = new DynamicDataSaveResult(
                DynamicTypeManager.SaveTypeDescriptor(typeName, valueDictionary, nameSpace),
                SaveDataInstance(rootHash, typeName, valueDictionary)
            );
            return result;
        }

        private void SaveJObjectData(string typeName, string rootHash, string parentHash, JObject value)
        {
            Dictionary<object, object> valueDictionary = value.ToObject<Dictionary<object, object>>();
            SaveObjectData(typeName, rootHash, parentHash, valueDictionary);
        }

        private void SaveObjectData(string typeName, string rootHash, string parentHash, Dictionary<object, object> valueDictionary)
        {
            DynamicTypeManager.SaveTypeDescriptor(typeName, valueDictionary);
            SaveDataInstance(rootHash, parentHash, typeName, valueDictionary);
        }

        private void EnsureDataDirectories(IDataDirectoryProvider settings)
        {
            CsvDirectory = settings.GetRootDataDirectory(nameof(DynamicTypeManager), "csv");
            if (!CsvDirectory.Exists)
            {
                CsvDirectory.Create();
            }
            JsonDirectory = settings.GetRootDataDirectory(nameof(DynamicTypeManager), "json");
            if (!JsonDirectory.Exists)
            {
                JsonDirectory.Create();
            }
            YamlDirectory = settings.GetRootDataDirectory(nameof(DynamicTypeManager), "yaml");
            if (!YamlDirectory.Exists)
            {
                YamlDirectory.Create();
            }
        }
    }
}
