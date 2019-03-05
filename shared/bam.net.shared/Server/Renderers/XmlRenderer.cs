/*
	Copyright © Bryan Apellanes 2015  
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Bam.Net;

namespace Bam.Net.Server.Renderers
{
    public class XmlRenderer: WebRenderer
    {
        public XmlRenderer()
            : base("application/xml", ".xml")
        { }

        public override void Render(object toRender, Stream output)
        {
            string xml = toRender.ToXml();

            byte[] data = Encoding.UTF8.GetBytes(xml);
            output.Write(data, 0, data.Length);
        }
    }
}
