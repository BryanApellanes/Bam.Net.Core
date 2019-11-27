/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bam.Net.ServiceProxy;
using DefaultNamespace;

namespace Bam.Net.Server
{
    public interface IResponder: IInitialize
    {
        string Name { get; }
        event ResponderEventHandler Responded;
        event ResponderEventHandler DidNotRespond;
        event ContentNotFoundEventHandler ContentNotFound;
        bool Respond(IHttpContext context);
        bool TryRespond(IHttpContext context);
    }
}
