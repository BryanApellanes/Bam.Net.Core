using System;
using System.Collections.Generic;

namespace Bam.Net.Presentation.Html
{
    public class DataTag : TypeTag
    {
        public DataTag(object data) : this(data.GetType())
        {
            
        }
        public DataTag(Type type) : base(type)
        {
        }

        public DataTag(string tagName, Func<Tag> content) : base(tagName, content)
        {
        }

        public DataTag(string tagName, object attributes, Func<Tag> content) : base(tagName, attributes, content)
        {
        }

        public DataTag(string tagName, object attributes, string content) : base(tagName, attributes, content)
        {
        }

        public DataTag(string tagName, string content) : base(tagName, content)
        {
        }

        public DataTag(string tagName, object attributes = null, object content = null) : base(tagName, attributes, content)
        {
        }

        public DataTag(string tagName, Dictionary<string, object> attributes = null, object content = null) : base(tagName, attributes, content)
        {
        }

        private object _data;

        public object Data
        {
            get { return _data; }
            set
            {
                _data = value;
                Type = _data?.GetType();
            }
        }

        public static DataTag Of(string tagName, object attributes, object content)
        {
            return new DataTag(tagName, attributes, content);
        }
        
        public static DataTag Of(string tagName, object attributes, Func<Tag> content)
        {
            return new DataTag(tagName, attributes, content);
        }
    }
}