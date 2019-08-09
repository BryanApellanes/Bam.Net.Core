using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Bam.Net.Data.Repositories;
using Bam.Net.CoreServices.NameResolution.Data;
using CsvHelper;
using DNS.Client.RequestResolver;
using DNS.Client;
using DNS.Protocol;
using DNS.Protocol.ResourceRecords;
using Bam.Net.Logging;
using DnsClient = DNS.Client.DnsClient;

namespace Bam.Net.CoreServices.NameResolution
{
    /// <summary>
    /// A Dns name resolver that resolves A records by asking a list of root servers specified in the root-servers.csv file,
    /// </summary>
    public class DnsRootServerRequestResolver : IRequestResolver
    {
        readonly HashSet<DnsServerDescriptorClient> _clients;
        readonly Dictionary<RecordType, List<Action<IResponse>>> _recordTypeHandlers;
        public DnsRootServerRequestResolver()
        {
            _clients = new HashSet<DnsServerDescriptorClient>();
            _recordTypeHandlers = new Dictionary<RecordType, List<Action<IResponse>>>();
            
            Repository = new DatabaseRepository();
            Repository.AddType<DnsResponse>();
            Repository.AddType<RootDnsServerDescriptor>();
            
            LoadRootDnsServerData();
            AddRootServerARecordResolver();
        }
        
        public DatabaseRepository Repository { get; set; }
        
        public Task<IResponse> Resolve(IRequest request)
        {
            IResponse response = Response.FromRequest(request);

            foreach (Question question in response.Questions)
            {
                if (_recordTypeHandlers.ContainsKey(question.Type))
                {
                    if (_recordTypeHandlers.ContainsKey(question.Type))
                    {
                        Parallel.ForEach(_recordTypeHandlers[question.Type], (action) => action(response));
                    }
                    else
                    {
                        Log.Warn("No handlers are registered for question type {0}", question.Type.ToString());
                    }
                }
            }

            return Task.FromResult(response);
        }

        public void ClearHandlers(RecordType recordType)
        {
            if (_recordTypeHandlers.ContainsKey(recordType))
            {
                _recordTypeHandlers[recordType].Clear();
            }
        }
        
        public void AddHandler(RecordType recordType, Action<IResponse> responseHandler)
        {
            if (!_recordTypeHandlers.ContainsKey(recordType))
            {
                _recordTypeHandlers.Add(recordType, new List<Action<IResponse>>());
            }
            
            _recordTypeHandlers[recordType].Add(responseHandler);
                
        }
        
        protected void LoadRootDnsServerData()
        {
            string rootDnsServerData = Path.Combine(".", "root-servers.csv");
            if (!File.Exists(rootDnsServerData))
            {
                Log.Warn("root-servers.csv file not found, BaDns will not resolve public host records: {0}", rootDnsServerData);
                return;
            }
            using (StreamReader reader = new StreamReader(rootDnsServerData))
            {
                using (CsvReader csvReader = new CsvReader(reader))
                {
                    foreach (RootDnsServerDescriptor serverInfo in csvReader.GetRecords<RootDnsServerDescriptor>())
                    {
                        _clients.Add(new DnsServerDescriptorClient(serverInfo));
                    }
                }
            }
        }

        protected void  AddRootServerARecordResolver()
        {
            AddHandler(RecordType.A, response =>
            {
                Parallel.ForEach(_clients, async (client) =>
                {
                    foreach (Question question in response.Questions)
                    {
                        List<DnsResponse> cachedResponses = this.Repository
                            .Query<DnsResponse>(d => d.HostName == question.Name.ToString()).ToList();
                        if (cachedResponses.Count == 0)
                        {
                            IList<IPAddress> addresses = await client.Lookup(question.Name.ToString());
                            addresses.Each(ip =>
                            {
                                response.AnswerRecords.Add(new IPAddressResourceRecord(question.Name, ip));
                                DnsResponse cached = new DnsResponse()
                                {
                                    HostName = question.Name.ToString(), DnsServerDescriptor = client.DnsServerDescriptor,
                                    IpAddress = ip.ToString()
                                };
                                cachedResponses.Add(cached);
                                this.Repository.SaveAsync(cached);
                            });
                        }
                        cachedResponses.Each(dnsResponse => response.AnswerRecords.Add(dnsResponse.ToResourceRecord()));
                    }
                });
            });
        }
    }
}