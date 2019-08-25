using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.CoreServices.AccessControl
{
    public class ResourceUri
    {
        public ResourceUri(string uri)
        {
            Value = uri;
            Parse();
        }

        public ResourceUri(Uri uri) : this(uri.ToString())
        {
        }

        private string Value { get; }

        public override string ToString()
        {
            return Value;
        }

        public string Scheme { get; private set; }

        public string Host { get; private set; }
        public string Path { get; private set; }
        public string QueryString { get; private set; }

        string[] _pathSegments;
        public string[] PathSegments => _pathSegments;
        public Dictionary<string, object> QueryParams { get; private set; }

        private void Parse()
        {
            string scheme = Value.ReadUntil(':', out string remainder);
            Scheme = $"{scheme}://";
            if (remainder.StartsWith("//"))
            {
                remainder = remainder.TruncateFront(2);
            }
            Host = remainder.ReadUntil('/', out string pathRemainder);
            Path = $"/{pathRemainder.ReadUntil('?', out string queryRemainder)}";
            if (!string.IsNullOrEmpty(queryRemainder))
            {
                QueryString = queryRemainder;
                QueryParams = queryRemainder.QueryStringToDictionary();
            }
            _pathSegments = Path.DelimitSplit("/");
        }
    }
}
