/*
	Copyright © Bryan Apellanes 2015  
*/
using Bam.Net.ServiceProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bam.Net.Server.Rest
{
    public class RestRequest
    {
        public RestRequest(IHttpContext context)
        {
            this.Request = context.Request;
            this.Response = context.Response;
            Initialize();
        }
        private void Initialize()
        {
            string[] split = Request.Url.AbsolutePath.DelimitSplit("/", ".");
            Dictionary<string, object> queryString = new Dictionary<string, object>();
            if (Request.QueryString.Count > 0)
            {
                foreach (string key in Request.QueryString.Keys)
                {
                    queryString.Add(key, Request.QueryString[key]);
                }
            }
            Query = queryString;
            switch (HttpMethod.ToUpperInvariant())
            {
                case "POST":
                    // ** Create / POST (data in request body)**
                    // /{Type}.{ext}
                    if(split.Length == 2)
                    {
                        TypeName = split[0];
                        Extension = $".{split[1]}";
                        IsValid = true;
                    };
                    break;
                case "GET":
                    // ** Retrieve / GET **
                    // /{Type}.{ext}?{Query}
                    // /{Type}/{Id}.{ext}
                    // /{Type}/{Id}/{ChildListProperty}.{ext}                      
                    switch (split.Length)
                    {
                        case 2:
                            TypeName = split[0];
                            Extension = $".{split[1]}";
                            IsValid = true;
                            break;
                        case 3:
                            if (long.TryParse(split[1], out var id3))
                            {
                                TypeName = split[0];
                                Id = id3;
                                Extension = $".{split[2]}";
                                IsValid = true;
                            }
                            break;
                        case 4:
                            long id4;
                            if (long.TryParse(split[1], out id4))
                            {
                                TypeName = split[0];
                                Id = id4;
                                ChildListProperty = split[2];
                                Extension = $".{split[3]}";
                                IsValid = true;
                            }
                            break;
                        default:
                            IsValid = false;
                            break;
                    }
                    break;
                case "PUT":
                    // ** Update / PUT (data in request body)**
                    // /{Type}/{Id}.{ext}
                    if (long.TryParse(split[1], out var putId))
                    {
                        TypeName = split[0];
                        Id = putId;
                        Extension = $".{split[2]}";
                        IsValid = true;
                    }
                    break;
                case "DELETE":
                    // ** Delete / DELETE
                    // /{Type}/{Id}
                    if (long.TryParse(split[1], out var deleteId))
                    {
                        TypeName = split[0];
                        Id = deleteId;
                        Extension = $".{split[2]}";
                        IsValid = true;
                    }
                    break;
                default:
                    IsValid = false;
                    break;
            }
        }

        public bool IsValid { get; private set; }
        public string HttpMethod { get { return Request.HttpMethod; } }
        public IRequest Request { get; private set; }
        public IResponse Response { get; private set; }
        public string TypeName { get; private set; }
        public long Id { get; private set; }
        public string ChildListProperty { get; private set; }
        public string Extension { get; private set; }
        public Dictionary<string, object> Query { get; private set; }
        public Type GetStorableType(IEnumerable<Type> types)
        {
            return types.FirstOrDefault(t => t.Name.ToLowerInvariant().Equals(TypeName.ToLowerInvariant()));
        }
    }
}
