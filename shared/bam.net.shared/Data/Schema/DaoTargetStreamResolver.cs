using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Bam.Net.Data.Schema
{
    public class DaoTargetStreamResolver: IDaoTargetStreamResolver
    {
        public Stream GetTargetContextStream(Func<string, Stream> targetResolver, string root, SchemaDefinition schema)
        {
            string parameterValue = string.Format("{0}Context", schema.Name);
            return GetTargetStream(targetResolver, root, parameterValue);
        }

        public Stream GetTargetClassStream(Func<string, Stream> targetResolver, string root, Table table)
        {
            string parameterValue = table.ClassName;
            return GetTargetStream(targetResolver, root, parameterValue);
        }

        public Stream GetTargetQueryClassStream(Func<string, Stream> targetResolver, string root, Table table)
        {
            string paramaterValue = string.Format("{0}Query", table.ClassName);
            return GetTargetStream(targetResolver, root, paramaterValue);
        }

        public Stream GetTargetPagedQueryClassStream(Func<string, Stream> targetResolver, string root, Table table)
        {
            string paramaterValue = string.Format("{0}PagedQuery", table.ClassName);
            return GetTargetStream(targetResolver, root, paramaterValue);
        }

        public Stream GetTargetQiClassStream(Func<string, Stream> targetResolver, string root, Table table)
        {
            string paramaterValue = string.Format("Qi/{0}", table.ClassName);
            return GetTargetStream(targetResolver, root, paramaterValue);
        }

        public Stream GetTargetCollectionStream(Func<string, Stream> targetResolver, string root, Table table)
        {
            string parameterValue = string.Format("{0}Collection", table.ClassName);
            return GetTargetStream(targetResolver, root, parameterValue);
        }

        public Stream GetTargetColumnsClassStream(Func<string, Stream> targetResolver, string root, Table table)
        {
            string parameterValue = string.Format("{0}Columns", table.ClassName);
            return GetTargetStream(targetResolver, root, parameterValue);
        }

        public Stream GetTargetPartialClassStream(Func<string, Stream> targetResolver, string root, Table table)
        {
            if(targetResolver != null)
            {
                return targetResolver($"{table.Name}_Partial");
            }
            string path = Path.Combine(root, "Partials", $"{table.Name}.cs");
            FileInfo f = new FileInfo(path);
            if (!f.Directory.Exists)
            {
                f.Directory.Create();
            }
            return f.OpenWrite();            
        }

        public Stream GetTargetStream(Func<string, Stream> targetResolver, string root, string parameterValue)
        {
            Stream s = new MemoryStream();
            if (targetResolver != null)
            {
                s = targetResolver(parameterValue);
            }
            else
            {
                string path = Path.Combine(root, string.Format("{0}.cs", parameterValue));
                FileInfo f = new FileInfo(path);
                if (!f.Directory.Exists)
                {
                    f.Directory.Create();
                }
                s = f.OpenWrite();
            }
            return s;
        }
    }
}
