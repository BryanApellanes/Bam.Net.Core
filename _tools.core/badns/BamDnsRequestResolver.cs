using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Bam.Net.Data.Repositories;
using Bam.Net.Services.Data;
using CsvHelper;
using DNS.Client.RequestResolver;
using DNS.Client;
using DNS.Protocol;
using DNS.Protocol.ResourceRecords;
using DnsClient = DNS.Client.DnsClient;

namespace Bam.Net.Application
{
    public class BamDnsRequestResolver : IRequestResolver
    {
        readonly HashSet<BamDnsClient> _clients;
        readonly Dictionary<RecordType, List<Action<IResponse>>> _recordTypeHandlers;
        public BamDnsRequestResolver()
        {
            _clients = new HashSet<BamDnsClient>();
            _recordTypeHandlers = new Dictionary<RecordType, List<Action<IResponse>>>();
            
            Repository = new DaoRepository();
            Repository.AddType<DnsResponse>();
            Repository.AddType<RootDnsServer>();
            
            LoadRootDnsServerData();
            AddRootServerARecordResolver();
        }
        
        public DaoRepository Repository { get; set; }
        
        public Task<IResponse> Resolve(IRequest request)
        {
            IResponse response = Response.FromRequest(request);

            foreach (Question question in response.Questions)
            {
                if (_recordTypeHandlers.ContainsKey(question.Type))
                {
                    Parallel.ForEach(_recordTypeHandlers[question.Type], (action) => action(response));
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
            using (StreamReader reader = new StreamReader(Path.Combine(".", "root-servers.csv")))
            {
                using (CsvReader csvReader = new CsvReader(reader))
                {
                    foreach (RootDnsServer serverInfo in csvReader.GetRecords<RootDnsServer>())
                    {
                        _clients.Add(new BamDnsClient(serverInfo));
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
                                    HostName = question.Name.ToString(), RootDnsServer = client.RootDnsServer,
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