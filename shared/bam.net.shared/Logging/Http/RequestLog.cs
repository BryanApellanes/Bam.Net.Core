using Bam.Net.Logging.Http.Data;
using Bam.Net.Logging.Http.Data.Dao.Repository;
using Bam.Net.ServiceProxy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.Logging.Http
{
    public class RequestLog
    {
        public RequestLog()
        {
            HttpLoggingRepository = new HttpLoggingRepository();
        }

        public RequestLog(IUserResolver userResolver, HttpLoggingRepository repository)
        {
            HttpLoggingRepository = repository;
            UserResolver = userResolver;
        }

        public IUserResolver UserResolver { get; }
        public HttpLoggingRepository HttpLoggingRepository { get; }

        public void Add(IHttpContext context, IResponse response, string[] checkedPaths = null)
        {
            LogRequest(context);
            LogResponse(context, response, checkedPaths);
        }
        
        public void LogResponse(IHttpContext context, IResponse response, string[] checkedPaths = null)
        {
            try
            {
                Task.Run(() =>
                {
                    try
                    {
                        ResponseData responseData = HttpLoggingRepository.Save(ResponseData.FromResponse(response));
                        string userName = UserResolver.GetUser(context);
                        ulong userKey = userName.ToSha512ULong();
                    }
                    catch (Exception ex)
                    {
                        Log.Warn("Error logging response: {0}", ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                Log.Warn("Error logging response: {0}", ex.Message);
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
                        IRequest request = context.Request;
                        RequestData requestData = HttpLoggingRepository.Save(RequestData.FromRequest(request));
                        string userName = UserResolver.GetUser(context);
                        ulong userKey = userName.ToSha512ULong();
                        HttpLoggingRepository.SetOneUserHashDataWhere(uhd => uhd.UserName == userName && uhd.UserNameHash == userKey);
                        HttpLoggingRepository.SetOneUserDataWhere(ud => ud.UserNameHash == userKey && ud.RequestCuid == requestData.Cuid);
                    }
                    catch (Exception ex)
                    {
                        Log.Warn("Error logging request: {0}", ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                Log.Warn("Error logging request: {0}", ex.Message);
            }
        }
    }
}
