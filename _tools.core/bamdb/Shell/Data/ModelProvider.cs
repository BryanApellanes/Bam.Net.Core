using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bam.Net;
using Bam.Net.Application;

namespace Bam.Shell.Data
{
    public class ModelProvider : DataShellProvider
    {
        public override void New(Action<string> output = null, Action<string> error = null)
        {
            string modelArg = GetArgument("name", "Enter the name of the model to add.");
            AppDataModel dataModel = ParseDataModelArgument(modelArg);
            DirectoryInfo projectParent = ShellProvider.FindProjectParent(out FileInfo csprojFile);
            if (csprojFile == null)
            {
                OutLine("Can't find csproj file", ConsoleColor.Magenta);
                Exit(1);
            }
            WriteDataModelDefinition(csprojFile, dataModel);
            Bam.Shell.CodeGen.ModelProvider.GenerateDataModels();
        }

        public override void Get(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }

        public override void Set(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }

        public override void Del(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }

        public override void Find(Action<string> output = null, Action<string> error = null)
        {
            throw new NotImplementedException();
        }
                
        private void WriteDataModelDefinition(FileInfo csprojFile, AppDataModel appDataModel)
        {
            WriteDataModelDefinition(csprojFile, appDataModel.Name, appDataModel.Properties.ToArray());
        }
        
        private AppDataModel WriteDataModelDefinition(FileInfo csprojFile, string dataModelName, params AppDataPropertyModel[] properties)
        {
            AppDataModel model = Bam.Shell.CodeGen.ModelProvider.ReadDataModelDefinition(csprojFile, dataModelName, out FileInfo dataModelFile);
            List<AppDataPropertyModel> props = new List<AppDataPropertyModel>();
            foreach(AppDataPropertyModel prop in properties)
            {
                props.Add(prop);
            }
            model.Properties = props.ToArray();
            model.ToYamlFile(dataModelFile);
            return model;
        }
        
        public static AppDataModel ParseDataModelArgument(string modelArg)
        {
            string[] split = modelArg.DelimitSplit(",", true);
            string modelName = split[0];
            AppDataModel dataModel = new AppDataModel { Name = modelName };
            int num = 0;
            List<AppDataPropertyModel> props = new List<AppDataPropertyModel>();
            split.Rest(1, (modelProperty) =>
            {
                string[] parts = modelProperty.DelimitSplit(":", true);
                bool key = false;
                string type = "string";
                string name = $"_Property_{++num}";
                if (parts.Length == 2)
                {
                    type = parts[0];
                    name = parts[1];
                }
                else if (parts.Length == 3)
                {
                    type = parts[1];
                    name = parts[2];
                    string[] keyParts = parts[0].DelimitSplit("=", true);
                    if (keyParts.Length != 2)
                    {
                        OutLineFormat("Unrecognized key specification {0}: expected format key=[true|false].", ConsoleColor.Yellow, parts[0]);
                    }
                    else
                    {
                        key = keyParts[1].IsAffirmative();
                    }
                }
                props.Add(new AppDataPropertyModel
                {
                    Key = key,
                    Type = type,
                    Name = name
                });
            });
            dataModel.Properties = props.ToArray();
            return dataModel;
        }
    }
}