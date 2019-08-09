using System;
using Bam.Net.Data.Repositories;
using Bam.Net.Logging;
using Bam.Net.Server;
using Bam.Net.Services.Events;

namespace Bam.Net.Services.DataReplication.Consensus
{
    public class RaftEventSource : EventSourceService
    {
        public RaftEventSource(DaoRepository daoRepository, AppConf appConf, ILogger logger) : base(daoRepository, appConf, logger)
        {
        }

        public override void Subscribe(string eventName, EventHandler handler)
        {
            SupportedEvents.Add(eventName);
            base.Subscribe(eventName, handler);
        }
    }
}