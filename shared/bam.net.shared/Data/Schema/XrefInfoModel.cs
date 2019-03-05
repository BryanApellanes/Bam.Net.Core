using System;
using System.Collections.Generic;
using System.Text;

namespace Bam.Net.Data.Schema
{
    public class XrefInfoModel
    {
        public XrefInfoModel(XrefInfo xrefInfo)
        {
            Model = xrefInfo;
        }

        public XrefInfo Model { get; set; }

        public string PropertyName
        {
            get
            {
                return Model.ListTableName.Pluralize();
            }
        }
    }
}
