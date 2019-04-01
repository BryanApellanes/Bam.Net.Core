using System;
using System.Collections.Generic;
using Bam.Net.CommandLine;
using Bam.Net.CoreServices.ApplicationRegistration.Data;
using Bam.Net.Logging;
using GraphQL;
using GraphQL.Types;

namespace Bam.Net.Data.GraphQL
{
    public class HostAddressGraph : ObjectGraphType<HostAddress>
    {
        public HostAddressGraph()
        {
            Field(h => h.HostName);
            
        }
    }
    
    public class MachineGraph : ObjectGraphType<Machine>
    {
        public MachineGraph()
        {
            Field("Id", m => m.Id, type: typeof(IdGraphType));
            Field("Uuid", m => m.Uuid, type: typeof(StringGraphType));
            Field("Name", m => m.Name, type: typeof(StringGraphType));
            Field("HostAddresses", m => m.HostAddresses, type: typeof(ListGraphType<HostAddressGraph>));
        }
    }

    public class GraphQueriesContext : ObjectGraphType
    {
        public GraphQueriesContext()
        {
            Field<MachineGraph>(
                name: "machines", 
                arguments: new QueryArguments(
                    new QueryArgument<IdGraphType> { Name="Id" },
                    new QueryArgument<StringGraphType> { Name = "Uuid" },
                    new QueryArgument<StringGraphType> { Name = "Name" }
                ), 
                resolve: ctx =>
                {
                    Log.Info(ctx.PropertiesToString());
                    // get all the arguments that were passed in
                    string id = ctx.GetArgument<string>("Id");
                    string uuid = ctx.GetArgument<string>("Uuid");
                    string name = ctx.GetArgument<string>("name");
                    // replace this with actual query to Repo or direct to dao
                    return new Machine()
                    {
                        Name = name, // just to show that the argument is accessible
                        HostAddresses = new List<HostAddress>()
                        {
                            new HostAddress(){HostName = "TestHostName"},
                            new HostAddress(){HostName =  "TestHN2"}
                        }
                    };
                });
        }
        
        [ConsoleAction]
        public void TestGraph()
        {
            global::GraphQL.Types.Schema schema = new global::GraphQL.Types.Schema(){Query = new GraphQueriesContext()};

            string json = schema.Execute(ql => ql.Query = @"
{ 
    machines(name: ""baloney"") {
        id 
        name
        uuid
        hostAddresses {
            hostName
        }
    } 
}");
            Console.WriteLine(json);
        }
    }
}