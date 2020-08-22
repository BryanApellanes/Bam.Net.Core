using Bam.Net.Data.Repositories;
using Bam.Net.Logging;
using Bam.Net.Server;
using Bam.Net.Server.Renderers;
using Bam.Net.Server.Rest;
using Bam.Net.ServiceProxy;
using System;

namespace Bam.Net.Application
{
    public class BamDbResponder : HttpHeaderResponder, IInitialize<BamDbResponder>
    {
        public BamDbResponder(BamConf conf, ILogger logger, IRepository repository, bool verbose = false)
            : base(conf, logger)
        {
            this.DaoResponder = new DaoResponder(conf, logger);
            this.RestResponder = new RestResponder(conf, repository, logger);
        }

        public DaoResponder DaoResponder
        {
            get;
            private set;
        }

        public RestResponder RestResponder
        {
            get;
            private set;
        }

        /// <summary>
        /// ServiceProxyResponder used to respond to sql pass through requests if SqlPassThroughEnabled is true.  This may be null.
        /// </summary>
        protected ServiceProxyResponder ServiceProxyResponder
        {
            get;
            private set;
        }
        
        private bool _sqlPassThroughEnabled;
        public bool SqlPassThroughEnabled
        {
            get { return _sqlPassThroughEnabled; }
            set
            {
                _sqlPassThroughEnabled = value;
                SetSqlPassThrough();
            }
        }

        public override bool Respond(IHttpContext context)
        {
            if (!TryRespond(context))
            {
                SendResponse(context, 404, new { BamServer = "BamDb Server" });
            }
            context.Response.Close();
            return true;
        }

        public override bool TryRespond(IHttpContext context)
        {
            if (DaoResponder.MayRespond(context))
            {
                return DaoResponder.TryRespond(context);
            }
            else
            {
                if (!RestResponder.TryRespond(context))
                {
                    // the rest responder didn't respond, check if sql pass through is enabled and try to respond.
                    return SqlPassThroughEnabled && ServiceProxyResponder.TryRespond(context);
                }
                else
                {
                    return true; // rest responder responded
                }
            }
        }

        public event Action<BamDbResponder> Initializing;

        public event Action<BamDbResponder> Initialized;

        public override void Initialize()
        {
            OnInitializing();
            DaoResponder.Initialize();
            RestResponder.Initialize();
            base.Initialize();
            OnInitialized();
        }

        protected void OnInitializing()
        {
            Initializing?.Invoke(this);
        }

        protected void OnInitialized()
        {
            Initialized?.Invoke(this);
        }
        
        private void SetSqlPassThrough()
        {
            if (_sqlPassThroughEnabled)
            {
                EnableSqlPassThrough();
            }
            else
            {
                DisableSqlPassThrough();
            }
        }

        private void EnableSqlPassThrough()
        {
            //ServiceProxyResponder  = new ServiceProxyResponder();
            throw new NotImplementedException();
        }

        private void DisableSqlPassThrough()
        {
            throw new NotImplementedException();
        }

    }
}
