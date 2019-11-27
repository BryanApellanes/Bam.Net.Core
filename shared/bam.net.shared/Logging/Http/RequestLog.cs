using Bam.Net.Logging.Http.Data;
using Bam.Net.Logging.Http.Data.Dao.Repository;
using Bam.Net.ServiceProxy;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Bam.Net.Server;

namespace Bam.Net.Logging.Http
{
    public class RequestLog
    {
        public RequestLog()
        {
            HttpLoggingRepository = new HttpLoggingRepository();
            UserResolver = UserResolvers.Default;
        }

        public RequestLog(IUserResolver userResolver, HttpLoggingRepository repository)
        {
            HttpLoggingRepository = repository;
            UserResolver = userResolver;
        }

        public IUserResolver UserResolver { get; }
        public HttpLoggingRepository HttpLoggingRepository { get; }
        
        public void LogContentNotFound(IResponder responder, IHttpContext context, string[] checkedPaths = null)
        {
            try
            {
                Task.Run(() =>
                {
                    try
                    {
                        RequestData requestData = EnsureRequestData(context);
                        EnsureUserData(context);
                        ContentNotFoundData contentNotFoundData = new ContentNotFoundData
                        {
                            ResponderName = responder.Name,
                            RequestId = requestData.RequestId,
                            RequestDataId = requestData.Id,
                            CheckedPaths = string.Join("\r\n\t", checkedPaths)
                        };
                        contentNotFoundData = HttpLoggingRepository.Save<ContentNotFoundData>(contentNotFoundData);
                        Args.ThrowIfNull(contentNotFoundData, "Failed to save ContentNotFoundData", "contentNotFoundData");
                    }
                    catch (Exception ex)
                    {
                        Log.Warn("Error logging http response: {0}", ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                Log.Warn("Error logging http response: {0}", ex.Message);
            }
        }

        public void LogRequest(HttpListenerContext context)
        {
            try
            {
                Task.Run(() =>
                {
                    try
                    {
                        RequestData requestData = EnsureRequestData(context);
                        EnsureUserData(context);
                    }
                    catch (Exception ex)
                    {
                        Log.Warn("Error logging http request: {0}", ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                Log.Warn("Error logging http request: {0}", ex.Message);
            }
        }
        
        public void LogRequest(IHttpContext context)
        {
            try
            {
                Task.Run(() =>
                {
                    try
                    {
                        RequestData requestData = EnsureRequestData(context);
                        EnsureUserData(context);
                    }
                    catch (Exception ex)
                    {
                        Log.Warn("Error logging http request: {0}", ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                Log.Warn("Error logging http request: {0}", ex.Message);
            }
        }

        static object _userDataLock = new object();

        private void EnsureUserData(HttpListenerContext context)
        {
            if (context?.Request == null)
            {
                return;
                
            }

            lock (_userDataLock)
            {
                string userName = UserResolver.GetUser(new HttpContextWrapper(context));
                ulong userKey = userName.ToSha512ULong();
                HttpLoggingRepository.SetOneUserKeyDataWhere(ukd => ukd.UserName == userName && ukd.UserKey == userKey);
                HttpLoggingRepository.SetOneUserDataWhere(ud => ud.UserKey == userKey && ud.RequestId == context.Request.GetRequestId());
            }
        }
        
        private void EnsureUserData(IHttpContext context)
        {
            if (context?.Request == null)
            {
                return;
            }
            lock (_userDataLock)
            {
                string userName = UserResolver.GetUser(context);
                ulong userKey = userName.ToSha512ULong();
                HttpLoggingRepository.SetOneUserKeyDataWhere(ukd => ukd.UserName == userName && ukd.UserKey == userKey);
                HttpLoggingRepository.SetOneUserDataWhere(ud => ud.UserKey == userKey && ud.RequestId == context.Request.GetRequestId());
            }
        }

        static readonly object requestLogLock = new object();
        private RequestData EnsureRequestData(IHttpContext context)
        {
            lock (requestLogLock)
            {
                IRequest request = context.Request;
                string requestId = request.GetRequestId();
                RequestData requestData = HttpLoggingRepository.OneRequestDataWhere(rd => rd.RequestId == requestId);
                if (requestData == null)
                {
                    RequestData tmp = RequestData.FromRequest(request);
                    requestData = HttpLoggingRepository.Save<RequestData>(tmp);
                }

                Args.ThrowIfNull(requestData, "Failed to persist request data.", "requestData");
                return requestData;
            }
        }

        private RequestData EnsureRequestData(HttpListenerContext context)
        {
            return EnsureRequestData(context.Request);
        }

        private RequestData EnsureRequestData(HttpListenerRequest request)
        {
            lock (requestLogLock)
            {
                string requestId = request.GetRequestId();
                RequestData requestData = HttpLoggingRepository.OneRequestDataWhere(rd => rd.RequestId == requestId);
                if (requestData == null)
                {
                    requestData = HttpLoggingRepository.Save<RequestData>(RequestData.FromRequest(new RequestWrapper(request)));
                }

                return requestData;
            }
        }
    }
}
