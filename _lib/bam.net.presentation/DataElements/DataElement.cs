using System;
using System.Collections.Generic;
using System.IO;
using Bam.Net;
using Bam.Net.Application;
using Bam.Net.Data.Repositories;
using Bam.Net.Presentation.Html;

namespace bam.net.presentation.DataElements
{
    public abstract class DataElement : RenderableModel
    {
        public DataElement(object data)
        {
            Data = data;
            DataTag = new DataTag(data);
            Attributes = new {};
            Content = new object();
        }

        public bool Indent { get; set; }

        public object Content { get; }
        public virtual Tag GetTag(bool reRender = false)
        {
            if (DataTag == null || reRender)
            {
                DataTag = DataTag.Of(TagName, (object)Attributes, Content);
            }

            return DataTag;
        }

        public string TagName => GetTagName();
        public object Data { get; }

        public override string Render()
        {
            return GetTag().Render(Indent);
        }
        public dynamic Attributes { get; }
        protected DataTag DataTag { get; set; }

        protected virtual string GetTagName()
        {
            return "div";
        }
    }
}