using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Bam.Net.Data.Schema
{
    public interface IDaoTargetStreamResolver
    {
        Stream GetTargetContextStream(Func<string, Stream> targetResolver, string rootDirectory, SchemaDefinition schema);
        Stream GetTargetClassStream(Func<string, Stream> targetResolver, string rootDirectory, Table table);
        Stream GetTargetQueryClassStream(Func<string, Stream> targetResolver, string rootDirectory, Table table);
        Stream GetTargetPagedQueryClassStream(Func<string, Stream> targetResolver, string rootDirectory, Table table);
        Stream GetTargetQiClassStream(Func<string, Stream> targetResolver, string rootDirectory, Table table);
        Stream GetTargetCollectionStream(Func<string, Stream> targetResolver, string rootDirectory, Table table);
        Stream GetTargetColumnsClassStream(Func<string, Stream> targetResolver, string rootDirectory, Table table);

        Stream GetTargetPartialClassStream(Func<string, Stream> targetResolver, string rootDirectory, Table table);
    }
}
